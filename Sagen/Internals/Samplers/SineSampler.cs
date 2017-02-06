using System;

namespace Sagen.Internals.Samplers
{
    internal class SineSampler : Sampler
    {
        private double state;
        private const double FullPhase = Math.PI * 2.0;

        /// <summary>
        /// The frequency in Hertz.
        /// </summary>
        public double Frequency { get; set; } = 1000.0;

        /// <summary>
        /// The amplitude of the wave.
        /// </summary>
        public double Amplitude { get; set; } = 0.5;

        /// <summary>
        /// The phase offset of the wave.
        /// </summary>
        public double Phase { get; } = 0.0f;

        /// <summary>
        /// The DC (vertical) offset of the wave.
        /// </summary>
        public double DCOffset { get; set; } = 0.0f;

        public SineSampler(Synthesizer synth) : base(synth)
        {
            
        }

        public SineSampler(Synthesizer synth, double frequency, double amplitude, double phase = 0.0f, double dcOffset = 0.0f) : base(synth)
        {
            Frequency = frequency;
            Amplitude = amplitude;
            Phase = phase;
            state = phase;
            DCOffset = dcOffset;
        }

        public override void Update(ref double sample)
        {
            state = (state + synth.TimeStep * Frequency) % 1.0f;
            sample += (Math.Sin(state * FullPhase) * Amplitude) + DCOffset;
        }
    }
}