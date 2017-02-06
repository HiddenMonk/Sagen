using System;

using Sagen.Phonetics;

namespace Sagen.Internals.Samplers
{
    /// <summary>
    /// Handles filtering, voicing/noise synthesis, and articulation given a pre-generated timeline of phonation events.
    /// </summary>
	internal unsafe class VocalSampler : Sampler
	{
		private double sampleIn, sampleOut;

		private readonly BandPassFilter[] bands;
		private readonly ButterworthFilter lowPass, highPass;
		private readonly int numBands;

        // Constants which control the attenuation of each individual formant
		private const double LEVEL_F1 = .02500;
		private const double LEVEL_F2 = .03800;
		private const double LEVEL_F3 = .02200;
		private const double LEVEL_F4 = .00150;
		private const double LEVEL_F5 = .00025;
        
        // Resonance levels for each formant
		private const double RES_F1 = .22;
		private const double RES_F2 = .08;
		private const double RES_F3 = .09;
		private const double RES_F4 = .2;
		private const double RES_F5 = .23;

        private const double Height = .25;
        private const double Backness = 0;
        private const double Roundedness = 0;
        private const double HeadSize = 1;

        // Formants 3 and 4 are amplified by lerp(1.0, x, backness) where x is this constant, to simulate the "dark" quality of back vowels
        private const double BacknessF34AttenuationFactor = 0.25;


        public VocalSampler(Synthesizer synth, Phoneme vowel) : base(synth)
		{
            double f1 = 0.0;
            double f2 = 0.0;
            double f3 = 0.0;
            double f4 = 0.0;
            double hsOffset = HeadSizeToFormantOffset(HeadSize);

            Vowel.GetFormants(Height, Backness, Roundedness, ref f1, ref f2, ref f3, ref f4);

            f1 += hsOffset;
            f2 += hsOffset;
            f3 += hsOffset;
            f4 += hsOffset;

            Console.WriteLine($"{f1}, {f2}, {f3}, {f4}");

			bands = new[]
			{ 
				new BandPassFilter(f4, f4, synth.SampleRate, RES_F4, RES_F4) { Volume = LEVEL_F4 * Util.Lerp(1.0, BacknessF34AttenuationFactor, Backness) },
				new BandPassFilter(f3, f3, synth.SampleRate, RES_F3, RES_F3) { Volume = LEVEL_F3 * Util.Lerp(1.0, BacknessF34AttenuationFactor, Backness) },
				new BandPassFilter(f2, f2, synth.SampleRate, RES_F2, RES_F2) { Volume = LEVEL_F2 },
				new BandPassFilter(f1, f1, synth.SampleRate, RES_F1, RES_F1) { Volume = LEVEL_F1 },
            };

			lowPass = new ButterworthFilter(500, synth.SampleRate, PassFilterType.LowPass, .15f);
			highPass = new ButterworthFilter(6500, synth.SampleRate, PassFilterType.HighPass, .2f);

			numBands = bands.Length;
		}

        private static double HeadSizeToFormantOffset(double headSize)
        {
            return -150.0 * Math.Log(headSize, 2.0);
        }

		public override void Update(ref double sample)
		{
			synth.Fundamental -= 20.0f * synth.TimeStep;
			sampleOut = 0f;

            // ha ha ha ha ha ha
            //double m = (Math.Sin(synth.TimePosition * Math.PI * 8.0f) + 1.0f) / 2.0f;
            //sampleIn = sample * m
            //	+ NoiseSampler.NoiseDataPointer[synth.Position % NoiseSampler.NoiseDataLength] * synth.Voice.FricativeForce * (0.2f + 0.55f * Math.Pow(1.0f - m, 2));

            sampleIn = sample;
            //sampleIn += NoiseSampler.NoiseDataPointer[synth.Position % NoiseSampler.NoiseDataLength] * synth.Voice.FricativeForce * 0.35;

            // Update filters
            for (int i = 0; i < numBands; i++)
			{
				bands[i].Update(sampleIn);
				sampleOut += bands[i].Value;
			}

			lowPass.Update(sampleIn);
			highPass.Update(sampleIn);
			sampleOut += lowPass.Value * .13f;
			sampleOut += highPass.Value * .02f * Util.Lerp(1.0, BacknessF34AttenuationFactor, Backness);

			sample = sampleOut;
		}
	}
}