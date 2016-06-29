using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using HAARP.Samplers;

namespace HAARP
{
	internal class Synthesizer
	{
		public const int MaxSamplers = 128;

		private readonly HashSet<Sampler> samplers = new HashSet<Sampler>();
		private readonly List<Sampler> samplerSequence = new List<Sampler>();

		private readonly int _sampleRate = (int)TTS.Quality;
		private readonly VoiceQuality _quality = TTS.Quality;
		private int _position = 0;
		private readonly VoiceParams _voice = VoiceParams.Jimmy;

		public float Fundamental { get; set; } = 100.0f;
		public float TimePosition => (float)_position / _sampleRate;
		public float TimeStep { get; }

		public VoiceParams Voice => _voice;

		public Synthesizer()
		{
			TimeStep = 1.0f / _sampleRate;
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

		public void Generate(float lengthSeconds, Stream output, SampleFormat format, bool includeWavHeader = true)
		{
			using (var writer = new BinaryWriter(output, Encoding.Default, true))
			{
				int sampleCount = (int)(SampleRate * lengthSeconds);

				if (includeWavHeader)
					Wav.GenerateWavHeader(this, output, sampleCount, format);

				for (_position = 0; _position < sampleCount; _position++)
				{
					float currentSample = 0f;
					foreach (var sampler in samplerSequence)
					{
						if (!sampler.Enabled) continue;
						sampler.Update(ref currentSample);
					}
					switch (format)
					{
						case SampleFormat.Float32:
							writer.Write(currentSample);
							break;
						case SampleFormat.Signed16:
							writer.Write((short)(currentSample * short.MaxValue));
							break;
						case SampleFormat.Unsigned8:
							writer.Write((byte)((currentSample + 1.0f) / 2.0f * 255));
							break;
					}
				}
				writer.Flush();
			}
		}
	}
}