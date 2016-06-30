using System;
using System.IO;
using System.Reflection;

namespace HAARP.Samplers
{
    internal unsafe class NoiseSampler : Sampler
    {
	    internal static readonly float* NoiseDataPointer;
	    internal static readonly int NoiseDataLength;

	    static NoiseSampler()
	    {
		    using (
			    var stream =
				    Assembly.GetExecutingAssembly().GetManifestResourceStream("HAARP.Data.noise.raw") as UnmanagedMemoryStream)
		    {
			    NoiseDataPointer = (float*)stream.PositionPointer;
			    NoiseDataLength = (int)stream.Length / sizeof(float);
		    }
	    }

        private readonly BandPassFilter filter;

        public float MinFrequency { get; set; } = 0.0f;
        public float MaxFrequency { get; set; } = 2000.0f;
        public float Amplitude { get; set; } = 1.0f;

        public NoiseSampler(Synthesizer synth, float minFrequency, float maxFrequency, float filterResonance = .3f) : base(synth)
        {
            MinFrequency = minFrequency;
            MaxFrequency = maxFrequency;
            filter = new BandPassFilter(maxFrequency, minFrequency, 44100, filterResonance, filterResonance);
        }

        public override void Update(ref double sample)
        {
            filter.Update((NoiseDataPointer[synth.Position % NoiseDataLength]) * Amplitude);
            sample += filter.Value;
        }
    }
}