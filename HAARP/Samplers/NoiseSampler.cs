using System;

namespace HAARP.Samplers
{
    internal class NoiseSampler : Sampler
    {
        private readonly ButterworthFilter lowPassFilter;
        private readonly ButterworthFilter highPassFilter;

        public float MinFrequency { get; set; } = 0.0f;
        public float MaxFrequency { get; set; } = 2000.0f;
        public float Amplitude { get; set; } = 1.0f;

        private readonly RNG rng;

        public NoiseSampler(long seed, float filterResonance = .3f)
        {
            rng = new RNG(seed);
            lowPassFilter = new ButterworthFilter(MaxFrequency, 44100, PassFilterType.LowPass, filterResonance);
            highPassFilter = new ButterworthFilter(MinFrequency, 44100, PassFilterType.HighPass, filterResonance);
        }

        public override void Update(Synthesizer synth, ref float sample)
        {
            lowPassFilter.Update(rng.NextSingle(-1f, 1f));
            highPassFilter.Update(lowPassFilter.Value);
            sample += highPassFilter.Value * Amplitude;
        }
    }
}