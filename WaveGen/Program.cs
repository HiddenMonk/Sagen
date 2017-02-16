using System;
using System.IO;

namespace WaveGen
{	
	class Program
	{
		private const int SampleCount = 96000;

		static void Main(string[] args)
		{
			var rand = new Random();
			using (var writer = new BinaryWriter(File.Create("noise.raw")))
			{
				for(int i = 0; i < SampleCount; i++)
				{
					double u1 = 1.0 - rand.NextDouble();
					double u2 = 1.0 - rand.NextDouble();
					double stdn = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);

					writer.Write(stdn);
				}
			}
		}
	}
}
