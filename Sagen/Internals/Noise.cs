using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Sagen.Internals.Layers
{
    internal static class Noise
    {
		private static double scale = 2.0 / 0xffffffff;
		private static uint x1 = 0x67452301;
		private static uint x2 = 0xefcdab89;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void SampleProcedural(ref double sample)
		{			
			sample = x2 * scale;
			x1 ^= x2;
			x2 += x1;
		}
    }
}