namespace HAARP
{
    internal abstract class Synthesizer
    {
        public const int MaxSampleRate = 192000;
        public const int MinSampleRate = 8000;
        public const int DefaultSampleRate = 44100;

        private int _sampleRate = DefaultSampleRate;
        private int _position = 0;
        private readonly float[] _buffer;

        public float BaselineFrequency { get; set; } = 100.0f;

        public Synthesizer()
        {
            _buffer = BufferPool.Loan();
        }

        public int SampleRate
        {
            get { return _sampleRate; }
            set
            {
                _sampleRate = value < MinSampleRate ? MinSampleRate : value > MaxSampleRate ? MaxSampleRate : value;
            }
        }

        public int Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public float TimePosition => (float)_position / _sampleRate;


    }
}