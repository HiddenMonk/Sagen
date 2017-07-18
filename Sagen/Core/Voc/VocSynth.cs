using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sagen.Core.Voc
{
	public sealed class VocSynth
	{
		public const int MaxTransients = 4;
		public const int BufferSize = 512;

		private readonly Glottis _glottis;
		private readonly Tract _tract;
		private int _counter;
		private readonly Random rng;

		internal Glottis Glottis => _glottis;
		internal Tract Tract => _tract;
		public int Counter => _counter;

		public VocSynth(int seed, int sampleRate)
		{
			_glottis = new Glottis(sampleRate);
			_tract = new Tract(sampleRate);
			rng = new Random(seed);
		}

		public double Fundamental
		{
			get => _glottis.Frequency;
			set => _glottis.Frequency = value;
		}

		public double Velum
		{
			get => _tract.VelumTarget;
			set => _tract.VelumTarget = value;
		}

		public double Tenseness
		{
			get => _glottis.Tenseness;
			set => _glottis.Tenseness = value;
		}

		public void SetTongueShape(double tongueIndex, double tongueDiameter)
		{
			SetDiameters(10, 39, 32, tongueIndex, tongueDiameter, _tract.TargetDiameter);
		}

		public void SetDiameters(
			int bladeStart,
			int lipStart,
			int tipStart,
			double tongueIndex,
			double tongueDiameter,
			double[] diameters)
		{
			double t, fixedTongueDiameter, curve;
			int gridOffset = 0;

			for(int i = bladeStart; i < lipStart; i++)
			{
				t = 1.1 * Math.PI * 
					(tongueIndex - i) / (tipStart - bladeStart);
				fixedTongueDiameter = 2 + (tongueDiameter - 2) / 1.5;
				curve = (1.5 - fixedTongueDiameter + gridOffset) * Math.Cos(t);

				if (i == bladeStart - 2 || i == lipStart - 1)
				{
					curve *= 0.8;
				}
				
				if (i == bladeStart || i == lipStart - 2)
				{
					curve *= 0.94;
				}

				diameters[i] = 1.5 - curve;
			}
		}

		public void Compute(ref double output)
		{
			double vocalOutput, glot, lambda1, lambda2;

			if (Counter == 0)
			{
				_tract.Reshape();
				_tract.CalculateReflections();				
			}

			vocalOutput = 0;
			lambda1 = (double)Counter / BufferSize;
			lambda2 = (double)(Counter + 0.5) / BufferSize;
			glot = _glottis.Compute(rng, lambda1);

			_tract.Compute(glot, lambda1);
			vocalOutput += _tract.LipOutput + _tract.NoseOutput;

			_tract.Compute(glot, lambda2);
			vocalOutput += _tract.LipOutput + _tract.NoseOutput;

			output += vocalOutput * 0.125;
			_counter = (_counter + 1) % BufferSize;
		}
	}
}
