using System;
using System.IO;

namespace WaveGen
{
	internal delegate double GeneratorFunc(int sampleIndex, int sampleCount);

	/// <summary>
	/// Generates a glottal pulse using the Fant glottal model.
	/// </summary>
	class Program
	{
		private const int SampleCount = 96000;
		private const double tp = 0.3; // Time of the maximum of the pulse
		private const double tc = 1.0; // Duration of the period
		private const double te = 0.7; // Time of the minimum of the pulse derivative
		private const double ta = 0.1; // Return phase duration (after Glottal Closing Instant)

		static void Main(string[] args)
		{
			GenerateSampleFile(GenerateVocalWave, "vocal.raw", SampleCount);
		}

		static double GenerateVocalWave(int sampleIndex, int sampleCount)
		{
			double x = (sampleIndex / (sampleCount - 1.0)) * Math.PI * 2.0;
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
