using System;

namespace HAARP
{
	// This is really just two butterworth filters combined.
	internal class BandPassFilter
	{
		private float _resonanceLow, _resonanceHigh;
		private float _freqLow, _freqHigh;
		private float _volume = 1.0f;
		private int _sampleRate;
		private bool _changedLow = false;
		private bool _changedHigh = false;

		public float Volume
		{
			get { return _volume; }
			set { _volume = value; }
		}

		public float ResonanceLow
		{
			get { return _resonanceLow; }
			set
			{
				_resonanceLow = value;
				_changedLow = true;
			}
		}

		public float ResonanceHigh
		{
			get { return _resonanceHigh; }
			set
			{
				_resonanceHigh = value;
				_changedLow = true;
			}
		}

		public int SampleRate
		{
			get { return _sampleRate; }
			set
			{
				if (value == _sampleRate) return;
				_sampleRate = value;
				_changedLow = true;
				_changedHigh = true;
			}
		}

		public float UpperBound
		{
			get { return _freqHigh; }
			set
			{
				_freqHigh = value;
				_changedHigh = true;
			}
		}

		public float LowerBound
		{
			get { return _freqLow; }
			set
			{
				_freqLow = value;
				_changedLow = true;
			}
		}

		private float hc, hc2, ha1, ha2, hb1, hb2;
		private float lc, lc2, la1, la2, lb1, lb2;
		private float output;

		private float il0, il1;
		private float ol0, ol1;
		private float ih0, ih1;
		private float oh0, oh1;

		public float Value => oh0;

		public BandPassFilter(
			float lowFrequency,
			float highFrequency,
			int sampleRate,
			float resonanceLow,
			float resonanceHigh)
		{
			_freqLow = lowFrequency;
			_freqHigh = highFrequency;
			_resonanceLow = resonanceLow;
			_resonanceHigh = resonanceHigh;
			_sampleRate = sampleRate;
			RecalculateLow();
			RecalculateHigh();
		}

		// Lower bound is a high-pass filter
		private void RecalculateLow()
		{
			lc = (float)Math.Tan(Math.PI * _freqLow / _sampleRate);
			lc2 = lc * lc;
			la1 = 1.0f / (1.0f + _resonanceLow * lc + lc2);
			la2 = -2f * la1;
			lb1 = 2.0f * (lc2 - 1.0f) * la1;
			lb2 = (1.0f - _resonanceLow * lc + lc2) * la1;
			_changedLow = false;
		}

		// Upper bound is a low-pass filter
		private void RecalculateHigh()
		{
			hc = (float)(1.0 / Math.Tan(Math.PI * _freqHigh / _sampleRate));
			hc2 = hc * hc;
			ha1 = 1.0f / (1.0f + _resonanceHigh * hc + hc2);
			ha2 = 2f * ha1;
			hb1 = 2.0f * (1.0f - hc2) * ha1;
			hb2 = (1.0f - _resonanceHigh * hc + hc2) * ha1;
			_changedHigh = false;
		}

		public void Update(float input)
		{
			input *= _volume;

			if (_changedHigh) RecalculateHigh();
			if (_changedLow) RecalculateLow();

			// Upper bound calculation
			output = la1 * input + la2 * il0 + la1 * il1 - lb1 * ol0 - lb2 * ol1;

			il1 = il0;
			il0 = input;
			ol1 = ol0;
			ol0 = output;

			// Lower bound calculation
			output = ha1 * ol0 + ha2 * ih0 + ha1 * ih1 - hb1 * oh0 - hb2 * oh1;

			ih1 = ih0;
			ih0 = ol0;
			oh1 = oh0;
			oh0 = output;
		}
	}
}