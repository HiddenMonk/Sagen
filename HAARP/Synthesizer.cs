using System.Collections.Generic;

using HAARP.Samplers;

namespace HAARP
{
    internal class Synthesizer
    {
        public const int MaxSamplers = 72;

        public const int MaxSampleRate = 192000;
        public const int MinSampleRate = 8000;
        public const int DefaultSampleRate = 44100;

        private readonly HashSet<Sampler> samplers = new HashSet<Sampler>();

        private readonly int _sampleRate = DefaultSampleRate;
        private readonly float _timeStep;
        private int _position = 0;
        private readonly float[] _buffer;

        public float Fundamental { get; set; } = 100.0f;

        public Synthesizer(float seconds)
        {
            _buffer = new float[(int)(seconds * _sampleRate)]; // Use a buffer pool later
            _timeStep = 1.0f / _sampleRate;
        }

        public Synthesizer(int sampleRate, float seconds)
        {
            _sampleRate = sampleRate < MinSampleRate ? MinSampleRate : sampleRate > MaxSampleRate ? MaxSampleRate : sampleRate;
            _buffer = new float[(int)(seconds * _sampleRate)]; // Use a buffer pool later
            _timeStep = 1.0f / _sampleRate;
        }

        public int SampleRate => _sampleRate;

        public int Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public void AddSampler(Sampler sampler)
        {
            if (sampler != null) samplers.Add(sampler);
        }

        public float[] Generate()
        {
            int ln = _buffer.Length;
            foreach (var sampler in samplers)
            {
                for (_position = 0; _position < ln; _position++)
                {
                    sampler.Update(this, ref _buffer[_position]);
                }
            }

            return _buffer;
        }

        public float TimePosition => (float)_position / _sampleRate;

        public float TimeStep => _timeStep;
    }
}