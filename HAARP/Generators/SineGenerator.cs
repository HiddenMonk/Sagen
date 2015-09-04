using System;

namespace HAARP.Generators
{
    public class SineGenerator : Generator
    {
        private const double FullPhase = Math.PI * 2.0f;

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
        public float Phase { get; set; } = 0.0f;

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

        public SineGenerator()
        {
            
        }

        public SineGenerator(float frequency, float amplitude, float phase = 0.0f, float dcOffset = 0.0f)
        {
            Frequency = frequency;
            Amplitude = amplitude;
            Phase = phase;
            DCOffset = dcOffset;
        }

        public override float Sample(float timeSeconds) => 
            ((float)Math.Sin(timeSeconds * (FullPhase + FullPhase * Phase) * Frequency) * Amplitude).Tilt(Frequency, SpectralTilt, SpectralUpperBound) + DCOffset;
    }
}