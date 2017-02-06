using System;
using System.IO;

namespace WaveGen
{
	internal delegate double GeneratorFunc(int sampleIndex, int sampleCount);

	class Program
	{
		private const int SampleCount = 96000;

		static void Main(string[] args)
		{
			GenerateSampleFile(GenerateVocalWave, "vocal.raw", SampleCount);
		}

		static double GenerateVocalWave(int sampleIndex, int sampleCount)
		{
			double x = ((double)sampleIndex / (sampleCount - 1.0)) * Math.PI * 2.0;
			return Math.Sin(x + Math.Sin(x + Math.Sin(x + Math.Sin(x + Math.Sin(x + Math.Sin(x + Math.Sin(x)))))));
		}

		static void GenerateSampleFile(GeneratorFunc func, string path, int samples)
		{
			using (var writer = new BinaryWriter(File.Create(path)))
			{
				for (int i = 0; i < samples; i++)
				{
					writer.Write(func(i, samples));
				}
			}
		}
	}
}
