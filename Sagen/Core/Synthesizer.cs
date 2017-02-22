using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

using Sagen.Core.Audio;
using Sagen.Core.Layers;
using Sagen.Core.Nodes;
using Sagen.Extensibility;

namespace Sagen.Core
{
	/// <summary>
	/// Handles state updates and audio generation for a voice synthesizer instance.
	/// </summary>
	internal class Synthesizer : ISynthesizer
	{
		public const int MaxSamplers = 128;
		public const SampleFormat PlaybackFormat = SampleFormat.Signed16;
		public const int PlaybackFormatBytes = (int)PlaybackFormat / 8;
		private const double StreamChunkDurationSeconds = 0.1;

		private readonly HashSet<Layer> samplers = new HashSet<Layer>();
		private readonly List<Layer> samplerSequence = new List<Layer>();
		private readonly int _sampleRate = (int)TTS.Quality;
		private readonly VoiceQuality _quality = TTS.Quality;
		private readonly TTS _tts;
		private readonly SpeechTimeline _timeline;
		private int _position = 0;

		/// <summary>
		/// Pitch, measured in relative octaves.
		/// </summary>
		public double Pitch { get; set; } = 0.0f;

		/// <summary>
		/// Fundamental frequency calculated from gender, pitch, and intonation.
		/// </summary>
		public double Fundamental { get; set; } = 100.0f;

		/// <summary>
		/// The time, in seconds, already elapsed before the current sample
		/// </summary>
		public double TimePosition => (double)_position / _sampleRate;

		/// <summary>
		/// The amount of time, in seconds, elapsed per sample
		/// </summary>
		public double TimeStep { get; }

		public VocalState State { get; }

		public Voice Voice => _tts.Voice;

		internal TTS TTS => _tts;

		public Synthesizer(TTS engine, SpeechTimeline timeline)
		{
			TimeStep = 1.0f / _sampleRate;
			_tts = engine;
			_timeline = timeline;
			State = new VocalState();
		}

		public int SampleRate => _sampleRate;

		public VoiceQuality Quality => _quality;

		public SpeechTimeline Timeline => _timeline;

		public int Position
		{
			get { return _position; }
			set { _position = value; }
		}

		public void AddSampler(Layer sampler)
		{
			if (sampler != null && samplers.Add(sampler))
			{
				samplerSequence.Add(sampler);
			}
		}

		public void Generate(Stream output, SampleFormat format, bool includeWavHeader = true)
		{
			unchecked
			{
				using (var writer = new BinaryWriter(output, Encoding.Default, true))
				{
					int sampleCount = (int)(_sampleRate * _timeline.LengthSeconds);

					if (includeWavHeader)
						Wav.GenerateWavHeader(this, output, sampleCount, format);

					var nodeEnumerator = _timeline.GetEnumerator();
					nodeEnumerator.MoveNext();

					for (_position = 0; _position < sampleCount; _position++)
					{
						// Traverse nodes
						if (nodeEnumerator.Current == null) break;
						while (TimePosition >= nodeEnumerator.Current.EndTime)
						{
							nodeEnumerator.Current.OnExit(this);
							State.LastGlottisLevel = State.GlottisLevel;
							if (!nodeEnumerator.MoveNext()) goto done;
							nodeEnumerator.Current.OnEnter(this);
						}

						nodeEnumerator.Current.OnUpdate(this);

						// Run synthesizer on current sample
						double currentSample = 0f;
						foreach (var sampler in samplerSequence)
						{
							if (!sampler.Enabled) continue;
							sampler.Update(ref currentSample);
						}

						// Convert sample to desired format and write to stream
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

					done:

					writer.Flush();
					nodeEnumerator.Dispose();
				}
			}
		}

		public void Play<TPlaybackEngine>(TTS<TPlaybackEngine> ttsPlayback) where TPlaybackEngine : IPlaybackEngine, new()
		{
			var playback = new TPlaybackEngine();
			ttsPlayback.AddActiveAudio(playback);
			ThreadPool.QueueUserWorkItem(o =>
			{
				int blockSize = (int)(SampleRate * StreamChunkDurationSeconds) * PlaybackFormatBytes;
				var data = new short[blockSize];
				int len = 0;

				var nodeEnumerator = _timeline.GetEnumerator();
				nodeEnumerator.MoveNext();

				while (nodeEnumerator.Current != null)
				{
					// Traverse nodes
					while (TimePosition >= nodeEnumerator.Current.EndTime)
					{
						nodeEnumerator.Current.OnExit(this);
						State.LastGlottisLevel = State.GlottisLevel;
						if (!nodeEnumerator.MoveNext()) goto done;
						nodeEnumerator.Current.OnEnter(this);
					}
					
					nodeEnumerator.Current.OnUpdate(this);

					// Run synthesizer on current sample
					double currentSample = 0f;
					foreach (var sampler in samplerSequence)
					{
						if (!sampler.Enabled) continue;
						sampler.Update(ref currentSample);
					}

					data[len++] = unchecked((short)(currentSample * short.MaxValue));

					// If a chunk is completed, push it out
					if (len >= blockSize)
					{
						playback.QueueDataBlock(data, len, _sampleRate);
						len = 0;
					}

					_position++;
				}
				done:
				// Push out any remaining chunk, even if it's not full
				if (len > 0) playback.QueueDataBlock(data, len, _sampleRate);

				playback.MarkComplete(() => ttsPlayback.RemoveActiveAudio(playback));

				nodeEnumerator.Dispose();
			});
		}
	}
}