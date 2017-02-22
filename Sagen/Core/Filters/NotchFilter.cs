using System;

namespace Sagen.Core.Filters
{
	internal sealed class NotchFilter
	{
		private double c, b0, b1, a1, a2;
		private double x0, x1, x2, y, y1, y2;
		private const double pi2 = Math.PI * 2.0;

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
