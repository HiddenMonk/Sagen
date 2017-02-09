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
        private const double LEVEL_F1 = .03800;
        private const double LEVEL_F2 = .03200;
        private const double LEVEL_F3 = .02500;
        private const double LEVEL_F4 = .05000;

        private const double LEVEL_HPO = .04;
        private const double LEVEL_LPO = .13;

        // Frequency constants
        private const double FREQ_LPO = 500;
        private const double FREQ_HPO = 4200;

        // Resonance levels
        private const double RES_F1 = .08;
        private const double RES_F2 = .08;
        private const double RES_F3 = .09;
        private const double RES_F4 = .125;

        private const double RES_HPO = .35;
        private const double RES_LPO = .15;

        // Half-bandwidths for each formant
        private const double BWH_F1 = 50.0;
        private const double BWH_F2 = 25.0;
        private const double BWH_F3 = 150.0;
        private const double BWH_F4 = 250.0;

        private const double HeadSizeFormantStretchFactor = 0.8;

        private const double Height = 1;
        private const double Backness = 0;
        private const double Roundedness = 0.0;
        private const double HeadSize = 1;
        private const double Rhotacization = 0.0;

        // Formants 3 and 4 are amplified by lerp(1.0, x, backness) where x is this constant, to simulate the "dark" quality of back vowels
        private const double BacknessF34AttenuationFactor = 0.25;

        // F3 - F2 will be multiplied by lerp(1.0, x, rhotacization) where x is this constant
        private const double RhotF3LowerFactor = 0.2;


        public PhonationFilter(Synthesizer synth, Phoneme vowel) : base(synth)
        {
            double f1 = 0.0;
            double f2 = 0.0;
            double f3 = 0.0;
            double f4 = 0.0;
            double hsOffset = HeadSizeToFormantOffset(HeadSize);

            Vowel.GetFormants(Height, Backness, Roundedness, ref f1, ref f2, ref f3, ref f4);

            // Apply offset from vocal tract length
            f1 += hsOffset;
            f2 += hsOffset * (1.0 + HeadSizeFormantStretchFactor);
            f3 += hsOffset * (1.0 + HeadSizeFormantStretchFactor * 2);
            f4 += hsOffset * (1.0 + HeadSizeFormantStretchFactor * 3);

            // Apply rhotacization
            Rhotacize(f2, ref f3, Rhotacization);

            Console.WriteLine($"{f1}Hz, {f2}Hz, {f3}Hz, {f4}Hz");

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

        private static double HeadSizeToFormantOffset(double headSize)
        {
            return -150.0 * Math.Log(headSize, 2.0);
        }

        public override void Update(ref double sample)
        {
            //synth.Fundamental -= 10.0f * synth.TimeStep;
            sampleOut = 0f;

            sampleIn = sample;
            sampleIn -= NoiseFilter.NoiseDataPointer[synth.Position % NoiseFilter.NoiseDataLength] * 0.2;

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
            //sampleOut += hpOverlay.Value * LEVEL_HPO * Util.Lerp(1.0, BacknessF34AttenuationFactor, Backness);

            sample = sampleOut;
        }
    }
}