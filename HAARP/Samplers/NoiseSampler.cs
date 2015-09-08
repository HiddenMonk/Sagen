using System;

namespace HAARP.Samplers
{
    internal class NoiseSampler : Sampler
    {
        private readonly ButterworthFilter lowPassFilter;
        private readonly ButterworthFilter highPassFilter;

        public float LowerBound { get; set; } = 1000.0f;
        public float UpperBound { get; set; } = 2000.0f;
        public float Amplitude { get; set; } = 1.0f;

        private readonly RNG rng;

        public NoiseSampler(long seed, float filterResonance = .3f)
        {
            rng = new RNG(seed);
            lowPassFilter = new ButterworthFilter(UpperBound, 44100, ButterworthFilter.PassType.Lowpass, filterResonance);
            highPassFilter = new ButterworthFilter(LowerBound, 44100, ButterworthFilter.PassType.Highpass, filterResonance);
        }

        public override void Update(Synthesizer synth, ref float sample)
        {
            lowPassFilter.Update(rng.NextSingle(-1f, 1f));
            highPassFilter.Update(lowPassFilter.Value);
            sample += highPassFilter.Value * Amplitude;
        }
    }
}