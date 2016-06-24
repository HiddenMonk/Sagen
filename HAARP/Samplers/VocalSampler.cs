namespace HAARP.Samplers
{
	internal class VocalSampler : Sampler
	{
		private readonly RNG rng;
		private float rawSample;

		private readonly VoiceData _voice;
		private readonly BandPassFilter[] bands;
		private readonly int numBands;

		private const float LowResonance = .25f;
		private const float HighResonance = .125f;

		public VocalSampler(Synthesizer synth, long seed) : base(synth)
		{
			rng = new RNG(seed);
			bands = new[]
			{
				// ʌ
				//new BandPassFilter(2700, 2700, synth.SampleRate, HighResonance, HighResonance) {Volume = .005f},
				//new BandPassFilter(1170, 1170, synth.SampleRate, HighResonance, HighResonance) {Volume = .075f},
				//new BandPassFilter(600, 600, synth.SampleRate, LowResonance, LowResonance) {Volume = .075f},

				// ɛ
				//new BandPassFilter(2600, 2600, synth.SampleRate, HighResonance, HighResonance) {Volume = .125f},
				//new BandPassFilter(1900, 1900, synth.SampleRate, HighResonance, HighResonance) {Volume = .125f},
				//new BandPassFilter(610, 610, synth.SampleRate, LowResonance, LowResonance) {Volume = .125f},

				// Harmonic 1 boost
				//new BandPassFilter(140, 200, synth.SampleRate, LowResonance, LowResonance) {Volume = .025f},

				// Nasal resonance
				//new BandPassFilter(2500, 4000, synth.SampleRate, HighResonance, HighResonance) {Volume = 0.75f}, 
				 
				// shhhhhhhhh
                new BandPassFilter(200, 1200, synth.SampleRate, 1.0f, 1.0f) { Volume = 0.075f }, 
                new BandPassFilter(1900, 5500, synth.SampleRate, .35f, .35f) { Volume = 0.01f }, 

                new BandPassFilter(6700, 7200, synth.SampleRate, .1f, .1f) { Volume = 0.0060f }, 
                new BandPassFilter(4600, 5400, synth.SampleRate, .18f, .18f) { Volume = 0.060f }, 
                new BandPassFilter(3700, 4100, synth.SampleRate, .2f, .2f) { Volume = 0.050f }, 
                new BandPassFilter(2100, 2500, synth.SampleRate, .2f, .2f) { Volume = 0.1f }
            };

			numBands = bands.Length;
			_voice = VoiceData.Get(Voice.Jimmy);
		}

		public override void Update(ref float sample)
		{
			rawSample += rng.NextSingle(-1, 1) * synth.Voice.FricativeForce * 0.06f;
			//rawSample += _voice.Sample(synth.Position, synth.Quality) * 0.1f;

			// Update filters
			for (int i = 0; i < numBands; i++)
			{
				bands[i].Update(rawSample);
				sample += bands[i].Value;
			}
		}
	}
}