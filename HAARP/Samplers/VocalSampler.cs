using System;
using System.Text.RegularExpressions;

namespace HAARP.Samplers
{
	internal unsafe class VocalSampler : Sampler
	{
		private float sampleIn, sampleOut;

		private readonly VoiceData _voice;
		private readonly BandPassFilter[] bands;
		private readonly int numBands;

		private const float LowResonance = .2f;
		private const float HighResonance = 0.09f;

		private const float LEVEL_F1 = .0175f;
		private const float LEVEL_F2 = .0115f;
		private const float LEVEL_F3 = .0105f;
		private const float LEVEL_F4 = .005f;

		public VocalSampler(Synthesizer synth, long seed) : base(synth)
		{
			bands = new[]
			{
				// ʌ
				//new BandPassFilter(2600, 2700, synth.SampleRate, HighResonance, HighResonance) {Volume = .005f},
				//new BandPassFilter(1170, 1170, synth.SampleRate, HighResonance, HighResonance) {Volume = .01f},
				//new BandPassFilter(600, 600, synth.SampleRate, LowResonance, LowResonance) {Volume = .01f},

				// ɛ
				new BandPassFilter(3500, 8000, synth.SampleRate, HighResonance, HighResonance) {Volume = LEVEL_F4}, 
				new BandPassFilter(2600, 2850, synth.SampleRate, HighResonance, HighResonance) {Volume = LEVEL_F3},
				new BandPassFilter(1900, 2150, synth.SampleRate, HighResonance, HighResonance) {Volume = LEVEL_F2},
				new BandPassFilter(650, 800, synth.SampleRate, HighResonance, HighResonance) {Volume = LEVEL_F1},

				// ü
				//new BandPassFilter(100, 24000, synth.SampleRate, HighResonance, HighResonance) {Volume = .0125f}, 
				//new BandPassFilter(3200, 3300, synth.SampleRate, HighResonance, HighResonance) {Volume = .0125f},
				//new BandPassFilter(2100, 2200, synth.SampleRate, HighResonance, HighResonance) {Volume = .0125f},
				//new BandPassFilter(1800, 1900, synth.SampleRate, HighResonance, HighResonance) {Volume = .0125f},
				//new BandPassFilter(200, 300, synth.SampleRate, LowResonance, LowResonance) {Volume = .0125f},
				
				// near-open front unrounded vowel
				//new BandPassFilter(3100, 3200, synth.SampleRate, HighResonance, HighResonance) {Volume = LEVEL_F3},
				//new BandPassFilter(1650, 1650, synth.SampleRate, HighResonance, HighResonance) {Volume = LEVEL_F2},
				//new BandPassFilter(860, 860, synth.SampleRate, HighResonance, HighResonance) {Volume = LEVEL_F1},
				new BandPassFilter(500, 6500, synth.SampleRate, HighResonance, HighResonance) {Volume = .0075f}, 
				 
				// RRRRRRR
				//new BandPassFilter(3100, 3100, synth.SampleRate, HighResonance, HighResonance) {Volume = .0025f},
				//new BandPassFilter(1500, 1500, synth.SampleRate, HighResonance, HighResonance) {Volume = .0125f},
				//new BandPassFilter(350, 350, synth.SampleRate, LowResonance, LowResonance) {Volume = .0125f},

				// i
				//new BandPassFilter(3000, 3000, synth.SampleRate, HighResonance, HighResonance) {Volume = LEVEL_F3},
				//new BandPassFilter(2250, 2250, synth.SampleRate, HighResonance, HighResonance) {Volume = LEVEL_F2},
				//new BandPassFilter(250, 250, synth.SampleRate, HighResonance, HighResonance) {Volume = LEVEL_F1},

				// shhhhhhhhh
                //new BandPassFilter(200, 1200, synth.SampleRate, 1.0f, 1.0f) { Volume = 0.075f }, 
                //new BandPassFilter(1900, 5500, synth.SampleRate, .35f, .35f) { Volume = 0.01f },
				//new BandPassFilter(2100, 2500, synth.SampleRate, .2f, .2f) { Volume = 0.1f },
				//new BandPassFilter(3700, 4100, synth.SampleRate, .2f, .2f) { Volume = 0.050f },
				//new BandPassFilter(4600, 5400, synth.SampleRate, .18f, .18f) { Volume = 0.060f },
				//new BandPassFilter(6700, 7200, synth.SampleRate, .1f, .1f) { Volume = 0.0060f }, 
            };

			numBands = bands.Length;
			_voice = VoiceData.Get(Voice.Jimmy);
		}

		public override void Update(ref float sample)
		{
			synth.Fundamental -= 25f * synth.TimeStep;
			sampleOut = 0f;

			// ha ha ha ha ha ha
			float m = ((float)Math.Sin(synth.TimePosition * Math.PI * 8.0f) + 1.0f) / 2.0f;
			sampleIn = sample * m
				+ NoiseSampler.NoiseDataPointer[synth.Position % NoiseSampler.NoiseDataLength] * synth.Voice.FricativeForce * (0.15f + 0.95f * (float)Math.Pow(1.0f - m, 2));

			// Update filters
			for (int i = 0; i < numBands; i++)
			{
				bands[i].Update(sampleIn);
				sampleOut += bands[i].Value;
			}

			sample = sampleOut;
		}
	}
}