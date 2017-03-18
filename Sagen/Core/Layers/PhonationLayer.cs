#region License

// https://github.com/TheBerkin/Sagen
// 
// Copyright (c) 2017 Nicholas Fleck
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in the
// Software without restriction, including without limitation the rights to use, copy,
// modify, merge, publish, distribute, sublicense, and/or sell copies of the Software,
// and to permit persons to whom the Software is furnished to do so, subject to the
// following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A
// PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE
// OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

#endregion

using System;

using Sagen.Core.Audio;

namespace Sagen.Core.Layers
{
    /// <summary>
    /// This layer handles the production of the raw vocal harmonics, which are generated in the larynx via vibration of the
    /// vocal folds.
    /// </summary>
    internal class PhonationLayer : Layer
    {
        private const double MALE_OCTAVE_ATTENUATION = 0.251;
        private const double FEMALE_OCTAVE_ATTENUATION = 0.32;
        private const double CHILD_OCTAVE_ATTENUATION = 0.85;

        private const double SecondOctaveAttenuation = 0.4;

        private const double OPEN_GLOTTIS_SUSTAIN = 0.05;

        private const double CLEAR_GLOTTAL_DESCENT = 6;
        private const double CLEAR_GLOTTAL_PEAK = 0.4;

        private const double WHISPER_GLOTTAL_DESCENT = 2;
        private const double WHISPER_GLOTTAL_PEAK = 0.45;

        private readonly double[] envelope;
        private readonly GlottalPulse glottalPulse;
        private readonly int numHarmonics;
        private double state;

        public PhonationLayer(Synthesizer synth, int harmonics, double amplitude, double phase = 0.0f, double dcOffset = 0.0f) : base(synth)
        {
            Amplitude = amplitude;
            Phase = phase;
            state = phase;
            DCOffset = dcOffset;

            // Generate envelope
            numHarmonics = harmonics;
            envelope = new double[harmonics];
            envelope[0] = amplitude;

            glottalPulse = new GlottalPulse(
                Util.Lerp(CLEAR_GLOTTAL_DESCENT, WHISPER_GLOTTAL_DESCENT, synth.Voice.Breathiness),
                OPEN_GLOTTIS_SUSTAIN,
                Util.Lerp(CLEAR_GLOTTAL_PEAK, WHISPER_GLOTTAL_PEAK, synth.Voice.Breathiness)
            );

            double atten;
            switch (synth.Voice.Gender)
            {
                case VoiceGender.Male:
                    atten = MALE_OCTAVE_ATTENUATION;
                    break;
                case VoiceGender.Female:
                    atten = FEMALE_OCTAVE_ATTENUATION;
                    break;
                case VoiceGender.Child:
                    atten = CHILD_OCTAVE_ATTENUATION;
                    break;
                default:
                    atten = MALE_OCTAVE_ATTENUATION;
                    break;
            }

            for (int i = 1; i < harmonics; i++)
            {
                if (i == 1)
                    envelope[1] = amplitude * SecondOctaveAttenuation;
                else
                    envelope[i] = envelope[1] * Math.Pow(atten, Math.Log(i - 1, 2));
            }
        }

        /// <summary>
        /// The amplitude of the fundamental frequency.
        /// </summary>
        public double Amplitude { get; } = 0.5;

        /// <summary>
        /// The phase offset of the wave.
        /// </summary>
        public double Phase { get; } = 0.0;

        /// <summary>
        /// The DC (vertical) offset of the wave.
        /// </summary>
        public double DCOffset { get; set; } = 0.0;

        public override void Update(ref double sample)
        {
            unchecked
            {
                for (int i = 0; i < numHarmonics; i++)
                    sample += glottalPulse.Sample((state + Phase * i) % 1.0) * envelope[i] + DCOffset;

                state = (state + synth.TimeStep * synth.Fundamental) % 1.0;

                // As voicing decreases, flatten pulse towards y = 1. This allows more noise through while still allowing phonation.
                sample += (1.0 - synth.Voice.VoicingGain * 0.9) * (1.0 - sample);
            }
        }
    }
}