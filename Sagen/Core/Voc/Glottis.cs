using System;

namespace Sagen.Core.Voc
{
	internal sealed class Glottis
	{
		public double Frequency;
		public double Tenseness;
		public double Rd;
		public double WaveformLength;
		public double TimeInWaveform;

		public double Alpha;
		public double E0;
		public double Epsilon;
		public double Shift;
		public double Delta;
		public double Te;
		public double Omega;

		public readonly double T;

		public Glottis(double sampleRate) // glottis_init
		{
			Frequency = 140;
			Tenseness = 0.6;
			T = 1.0 / sampleRate;
			TimeInWaveform = 0.0;
			SetupWaveform(0.0);
		}

		private void SetupWaveform(double lambda) // glottis_setup_waveform
		{
			double rd, ra, rk, rg;
			double ta, tp, te;
			double epsilon, shift, delta;
			double rhsIntegral, lowerIntegral, upperIntegral;
			double alpha, e0, omega, s, y, z;

			Rd = 3.0 * (1.0 - Tenseness);
			WaveformLength = 1.0 / Frequency;

			rd = Rd;
			if (rd < 0.5) rd = 0.5;
			if (rd > 2.7) rd = 2.7;

			ra = -0.01 + 0.048 * rd;
			rk = 0.224 + 0.118 * rd;
			rg = (rk / 4.0) * (0.5 + 1.2 * rk) / (0.11 * rd - ra * (0.5 + 1.2 * rk));

			ta = ra;
			tp = 1.0 / (2.0 * rg);
			te = tp + tp * rk;

			epsilon = 1.0 / ta;
			shift = Math.Exp(-epsilon * (1.0 - te));
			delta = 1.0 - shift;

			rhsIntegral = (1.0 / epsilon) * (shift - 1.0) + (1.0 - te) * shift;
			rhsIntegral /= delta;
			lowerIntegral = -(te - tp) / 2.0 + rhsIntegral;
			upperIntegral = -lowerIntegral;

			omega = Math.PI / tp;
			s = Math.Sin(omega * te);

			y = -Math.PI * s * upperIntegral / (tp * 2.0);
			z = Math.Log(y);
			alpha = z / (tp / 2.0 - te);
			e0 = -1.0 / (s * Math.Exp(alpha * te));

			Alpha = alpha;
			E0 = e0;
			Epsilon = epsilon;
			Shift = shift;
			Delta = delta;
			Te = te;
			Omega = omega;
		}

		public double Compute(Random rng, double lambda) // glottis_compute
		{
			double output = 0.0;
			double aspiration = 0.0;
			double noise = 0.0;
			double t = 0.0;
			double intensity = 1.0;

			TimeInWaveform += T;

			if (TimeInWaveform > WaveformLength)
			{
				TimeInWaveform %= WaveformLength;
				SetupWaveform(lambda);
			}

			t = TimeInWaveform / WaveformLength;

			if (t > Te)
			{
				output = (-Math.Exp(-Epsilon * (t - Te)) + Shift) / Delta;
			}
			else
			{
				output = (E0 * Math.Exp(Alpha * t) * Math.Sin(Omega * t));
			}

			noise = 2.0 * rng.NextDouble() - 1.0;

			aspiration = intensity * (1.0 - Math.Sqrt(Tenseness)) * 0.3 * noise;
			aspiration *= 0.2;

			output += aspiration;

			return output;
		}

	}
}
