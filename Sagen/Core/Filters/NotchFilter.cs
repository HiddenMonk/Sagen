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

namespace Sagen.Core.Filters
{
    internal sealed class NotchFilter
    {
        private const double pi2 = Math.PI * 2.0;
        private double c, b0, b1, a1, a2;
        private double x0, x1, x2, y, y1, y2;

        public NotchFilter(int sampleRate, double frequency, double steep)
        {
            c = Math.Cos(pi2 * frequency / sampleRate);
            b0 = (1.0 - steep) * (1.0 - steep) / (2.0 * (Math.Abs(c) + 1)) + steep;
            b1 = -2.0 * c * b0;
            a1 = -2.0 * c * steep;
            a2 = steep * steep;
        }

        public double Update(double sample)
        {
            y = b0 * x0 + b1 * x1 + b0 * x2 - a1 * y1 - a2 * y2;
            y2 = y1;
            y1 = y;
            x2 = x1;
            x1 = x0;
            x0 = sample;

            return y;
        }
    }
}