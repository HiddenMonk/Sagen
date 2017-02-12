using System;

namespace Sagen.Internals
{
    /// <summary>
    /// Represents a formant frequency gradient based on articulatory orientation parameters, which may be sampled from.
    /// </summary>
    internal static class VowelConverter
    {
        private static readonly VowelQuality[] _points;

        [ThreadStatic]
        private static readonly double[] _weightBuffer;

        static VowelConverter()
        {
            _points = new VowelQuality[]
            {
                // Close front unrounded
                new VowelQuality(1.0, 0.0, 0.0, 250, 2750, 3010, 3900),
                // Close front rounded
                new VowelQuality(1.0, 0.0, 1.0, 250, 1700, 2400, 2950),
                // Close central unrounded
                new VowelQuality(1.0, 0.5, 0.0, 250, 1750, 2125, 3125),
                // Close central rounded
                new VowelQuality(1.0, 0.5, 1.0, 200, 1400, 2000, 3050),
                // Close back unrounded
                new VowelQuality(1.0, 1.0, 0.0, 250, 1300, 2750, 3400),
                // Close back rounded
                new VowelQuality(1.0, 1.0, 1.0, 250, 550, 2550, 3050),
                // Near-close near-front unrounded
                new VowelQuality(.85, .25, 0.0, 325, 2005, 2550, 2950),
                // Near-close near-front rounded
                new VowelQuality(.85, .25, 1.0, 325, 1500, 1890, 2855),
                // Near-close central unrounded
                new VowelQuality(.85, .5, 0.0, 330, 1600, 2500, 3575),
                // Near-close central rounded
                new VowelQuality(.85, .5, 1.0, 330, 1375, 2250, 3150),
                // Near-close near-back unrounded
                new VowelQuality(.85, .75, 0.0, 400, 1040, 2525, 3375),
                // Close-mid front unrounded
                new VowelQuality(.75, 0.0, 0.0, 390, 2100, 2650, 4800),
                // Close-mid front rounded
                new VowelQuality(.75, 0.0, 1.0, 390, 1475, 2180, 2850),
                // Close-mid central unrounded
                new VowelQuality(.75, 0.5, 0.0, 400, 1500, 2500, 3350),
                // Close-mid central rounded
                new VowelQuality(.75, 0.5, 1.0, 400, 1150, 2260, 3110),
                // Close-mid back unrounded
                new VowelQuality(.75, 1.0, 0.0, 400, 1300, 2475, 2850),
                // Close-mid back rounded
                new VowelQuality(.75, 1.0, 1.0, 400, 550, 2300, 2500),
                // Mid front unrounded
                new VowelQuality(.5, 0.0, 0.0, 400, 1760, 2475, 3640),
                // Mid front rounded
                new VowelQuality(.5, 0.0, 1.0, 550, 1100, 2525, 3490),
                // Mid central unrounded
                new VowelQuality(.5, .5, 0.0, 450, 1200, 2250, 3150),
                // Mid back rounded
                new VowelQuality(.5, 1.0, 1.0, 400, 550, 2490, 3630),
                // Open-mid front unrounded
                new VowelQuality(.25, 0.0, 0.0, 550, 1800, 2575, 5500),
                // Open-mid front rounded
                new VowelQuality(.25, 0.0, 1.0, 540, 1340, 2300, 3460),
                // Open-mid central unrounded
                new VowelQuality(.25, 0.5, 0.0, 600, 1350, 2500, 3100),
                // Open-mid central rounded
                new VowelQuality(.25, 0.5, 1.1, 400, 1190, 2295, 3100),
                // Open-mid back unrounded
                new VowelQuality(.25, 1.0, 0.0, 610, 1200, 2500, 3100),
                // Open-mid back rounded
                new VowelQuality(.25, 1.0, 1.0, 400, 560, 2460, 2575),
                // Near-open front unrounded
                new VowelQuality(.15, 0.0, 0.0, 800, 1650, 2500, 2900),

				// Open-back unrounded
				new VowelQuality(0.0, 1.0, 0.0, 750, 1000, 3300, 4200),
                // Open-front unrounded
                new VowelQuality(0.0, 0.0, 0.0, 940, 1500, 3100, 4200),
            };
            _weightBuffer = new double[_points.Length];
        }

        public static void GetFormants(double height, double backness, double roundedness, ref double f1, ref double f2, ref double f3, ref double f4)
        {
            double distSum = 0.0;
            double minDist = -1.0;
            double weightSum = 0.0; 
            double dist = 0.0;
            double weight;
            double h, b, r;

            // Calculate the sum of the distances between the input point and all the prism's reference points
            for(int i = 0; i < _points.Length; i++)
            {
                h = height - _points[i].Height;
                b = backness - _points[i].Backness;
                r = roundedness - _points[i].Roundedness;
                _weightBuffer[i] = dist = Math.Sqrt(h * h + b * b + r * r);
                if (dist <= 0.0)
                {
                    f1 = _points[i].F1;
                    f2 = _points[i].F2;
                    f3 = _points[i].F3;
                    f4 = _points[i].F4;
                    return;
                }
                distSum += dist;
                if (minDist < 0 || dist < minDist) minDist = dist;
            }
            
            // Calculate secondary weight sum
            // Buffer contains distances
            for (int i = 0; i < _points.Length; i++)
            {
                dist = _weightBuffer[i];
                weightSum += _weightBuffer[i] = (1.0 - dist / distSum) / Math.Pow(10.0, dist / minDist);
            }

            // Calculate weights
            // Buffer contains secondary weights
            for (int i = 0; i < _points.Length; i++)
            {
                weight = _weightBuffer[i] / weightSum;
                f1 += weight * _points[i].F1;
                f2 += weight * _points[i].F2;
                f3 += weight * _points[i].F3;
                f4 += weight * _points[i].F4;
            }
        }
    }
}
