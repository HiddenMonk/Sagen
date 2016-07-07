using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

using Sagen.Samplers;

namespace Sagen
{
	internal class Synthesizer
	{
		public const int MaxSamplers = 128;
		public const SampleFormat PlaybackFormat = SampleFormat.Signed16;
		public const int PlaybackFormatBytes = (int)PlaybackFormat / 8;
		private const double StreamChunkDurationSeconds = 0.1;

		private readonly HashSet<Sampler> samplers = new HashSet<Sampler>();
		private readonly List<Sampler> samplerSequence = new List<Sampler>();

		private readonly int _sampleRate = (int)TTS.Quality;
		private readonly VoiceQuality _quality = TTS.Quality;
		private int _position = 0;
		private readonly VoiceParams _voice = VoiceParams.Jimmy;
		private readonly TTS _tts;
		private AudioStream _audioStream;

		public double Fundamental { get; set; } = 100.0f;
		public double TimePosition => (double)_position / _sampleRate;
		public double TimeStep { get; }

		public VoiceParams Voice => _voice;

		internal TTS TTS => _tts;
		
		public AudioStream AudioStream => _audioStream;

		public Synthesizer(TTS engine)
		{
			TimeStep = 1.0f / _sampleRate;
			_tts = engine;
		}

		/// <summary>
		/// This method is used to create the audio stream and its accompanying ManualResetEventSlim ahead of time so that TTS.Sync works properly with multithreading.
		/// </summary>
		public void CreateAudioStream()
		{
			if (_audioStream == null)
			{
				_audioStream = new AudioStream(PlaybackFormat, this);
			}
		}

		public int SampleRate => _sampleRate;

		public VoiceQuality Quality => _quality;

		public int Position
		{
			get { return _position; }
			set { _position = value; }
		}

		public void AddSampler(Sampler sampler)
		{
			if (sampler != null && samplers.Add(sampler))
			{
				samplerSequence.Add(sampler);
			}
		}

		public void Generate(double lengthSeconds, Stream output, SampleFormat format, bool includeWavHeader = true)
		{
			using (var writer = new BinaryWriter(output, Encoding.Default, true))
			{
				int sampleCount = (int)(SampleRate * lengthSeconds);

				if (includeWavHeader)
					Wav.GenerateWavHeader(this, output, sampleCount, format);

				for (_position = 0; _position < sampleCount; _position++)
				{
					double currentSample = 0f;
					foreach (var sampler in samplerSequence)
					{
						if (!sampler.Enabled) continue;
						sampler.Update(ref currentSample);
					}
					switch (format)
					{
						case SampleFormat.Float64:
							writer.Write(currentSample);
							break;
						case SampleFormat.Float32:
							writer.Write((float)currentSample);
							break;
						case SampleFormat.Signed16:
							writer.Write((short)(currentSample * short.MaxValue));
							break;
						case SampleFormat.Unsigned8:
							writer.Write((byte)((currentSample + 1.0f) / 2.0f * byte.MaxValue));
							break;
					}
				}
				writer.Flush();
			}
		}

		public void Play(double lengthSeconds)
		{
			CreateAudioStream();
			int blockSize = (int)(SampleRate * StreamChunkDurationSeconds) * PlaybackFormatBytes;
			int sampleCount = (int)(SampleRate * lengthSeconds);
			using (var stream = new MemoryStream(blockSize))
			using (var writer = new BinaryWriter(stream))
			{
				for (_position = 0; _position < sampleCount; _position++)
				{
					double currentSample = 0f;
					foreach (var sampler in samplerSequence)
					{
						if (!sampler.Enabled) continue;
						sampler.Update(ref currentSample);
					}

					writer.Write((short)(currentSample * short.MaxValue));
					if (stream.Position >= blockSize)
					{
						_audioStream.QueueDataBlock(stream);
					}
				}
				if (stream.Position > 0)
					_audioStream.QueueDataBlock(stream);
			}
			_audioStream.MarkFullyQueued();
		}
	}
}