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

namespace Sagen.Core.Layers
{
    internal class SineLayer : Layer
    {
        private const double FullPhase = Math.PI * 2.0;
        private double stateA, stateB;

        public SineLayer(Synthesizer synth) : base(synth)
        {
        }

        public SineLayer(Synthesizer synth, double fa, double fb, double amplitude, double phase = 0.0f, double dcOffset = 0.0f) : base(synth)
        {
            FrequencyA = fa;
            FrequencyB = fb;
            Amplitude = amplitude;
            Phase = phase;
            stateA = phase;
            stateB = phase;
            DCOffset = dcOffset;
        }

        /// <summary>
        /// Frequency A in Hertz.
        /// </summary>
        public double FrequencyA { get; set; } = 697.0;

        /// <summary>
        /// Frequency B in Hertz.
        /// </summary>
        public double FrequencyB { get; set; } = 1209.0;

        /// <summary>
        /// The amplitude of the tone.
        /// </summary>
        public double Amplitude { get; set; } = 0.5;

        /// <summary>
        /// The phase offset of the tone.
        /// </summary>
        public double Phase { get; } = 0.0f;

        /// <summary>
        /// The DC (vertical) offset of the wave.
        /// </summary>
        public double DCOffset { get; set; } = 0.0f;

        public override void Update(ref double sample)
        {
            stateA = (stateA + synth.TimeStep * FrequencyA) % 1.0f;
            stateB = (stateB + synth.TimeStep * FrequencyB) % 1.0f;
            sample += (FrequencyA > 0 ? Math.Sin(stateA * FullPhase) * Amplitude : 0) + (FrequencyB > 0 ? Math.Sin(stateB * FullPhase) * Amplitude : 0) + DCOffset;
        }
    }
}