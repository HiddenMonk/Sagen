using System;

namespace HAARP
{
    internal class VocalHarmonics
    {
        private readonly float[] states;
        private readonly float[] factors;
        private float f0, _output;
        private double dOut;
        private readonly float _count;

        public VocalHarmonics(int count, float fundamental, float spectralTilt = 0.5f, float phaseOffset = 0.0f)
        {
            if (count <= 0) throw new ArgumentException($"'{nameof(count)}' must be greater than zero.");
            f0 = fundamental;
            _count = count;
            states = new float[count];
            factors = new float[count];
            float factor = 1.0f;
            for (int i = 0; i < count; i++)
            {
                states[i] = phaseOffset * i;

                factors[i] = factor;
                factor *= spectralTilt;
            }
        }

        public float Fundamental
        {
            get { return f0; }
            set { f0 = value; }
        }

        public float FundamentalState => states[0];

        public float Pitch { get; set; } = 1.0f;

        public float Output => _output;

        public void Update(float timeDelta)
        {
            const double fullPhase = Math.PI * 2;
            dOut = 0.0f;
            for (int i = 0; i < _count; i++)
            {
                states[i] = (states[i] + timeDelta * f0 * Pitch * (1 + i)) % 1.0f;
                dOut += Math.Sin(states[i] * fullPhase) * factors[i];
            }
            _output = (float)dOut;
        }
    }
}