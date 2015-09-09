using System;

namespace HAARP.Samplers
{
    internal class NoiseSampler : Sampler
    {
        private readonly BandPassFilter filter;

        public float MinFrequency { get; set; } = 0.0f;
        public float MaxFrequency { get; set; } = 2000.0f;
        public float Amplitude { get; set; } = 1.0f;

        private readonly RNG rng;

        public NoiseSampler(Synthesizer synth, long seed, float minFrequency, float maxFrequency, float filterResonance = .3f) : base(synth)
        {
            rng = new RNG(seed);
            MinFrequency = minFrequency;
            MaxFrequency = maxFrequency;
            filter = new BandPassFilter(maxFrequency, minFrequency, 44100, filterResonance, filterResonance);
        }

        public override void Update(ref float sample)
        {
            filter.Update(rng.NextSingle(-1f, 1f) * Amplitude);
            sample += filter.Value;
        }
    }
}