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

namespace Sagen.Core.Nodes
{
    internal class PhonemeNode : Node
    {
        private readonly double _b;
        private readonly double _h;
        private readonly double _r;

        public PhonemeNode(double duration, double h, double b, double r) : base(duration)
        {
            _h = h;
            _b = b;
            _r = r;
        }

        public override void OnEnter(Synthesizer synth)
        {
        }

        public override void OnExit(Synthesizer synth)
        {
            synth.State.GlottisLevel = 1.0;
        }

        public override void OnUpdate(Synthesizer synth)
        {
            if (synth.State.LastGlottisLevel < 1.0)
            {
                double localTime = GetRelativeSeconds(synth);

                synth.State.GlottisLevel =
                    localTime >= synth.Voice.GlottisOpenTime
                        ? 1.0
                        : Util.CosineInterpolate(synth.State.LastGlottisLevel, 1.0, localTime / synth.Voice.GlottisOpenTime);
            }
            synth.State.Height = _h;
            synth.State.Backness = _b;
            synth.State.Roundedness = _r;
            synth.Pitch = Util.Lerp(0, -0.1, GetNormalizedRelativePos(synth));
        }
    }
}