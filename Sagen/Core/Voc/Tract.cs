using System;

namespace Sagen.Core.Voc
{
	internal sealed class Tract
	{
		// TODO: Investigate changing tract size for female/child voices
		public const int Size = 44;
		public const int NoseLength = (int)(28 * (Size / 44));
		public const int NoseStart = Size - NoseLength + 1;
		public const int TipStart = 32;

		public TransientPool Transients;

		// Length = Size
		public double[] Diameter, RestDiameter, TargetDiameter, NewDiameter;
		public double[] L, R, A;
		// Length = Size + 1
		public double[] Reflection, NewReflection, JunctionOutL, JunctionOutR;
		// Length = NoseLength
		public double[] NoseL, NoseR, NoseA, NoseDiameter;
		// Length = NoseLength + 1
		public double[] NoseJuncOutL, NoseJuncOutR, NoseReflection;

		public double ReflectionLeft, ReflectionRight, ReflectionNose;
		public double NewReflectionLeft, NewReflectionRight, NewReflectionNose;
		public double VelumTarget;
		public double GlottalReflection;
		public double LipReflection;
		public int LastObstruction;
		public double Fade;
		public double MovementSpeed;
		public double LipOutput;
		public double NoseOutput;
		public double DeltaTime;
		public double T;

		public Tract(int sampleRate) // tract_init
		{
			double diameter, d;

			VelumTarget = 0.01;
			GlottalReflection = 0.75;
			LipReflection = -0.85;
			LastObstruction = -1;
			MovementSpeed = 15;

			Diameter = new double[Size];
			RestDiameter = new double[Size];
			TargetDiameter = new double[Size];
			NewDiameter = new double[Size];
			L = new double[Size];
			R = new double[Size];
			A = new double[Size];

			Reflection = new double[Size + 1];
			NewReflection = new double[Size + 1];
			JunctionOutL = new double[Size + 1];
			JunctionOutR = new double[Size + 1];

			NoseL = new double[NoseLength];
			NoseR = new double[NoseLength];
			NoseA = new double[NoseLength];
			NoseDiameter = new double[NoseLength];

			NoseJuncOutL = new double[NoseLength + 1];
			NoseJuncOutR = new double[NoseLength + 1];
			NoseReflection = new double[NoseLength + 1];

			// Set diameters for main vocal tract
			for(int i = 0; i < Size; i++)
			{
				diameter = 0;

				if (i < 7 * Size / 44 - 0.5)
				{
					diameter = 0.6;
				}
				else if (i < 12 * Size / 44)
				{
					diameter = 1.1;
				}
				else
				{
					diameter = 1.5;
				}

				Diameter[i] =
				RestDiameter[i] =
				TargetDiameter[i] =
				NewDiameter[i] =
					diameter;
			}

			// Set diameters for nasal tract
			for(int i = 0; i < NoseLength; i++)
			{
				d = 2 * (i / NoseLength);

				if (d < 1)
				{
					diameter = 0.4 * 1.6 * d;
				}
				else
				{
					diameter = 0.5 + 1.5 * (2 - d);
				}

				diameter = Math.Min(diameter, 1.9);

				NoseDiameter[i] = diameter;
			}

			CalculateReflections();
			CalculateNoseReflections();

			NoseDiameter[0] = VelumTarget;
			DeltaTime = 512.0 / sampleRate;
			T = 1.0 / sampleRate;
			Transients = new TransientPool();
		}

		public void CalculateReflections() // tract_calculate_reflections
		{
			for (int i = 0; i < Size; i++)
			{
				A[i] = Diameter[i] * Diameter[i];
			}

			for (int i = 1; i < Size; i++)
			{
				Reflection[i] = NewReflection[i];

				if (A[i] == 0.0)
				{
					NewReflection[i] = 0.999;
				}
				else
				{
					NewReflection[i]
						= (A[i - 1] - A[i]) / (A[i - 1] + A[i]);
				}
			}

			ReflectionLeft = NewReflectionLeft;
			ReflectionRight = NewReflectionRight;
			ReflectionNose = NewReflectionNose;

			double sum = A[NoseStart] + A[NoseStart + 1] + NoseA[0];

			NewReflectionLeft = (2.0 * A[NoseStart] - sum) / sum;
			NewReflectionRight = (2.0 * A[NoseStart + 1] - sum) / sum;
			NewReflectionNose = (2.0 * NoseA[0] - sum) / sum;
		}

		public void CalculateNoseReflections() // tract_calculate_nose_reflections
		{
			for (int i = 0; i < NoseLength; i++)
			{
				NoseA[i] = NoseDiameter[i] * NoseDiameter[i];
			}

			for (int i = 1; i < NoseLength; i++)
			{
				NoseReflection[i]
					= (NoseA[i - 1] - NoseA[i])
					/ (NoseA[i - 1] + NoseA[i]);
			}
		}

		public void Reshape() // tract_reshape
		{
			double amount = DeltaTime * MovementSpeed;
			int currentObstruction = -1;

			for (int i = 0; i < Size; i++)
			{
				double slowReturn;
				double diameter = Diameter[i];
				double targetDiameter = TargetDiameter[i];

				if (diameter < 0.001)
				{
					currentObstruction = i;
				}

				if (i < NoseStart)
				{
					slowReturn = 0.6;
				}
				else if (i >= TipStart)
				{
					slowReturn = 1.0;
				}
				else
				{
					slowReturn = .6 + .4 * (i - NoseStart) / (TipStart - NoseStart);
				}

				Diameter[i] =
					Util.MoveTowards(diameter, targetDiameter, slowReturn * amount, 2 * amount);

			}

			if (LastObstruction > -1 && currentObstruction == -1 && NoseA[0] < 0.05)
			{
				Transients.AppendTransient(LastObstruction);
			}

			LastObstruction = currentObstruction;

			NoseDiameter[0] =
				Util.MoveTowards(NoseDiameter[0], VelumTarget, amount * 0.25, amount * 0.1);

			NoseA[0] = NoseDiameter[0] * NoseDiameter[0];
		}

		public void Compute(double input, double lambda)
		{
			double r, w, amp;
			var pool = Transients;
			int transientCount = pool.Size;

			// Process transients
			for(int i = 0; i < transientCount; i++)
			{
				var trans = Transients.Pool[i];
				amp = trans.Strength * 
					Math.Pow(2, -trans.Exponent * trans.TimeAlive);
				L[trans.Position] += amp * 0.5;
				R[trans.Position] += amp * 0.5;
				trans.TimeAlive += T * 0.5;

				if (trans.TimeAlive > trans.Lifetime)
				{
					pool.RemoveTransient(trans.ID);
				}
			}

			JunctionOutR[0] = L[0] * GlottalReflection + input;
			JunctionOutL[Size] = R[Size - 1] * LipReflection;

			for(int i = 1; i < Size; i++)
			{
				r = Reflection[i] * (1.0 - lambda) + NewReflection[i] * lambda;
				w = r * (R[i - 1] + L[i]);
				JunctionOutR[i] = R[i - 1] - w;
				JunctionOutL[i] = L[i] + w;
			}

			int ns = NoseStart;

			r = NewReflectionLeft * (1.0 - lambda) + ReflectionLeft * lambda;
			JunctionOutL[ns] = r * R[ns - 1] + (1.0 + r) * (NoseL[0] + L[ns]);

			r = NewReflectionRight * (1.0 - lambda) + ReflectionRight * lambda;
			JunctionOutR[ns] = r * L[ns] + (1.0 + r) * (R[ns - 1] + NoseL[0]);

			r = NewReflectionNose * (1.0 - lambda) + ReflectionNose * lambda;
			NoseJuncOutR[0] = r * NoseL[0] + (1.0 + r) * (L[ns] + R[ns - 1]);

			for(int i = 0; i < Size; i++)
			{
				R[i] = JunctionOutR[i] * 0.999;
				L[i] = JunctionOutL[i + 1] * 0.999;
			}

			LipOutput = R[Size - 1];

			NoseJuncOutL[NoseLength] = NoseR[NoseLength - 1] * LipReflection;

			for(int i = 1; i < NoseLength; i++)
			{
				w = NoseReflection[i] * (NoseR[i - 1] + NoseL[i]);
				NoseJuncOutR[i] = NoseR[i - 1] - w;
				NoseJuncOutL[i] = NoseL[i] + w;
			}

			for(int i = 0; i < NoseLength; i++)
			{
				NoseR[i] = NoseJuncOutR[i];
				NoseL[i] = NoseJuncOutL[i + 1];
			}

			NoseOutput = NoseR[NoseLength - 1];
		}
	}
}
