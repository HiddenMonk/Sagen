using System.Collections.Generic;

using HAARP.Samplers;

namespace HAARP
{
	internal class Synthesizer
	{
		public const int MaxSamplers = 72;

		private readonly HashSet<Sampler> samplers = new HashSet<Sampler>();
		private readonly LinkedList<Sampler> samplerSequence = new LinkedList<Sampler>();

		private readonly int _sampleRate = (int)TTS.Quality;
		private readonly VoiceQuality _quality = TTS.Quality;
		private int _position = 0;
		private readonly float[] _buffer;
		private readonly VoiceParams _voice = VoiceParams.Jimmy;

		public float Fundamental { get; set; } = 100.0f;
		public float TimePosition => (float)_position / _sampleRate;
		public float TimeStep { get; }
		public float Length => _buffer.Length / (float)_sampleRate;

		public VoiceParams Voice => _voice;

		public Synthesizer(float seconds)
		{
			_buffer = new float[(int)(seconds * _sampleRate)]; // Use a buffer pool later
			TimeStep = 1.0f / _sampleRate;
		}

		private static void ConfigureTransform(float sl, float su, float dl, float du, out float offset, out float mult)
		{
			mult = (su - sl) / (du - dl);
			offset = dl - sl;
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
				samplerSequence.AddLast(sampler);
			}
		}

		public float[] Generate()
		{
			int ln = _buffer.Length;
			foreach (var sampler in samplerSequence)
			{
				if (!sampler.Enabled) continue;
				for (_position = 0; _position < ln; _position++)
				{
					sampler.Update(ref _buffer[_position]);
				}
			}

			return _buffer;
		}
	}
}