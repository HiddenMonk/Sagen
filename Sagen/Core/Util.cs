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
using System.Runtime.CompilerServices;

namespace Sagen.Core
{
    internal static class Util
    {
        public static float PIf = (float)Math.PI;

        public static float Lerp(float a, float b, float delta) => a + (b - a) * delta;
        public static double Lerp(double a, double b, double delta) => a + (b - a) * delta;

        public static double CosineInterpolate(double a, double b, double delta)
        {
            delta = (1.0f - Math.Cos(delta * Math.PI)) / 2.0f;
            return a * (1.0f - delta) + b * delta;
        }

        public static float CosineInterpolate(float a, float b, float delta)
        {
            delta = (1.0f - (float)Math.Cos(delta * PIf)) / 2.0f;
            return a * (1.0f - delta) + b * delta;
        }

        public static double FactorToDecibels(double factor) => Math.Log10(factor) * 10.0;

        public static double DecibelsToFactor(double dB) => Math.Pow(10.0, dB / 10.0);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Sigmoid(ref double x) => x = 2.0 / (1.0 + Math.Pow(1.5, -x)) - 1.0;
    }
}