using System;

using Sagen.Phonetics;

namespace Sagen.Internals.Filters
{
    /// <summary>
    /// Handles filtering, voicing/noise synthesis, and articulation given a pre-generated timeline of phonation events.
    /// </summary>
	internal unsafe class PhonationFilter : Filter
    {
        private double sampleIn, sampleOut;
        
        private readonly BandPassFilter bpf1;
        private readonly BandPassFilter bpf2;
        private readonly BandPassFilter bpf3;
        private readonly BandPassFilter bpf4;
        private readonly ButterworthFilter lpOverlay, hpOverlay;

        // Attenuation for filters
        private const double LEVEL_F1 = .04000;
        private const double LEVEL_F2 = .05000;
        private const double LEVEL_F3 = .03000;
        private const double LEVEL_F4 = .02000;

        private const double LEVEL_HPO = .08;
        private const double LEVEL_LPO = .1;

        // Frequency constants
        private const double FREQ_LPO = 390;
        private const double FREQ_HPO = 7000;

        // Resonance levels
        private const double RES_F1 = .15;
        private const double RES_F2 = .075;
        private const double RES_F3 = .075;
        private const double RES_F4 = .15;
        private const double RES_HPO = .2;
        private const double RES_LPO = .25;

        // Half-bandwidths for each formant
        private const double BWH_F1 = 10.0;
        private const double BWH_F2 = 10.0;
        private const double BWH_F3 = 10.0;
        private const double BWH_F4 = 10.0;

        private const double Height = 1;
        private const double Backness = 0;
        private const double Roundedness = 0;
        private const double Rhotacization = 0;

        // Formants 3 and 4 are amplified by lerp(1.0, x, backness) where x is this constant, to simulate the "dark" quality of back vowels
        private const double BacknessF34AttenuationFactor = 0.1;

        // F3 - F2 will be multiplied by lerp(1.0, x, rhotacization) where x is this constant
        private const double RhotF3LowerFactor = 0.2;

        public PhonationFilter(Synthesizer synth, Phoneme vowel) : base(synth)
        {
            double f1 = 0.0;
            double f2 = 0.0;
            double f3 = 0.0;
            double f4 = 0.0;

            Vowel.GetFormants(Height, Backness, Roundedness, ref f1, ref f2, ref f3, ref f4);
			ApplyHeadSize(synth.Voice.HeadSize, ref f1, ref f2, ref f3, ref f4);
            Rhotacize(f2, ref f3, Rhotacization);

            Console.WriteLine($"{f1:0.00}Hz, {f2:0.00}Hz, {f3:0.00}Hz, {f4:0.00}Hz");

            bpf1 = new BandPassFilter(f1 - BWH_F1, f1 + BWH_F1, synth.SampleRate, RES_F1, RES_F1)
            {
                Volume = LEVEL_F1
            };

            bpf2 = new BandPassFilter(f2 - BWH_F2, f2 + BWH_F2, synth.SampleRate, RES_F2, RES_F2)
            {
                Volume = LEVEL_F2
            };

            bpf3 = new BandPassFilter(f3 - BWH_F3, f3 + BWH_F3, synth.SampleRate, RES_F3, RES_F3)
            {
                Volume = LEVEL_F3 * Util.Lerp(1.0, BacknessF34AttenuationFactor, Backness)
            };

            bpf4 = new BandPassFilter(f4 - BWH_F4, f4 + BWH_F4, synth.SampleRate, RES_F4, RES_F4)
            {
                Volume = LEVEL_F4 * Util.Lerp(1.0, BacknessF34AttenuationFactor, Backness)
            };            

            lpOverlay = new ButterworthFilter(FREQ_LPO, synth.SampleRate, PassFilterType.LowPass, RES_LPO);
            hpOverlay = new ButterworthFilter(FREQ_HPO, synth.SampleRate, PassFilterType.HighPass, RES_HPO);
        }

        private static void Rhotacize(double f2, ref double f3, double rf)
        {
            f3 = f2 + (f3 - f2) * Util.Lerp(1.0, RhotF3LowerFactor, rf);
        }

		private static void ApplyHeadSize(double headSize, ref double f1, ref double f2, ref double f3, ref double f4)
		{
			double factor = 1.0 / headSize;
			f1 *= factor;
			f2 *= factor;
			f3 *= factor;
			f4 *= factor;
		}

        public override void Update(ref double sample)
        {
            synth.Pitch -= 0.1f * synth.TimeStep;
            sampleOut = 0f;

            sampleIn = sample;
            sampleIn += NoiseFilter.NoiseDataPointer[synth.Position % NoiseFilter.NoiseDataLength] * 0.11;

            // Update filters
            bpf1.Update(sampleIn);
            bpf2.Update(sampleIn);
            bpf3.Update(sampleIn);
            bpf4.Update(sampleIn);
            lpOverlay.Update(sampleIn);
            hpOverlay.Update(sampleIn);

            sampleOut += bpf1.Value;
            sampleOut += bpf2.Value;
            sampleOut += bpf3.Value;
            sampleOut += bpf4.Value;
            sampleOut += lpOverlay.Value * LEVEL_LPO;
            sampleOut += hpOverlay.Value * LEVEL_HPO * Util.Lerp(1.0, BacknessF34AttenuationFactor, Backness);

            sample = sampleOut;
        }
    }
}