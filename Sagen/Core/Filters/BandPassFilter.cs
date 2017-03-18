#region License

// https://github.com/TheBerkin/Sagen
// 
// Copyright (c) 2017 Nicholas Fleck
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in the
// Software without restriction, including without limitation the rights to use, copy,
// modify, merge, publish, distribute, sublicense, and/or sell copies of the Software,
// and to permit persons to whom the Software is furnished to do so, subject to the
// following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A
// PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE
// OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

#endregion

using System;

namespace Sagen.Core.Filters
{
    // This is really just two butterworth filters combined.
    internal class BandPassFilter
    {
        private bool _changedHigh = false;
        private bool _changedLow = false;
        private double _freqLow, _freqHigh;
        private double _resonanceLow, _resonanceHigh;
        private int _sampleRate;
        private double _volume = 1.0f;

        private double hc, hc2, ha1, ha2, hb1, hb2;
        private double ih0, ih1;

        private double il0, il1;
        private double lc, lc2, la1, la2, lb1, lb2;
        private double oh0, oh1;
        private double ol0, ol1;
        private double output;

        public BandPassFilter(
            double lowFrequency,
            double highFrequency,
            int sampleRate,
            double resonanceLow,
            double resonanceHigh)
        {
            _freqLow = lowFrequency;
            _freqHigh = highFrequency;
            _resonanceLow = resonanceLow;
            _resonanceHigh = resonanceHigh;
            _sampleRate = sampleRate;
            RecalculateLow();
            RecalculateHigh();
        }

        public double Volume
        {
            get { return _volume; }
            set { _volume = value; }
        }

        public double ResonanceLow
        {
            get { return _resonanceLow; }
            set
            {
                _resonanceLow = value;
                _changedLow = true;
            }
        }

        public double ResonanceHigh
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

        public double UpperBound
        {
            get { return _freqHigh; }
            set
            {
                _freqHigh = value;
                _changedHigh = true;
            }
        }

        public double LowerBound
        {
            get { return _freqLow; }
            set
            {
                _freqLow = value;
                _changedLow = true;
            }
        }

        public double Value => oh0 * _volume;

        // Lower bound is a high-pass filter
        private void RecalculateLow()
        {
            lc = Math.Tan(Math.PI * _freqLow / _sampleRate);
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
            hc = 1.0 / Math.Tan(Math.PI * _freqHigh / _sampleRate);
            hc2 = hc * hc;
            ha1 = 1.0f / (1.0f + _resonanceHigh * hc + hc2);
            ha2 = 2f * ha1;
            hb1 = 2.0f * (1.0f - hc2) * ha1;
            hb2 = (1.0f - _resonanceHigh * hc + hc2) * ha1;
            _changedHigh = false;
        }

        public void Update(double input)
        {
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