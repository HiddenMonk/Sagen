using System;

namespace Sagen.Core.Audio
{
	/// <summary>
	/// Generates glottal pulses using a modified version of Fant's glottal model.
	/// </summary>
	internal sealed class GlottalPulse
	{
		private double _k = 4.0;
		private double _tp = 0.5;
		private double _o = 0.1;
		private double og; // Coefficient used in ascending and descending branches
		private double tc; // Time of closure

		public GlottalPulse(double k, double o, double peakTime)
		{
			_k = k < 0.5 ? 0.5 : k;
			_o = o;
			_tp = peakTime < 0.0 ? 0.0 : peakTime > 1.0 ? 1.0 : peakTime;
			SetCoefficients();
		}

		/// <summary>
		/// Controls the slope of the descending branch. Higher values cause a sharper descent. Setting this to 0.5 makes the wave symmetrical.
		/// <para>Range: [0.5, Infinity)</para>
		/// </summary>
		public double K
		{
			get { return _k; }
			set
			{
				_k = value < 0.5 ? 0.5 : value;
				SetCoefficients();
			}
		}

		/// <summary>
		/// The normalized time at which the wave is at its maximum.
		/// <para>Range: [0, 1]</para>
		/// </summary>
		public double PeakTime
		{
			get { return _tp; }
			set
			{
				_tp = value < 0.0 ? 0.0 : value > 1.0 ? 1.0 : value;
				SetCoefficients();
			}
		}

		private void SetCoefficients()
		{
			og = Math.PI / _tp;
			tc = _tp + _o + 1.0 / og * Math.Acos((_k - 1) / _k);
		}

		public double Sample(double normalizedTime)
		{
			// Ascending branch
			if (normalizedTime <= _tp)
			{
				return 0.5 * (1.0 - Math.Cos(normalizedTime * og));
			}

			if (normalizedTime <= _tp + _o) return 1.0;
			
			if (normalizedTime <= tc)
			{
				return _k * Math.Cos(og * (normalizedTime - _tp - _o)) - _k + 1.0;
			}

			return 0.0;
		}
	}
}
