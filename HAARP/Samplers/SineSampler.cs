using System;

namespace HAARP.Samplers
{
    internal class SineSampler : Sampler
    {
        private double state;
        private const double FullPhase = Math.PI * 2.0;

        /// <summary>
        /// The frequency in Hertz.
        /// </summary>
        public float Frequency { get; set; } = 1000;

        /// <summary>
        /// The amplitude of the wave.
        /// </summary>
        public float Amplitude { get; set; } = 0.5f;

        /// <summary>
        /// The phase offset of the wave.
        /// </summary>
        public float Phase { get; } = 0.0f;

        /// <summary>
        /// The DC (vertical) offset of the wave.
        /// </summary>
        public float DCOffset { get; set; } = 0.0f;

        /// <summary>
        /// The spectral tilt of the wave.
        /// </summary>
        public float SpectralTilt { get; set; } = 0.0f;

        /// <summary>
        /// The nyquist frequency to use for spectral tilting.
        /// </summary>
        public float SpectralUpperBound { get; set; } = 22050.0f;

        public SineSampler(Synthesizer synth) : base(synth)
        {
            
        }

        public SineSampler(Synthesizer synth, float frequency, float amplitude, float phase = 0.0f, float dcOffset = 0.0f) : base(synth)
        {
            Frequency = frequency;
            Amplitude = amplitude;
            Phase = phase;
            state = phase;
            DCOffset = dcOffset;
        }

        public override void Update(ref float sample)
        {
            state = (state + synth.TimeStep * Frequency) % 1.0f;
            sample += ((float)Math.Sin(state * FullPhase) * Amplitude).Tilt(Frequency, SpectralTilt, SpectralUpperBound) + DCOffset;
        }
    }
}