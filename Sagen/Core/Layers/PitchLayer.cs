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
    internal class PitchLayer : Layer
    {
        private const double F0Male = 120;
        private const double F0Female = 240;
        private const double F0Child = 240;
        private readonly RNG rng;

        private bool shakeAscend = false;
        private double shakeOffset = 0.0;
        private double shakeOffsetAtten = 0.9;
        private double shakeTimer = 0.0;
        private double vibTimer = 0.0;

        public PitchLayer(Synthesizer synthesizer) : base(synthesizer)
        {
            rng = new RNG();
        }

        public override void Update(ref double sample)
        {
            // Update shake-related variables
            if ((shakeTimer -= synth.TimeStep) <= 0.0)
            {
                shakeTimer = rng.NextDouble(0.01, 0.2);
                shakeOffsetAtten = rng.NextDouble(0.9, 0.99999999999);
                shakeAscend = !shakeAscend;
            }

            // Update normalized shake offset
            if (shakeAscend)
                shakeOffset += (1.0 - shakeOffset) * shakeOffsetAtten * synth.TimeStep * synth.Voice.VoiceShakeAscendRate;
            else
                shakeOffset += (-1.0 - shakeOffset) * shakeOffsetAtten * synth.TimeStep * synth.Voice.VoiceShakeDescendRate;

            // Apply base frequency for selected gender
            switch (synth.Voice.Gender)
            {
                case VoiceGender.Male:
                    synth.Fundamental = F0Male * synth.Voice.FundamentalFrequencyMultiplier;
                    break;
                case VoiceGender.Female:
                    synth.Fundamental = F0Female * synth.Voice.FundamentalFrequencyMultiplier;
                    break;
                default:
                    synth.Fundamental = F0Child * synth.Voice.FundamentalFrequencyMultiplier;
                    break;
            }

            synth.Fundamental *= Math.Pow(2.0, synth.Pitch);

            // Apply vibrato if available
            if (synth.Voice.VibratoAmount > 0)
            {
                vibTimer = (vibTimer + synth.TimeStep) % 1.0;

                synth.Fundamental *= Math.Pow(2.0, synth.Voice.VibratoAmount * Math.Sin(vibTimer * Math.PI * 2.0 * synth.Voice.VibratoSpeed));
            }

            // Apply shake offset
            synth.Fundamental += shakeOffset * synth.Voice.VoiceShakeAmountHz;
        }
    }
}