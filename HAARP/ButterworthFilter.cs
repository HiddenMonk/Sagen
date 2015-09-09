using System;

namespace HAARP
{
    public class ButterworthFilter
    {
        private float _resonance;
        private float _frequency;
        private int _sampleRate;
        private PassFilterType _filterType;
        private bool _changed = false;

        /// <summary>
        /// The resonance amount. Range: [0.1, sqrt(2)]
        /// </summary>
        public float Resonance
        {
            get { return _resonance; }
            set
            {
                _resonance = value;
                _changed = true;
            }
        }

        /// <summary>
        /// The cutoff frequency.
        /// </summary>
        public float Frequency
        {
            get { return _frequency; }

            set
            {
                _frequency = value;
                _changed = true;
            }
        }

        /// <summary>
        /// The sampling rate.
        /// </summary>
        public int SampleRate
        {
            get { return _sampleRate; }
            set
            {
                _sampleRate = value;
                _changed = true;
            }
        }

        /// <summary>
        /// The filter type.
        /// </summary>
        public PassFilterType FilterType
        {
            get { return _filterType; }
            set
            {
                _filterType = value;
                _changed = true;
            }
        }

        private float c, a1, a2, b1, b2;

        /// <summary>
        /// Array of input values, latest are in front
        /// </summary>
        private readonly float[] inputHistory = new float[2];

        /// <summary>
        /// Array of output values, latest are in front
        /// </summary>
        private readonly float[] outputHistory = new float[3];

        public ButterworthFilter(float frequency, int sampleRate, PassFilterType filterType, float resonance)
        {
            _resonance = resonance;
            _frequency = frequency;
            _sampleRate = sampleRate;
            _filterType = filterType;
            RecalculateConstants();
        }

        private void RecalculateConstants()
        {
            switch (_filterType)
            {
                case PassFilterType.LowPass:
                    c = 1.0f / (float)Math.Tan(Math.PI * _frequency / _sampleRate);
                    a1 = 1.0f / (1.0f + _resonance * c + c * c);
                    a2 = 2f * a1;
                    b1 = 2.0f * (1.0f - c * c) * a1;
                    b2 = (1.0f - _resonance * c + c * c) * a1;
                    break;
                case PassFilterType.HighPass:
                    c = (float)Math.Tan(Math.PI * _frequency / _sampleRate);
                    a1 = 1.0f / (1.0f + _resonance * c + c * c);
                    a2 = -2f * a1;
                    b1 = 2.0f * (c * c - 1.0f) * a1;
                    b2 = (1.0f - _resonance * c + c * c) * a1;
                    break;
            }
        }

        public void Update(float newInput)
        {
            if (_changed)
            {
                RecalculateConstants();
                _changed = false;
            }

            var output = a1 * newInput + a2 * inputHistory[0] + a1 * inputHistory[1] - b1 * outputHistory[0] - b2 * outputHistory[1];

            inputHistory[1] = inputHistory[0];
            inputHistory[0] = newInput;

            outputHistory[2] = outputHistory[1];
            outputHistory[1] = outputHistory[0];
            outputHistory[0] = output;
        }

        public float Value => outputHistory[0];
    }

    public enum PassFilterType
    {
        HighPass,
        LowPass,
    }
}