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

namespace Sagen.Phonetics
{
    /// <summary>
    /// Represents a formant frequency gradient based on articulatory orientation parameters, which may be sampled from.
    /// </summary>
    internal static class VowelConverter
    {
        private static readonly VowelQuality[] _vowelsAdult, _vowelsChild;

        [ThreadStatic]
        private static double[] _weightBuffer;

        static VowelConverter()
        {
            _vowelsAdult = new[]
            {
                // Close front unrounded
                new VowelQuality(1.0, 0.0, 0.0, 220, 3600, 4000),
                // Close front rounded
                new VowelQuality(1.0, 0.0, 1.0, 250, 1700, 2400),
                // Close central unrounded
                new VowelQuality(1.0, 0.5, 0.0, 250, 1750, 2125),
                // Close central rounded
                new VowelQuality(1.0, 0.5, 1.0, 200, 1400, 2000),
                // Close back unrounded
                new VowelQuality(1.0, 1.0, 0.0, 250, 1300, 2750),
                // Close back rounded
                new VowelQuality(1.0, 1.0, 1.0, 220, 300, 670),
                // Near-close near-front unrounded
                new VowelQuality(.85, .25, 0.0, 395, 1830, 2800),
                // Near-close near-front rounded
                new VowelQuality(.85, .25, 1.0, 325, 1500, 1890),
                // Near-close central unrounded
                new VowelQuality(.85, .5, 0.0, 330, 1600, 2500),
                // Near-close central rounded
                new VowelQuality(.85, .5, 1.0, 330, 1375, 2250),
                // Near-close near-back unrounded
                new VowelQuality(.85, .75, 0.0, 400, 1040, 2525),
                // Close-mid front unrounded
                new VowelQuality(.75, 0.0, 0.0, 390, 2300, 2650),
                // Close-mid front rounded
                new VowelQuality(.75, 0.0, 1.0, 390, 1475, 2180),
                // Close-mid central unrounded
                new VowelQuality(.75, 0.5, 0.0, 400, 1500, 2500),
                // Close-mid central rounded
                new VowelQuality(.75, 0.5, 1.0, 400, 1150, 2260),
                // Close-mid back unrounded
                new VowelQuality(.75, 1.0, 0.0, 400, 1300, 2475),
                // Close-mid back rounded
                new VowelQuality(.75, 1.0, 1.0, 400, 550, 2700),
                // Mid front unrounded
                new VowelQuality(.5, 0.0, 0.0, 400, 1760, 2475),
                // Mid front rounded
                new VowelQuality(.5, 0.0, 1.0, 550, 1100, 2525),
                // Mid central unrounded
                new VowelQuality(.5, .5, 0.0, 450, 1200, 2250),
                // Mid back rounded
                new VowelQuality(.5, 1.0, 1.0, 400, 550, 2490),
                // Open-mid front unrounded
                new VowelQuality(.25, 0.0, 0.0, 550, 1800, 2575),
                // Open-mid front rounded
                new VowelQuality(.25, 0.0, 1.0, 540, 1400, 2300),
                // Open-mid central unrounded
                new VowelQuality(.25, 0.5, 0.0, 600, 1350, 2500),
                // Open-mid central rounded
                new VowelQuality(.25, 0.5, 1.1, 400, 1190, 2295),
                // Open-mid back unrounded
                new VowelQuality(.25, 1.0, 0.0, 610, 1200, 2500),
                // Open-mid back rounded
                new VowelQuality(.25, 1.0, 1.0, 400, 560, 2460),
                // Near-open front unrounded
                new VowelQuality(.15, 0.0, 0.0, 800, 1650, 2500),

                // Open-back unrounded
                new VowelQuality(0.0, 1.0, 0.0, 750, 1000, 3300),
                // Open-front unrounded
                new VowelQuality(0.0, 0.0, 0.0, 940, 1400, 3100)
            };
            _vowelsChild = new[]
            {
                // Close front unrounded
                new VowelQuality(1.0, 0.0, 0.0, 250, 2600, 2800),
                // Close front rounded
                new VowelQuality(1.0, 0.0, 1.0, 250, 1700, 2400),
                // Close central unrounded
                new VowelQuality(1.0, 0.5, 0.0, 250, 1750, 2125),
                // Close central rounded
                new VowelQuality(1.0, 0.5, 1.0, 200, 1400, 2000),
                // Close back unrounded
                new VowelQuality(1.0, 1.0, 0.0, 250, 1300, 2750),
                // Close back rounded
                new VowelQuality(1.0, 1.0, 1.0, 270, 550, 2900),
                // Near-close near-front unrounded
                new VowelQuality(.85, .25, 0.0, 325, 2005, 2550),
                // Near-close near-front rounded
                new VowelQuality(.85, .25, 1.0, 325, 1500, 1890),
                // Near-close central unrounded
                new VowelQuality(.85, .5, 0.0, 330, 1600, 2500),
                // Near-close central rounded
                new VowelQuality(.85, .5, 1.0, 330, 1375, 2250),
                // Near-close near-back unrounded
                new VowelQuality(.85, .75, 0.0, 400, 1040, 2525),
                // Close-mid front unrounded
                new VowelQuality(.75, 0.0, 0.0, 390, 2300, 2650),
                // Close-mid front rounded
                new VowelQuality(.75, 0.0, 1.0, 390, 1475, 2180),
                // Close-mid central unrounded
                new VowelQuality(.75, 0.5, 0.0, 400, 1500, 2500),
                // Close-mid central rounded
                new VowelQuality(.75, 0.5, 1.0, 400, 1150, 2260),
                // Close-mid back unrounded
                new VowelQuality(.75, 1.0, 0.0, 400, 1300, 2475),
                // Close-mid back rounded
                new VowelQuality(.75, 1.0, 1.0, 410, 550, 3000),
                // Mid front unrounded
                new VowelQuality(.5, 0.0, 0.0, 400, 1760, 2475),
                // Mid front rounded
                new VowelQuality(.5, 0.0, 1.0, 550, 1100, 2525),
                // Mid central unrounded
                new VowelQuality(.5, .5, 0.0, 450, 1200, 2250),
                // Mid back rounded
                new VowelQuality(.5, 1.0, 1.0, 400, 550, 2490),
                // Open-mid front unrounded
                new VowelQuality(.25, 0.0, 0.0, 640, 1900, 2475),
                // Open-mid front rounded
                new VowelQuality(.25, 0.0, 1.0, 540, 1400, 2300),
                // Open-mid central unrounded
                new VowelQuality(.25, 0.5, 0.0, 600, 1350, 2500),
                // Open-mid central rounded
                new VowelQuality(.25, 0.5, 1.1, 400, 1190, 2295),
                // Open-mid back unrounded
                new VowelQuality(.25, 1.0, 0.0, 650, 1000, 1700),
                // Open-mid back rounded
                new VowelQuality(.25, 1.0, 1.0, 400, 560, 2460),
                // Near-open front unrounded
                new VowelQuality(.15, 0.0, 0.0, 800, 1650, 2500),

                // Open-back unrounded
                new VowelQuality(0.0, 1.0, 0.0, 750, 1000, 3300),
                // Open-front unrounded
                new VowelQuality(0.0, 0.0, 0.0, 840, 1200, 3100)
            };
            _weightBuffer = new double[_vowelsAdult.Length];
        }

        public static void GetFormants(VoiceGender gender, double height, double backness, double roundedness, ref double f1, ref double f2, ref double f3)
        {
            var vowels = gender == VoiceGender.Child ? _vowelsChild : _vowelsAdult;
            if (_weightBuffer == null) _weightBuffer = new double[_vowelsAdult.Length];
            f1 = f2 = f3 = 0.0;
            double distSum = 0.0;
            double minDist = -1.0;
            double weightSum = 0.0;
            double dist = 0.0;
            double weight;
            double h, b, r;

            // Calculate the sum of the distances between the input point and all the prism's reference points
            for (int i = 0; i < vowels.Length; i++)
            {
                h = height - vowels[i].Height;
                b = backness - vowels[i].Backness;
                r = roundedness - vowels[i].Roundedness;
                _weightBuffer[i] = dist = Math.Sqrt(h * h + b * b + r * r);
                if (dist <= 0.0)
                {
                    f1 = vowels[i].F1;
                    f2 = vowels[i].F2;
                    f3 = vowels[i].F3;
                    return;
                }
                distSum += dist;
                if (minDist < 0 || dist < minDist) minDist = dist;
            }

            // Calculate secondary weight sum
            // Buffer contains distances
            for (int i = 0; i < vowels.Length; i++)
            {
                dist = _weightBuffer[i];
                weightSum += _weightBuffer[i] = (1.0 - dist / distSum) / Math.Pow(4.5, dist / minDist);
            }

            // Calculate weights
            // Buffer contains secondary weights
            for (int i = 0; i < vowels.Length; i++)
            {
                weight = _weightBuffer[i] / weightSum;
                f1 += weight * vowels[i].F1;
                f2 += weight * vowels[i].F2;
                f3 += weight * vowels[i].F3;
            }
        }
    }
}