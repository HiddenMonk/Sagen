using System;

namespace HAARP
{
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

        private float hc, ha1, ha2, hb1, hb2;
        private float lc, la1, la2, lb1, lb2;
        private float output;

        private readonly float[] inputsLow = new float[2];
        private readonly float[] outputsLow = new float[3];
        private readonly float[] inputsHigh = new float[2];
        private readonly float[] outputsHigh = new float[3];

        public float Value => outputsHigh[0];

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
            la1 = 1.0f / (1.0f + _resonanceLow * lc + lc * lc);
            la2 = -2f * la1;
            lb1 = 2.0f * (lc * lc - 1.0f) * la1;
            lb2 = (1.0f - _resonanceLow * lc + lc * lc) * la1;
            _changedLow = false;
        }

        // Upper bound is a low-pass filter
        private void RecalculateHigh()
        {
            hc = 1.0f / (float)Math.Tan(Math.PI * _freqHigh / _sampleRate);
            ha1 = 1.0f / (1.0f + _resonanceHigh * hc + hc * hc);
            ha2 = 2f * ha1;
            hb1 = 2.0f * (1.0f - hc * hc) * ha1;
            hb2 = (1.0f - _resonanceHigh * hc + hc * hc) * ha1;
            _changedHigh = false;
        }

        public void Update(float input)
        {
            input *= _volume;

            if (_changedHigh) RecalculateHigh();
            if (_changedLow) RecalculateLow();

            output = la1 * input + la2 * inputsLow[0] + la1 * inputsLow[1] - lb1 * outputsLow[0] - lb2 * outputsLow[1];

            inputsLow[1] = inputsLow[0];
            inputsLow[0] = input;
            outputsLow[2] = outputsLow[1];
            outputsLow[1] = outputsLow[0];
            outputsLow[0] = output;

            output = ha1 * outputsLow[0] + ha2 * inputsHigh[0] + ha1 * inputsHigh[1] - hb1 * outputsHigh[0] - hb2 * outputsHigh[1];

            inputsHigh[1] = inputsHigh[0];
            inputsHigh[0] = outputsLow[0];
            outputsHigh[2] = outputsHigh[1];
            outputsHigh[1] = outputsHigh[0];
            outputsHigh[0] = output;
        }
    }
}