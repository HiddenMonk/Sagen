using System;

namespace Sagen.Core.Voc
{
	internal static class Util
	{
		public static double MoveTowards(double current, double target, double amtUp, double amtDown)
		{
			if (current < target)
			{
				return Math.Min(current + amtUp, target);
			}
			else
			{
				return Math.Max(current - amtDown, target);
			}
		}
	}
}
