using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace HAARP.Samplers
{
	internal unsafe class HarmonicSampler : Sampler
	{
		private double state;
		private static readonly int _dataLength;
		private static readonly float* _ptrSamples;

		static HarmonicSampler()
		{
			using (
				var stream =
					Assembly.GetExecutingAssembly().GetManifestResourceStream("HAARP.Data.vocal.raw") as UnmanagedMemoryStream)
			{
				_ptrSamples = (float*)stream.PositionPointer;
				_dataLength = (int)stream.Length / sizeof(float);
			}
		}

		/// <summary>
		/// The frequency in Hertz.
		/// </summary>
		public float HarmonicOffsetFactor { get; set; } = 2.75f;

		/// <summary>
		/// The harmonic number. 0 = fundamental frequency
		/// </summary>
		public int Harmonic { get; set; } = 0;

		/// <summary>
		/// The amplitude of the wave.
		/// </summary>
		public float Amplitude { get; set; } = 0.5f;

		/// <summary>
		/// The phase offset of the wave.
		/// </summary>
		public float Phase { get; } = 0.0f;

		/// <summary>
		/// The DC (vertical) offset of the wave.
		/// </summary>
		public float DCOffset { get; set; } = 0.0f;

		/// <summary>
		/// The spectral tilt of the wave.
		/// </summary>
		public float SpectralTilt { get; set; } = 0.0f;

		/// <summary>
		/// The nyquist frequency to use for spectral tilting.
		/// </summary>
		public float SpectralUpperBound { get; set; } = 22050.0f;

		public HarmonicSampler(Synthesizer synth) : base(synth)
		{

		}

		public HarmonicSampler(Synthesizer synth, int harmonic, float amplitude, float phase = 0.0f, float tilt = 0.0f, float dcOffset = 0.0f) : base(synth)
		{
			Amplitude = amplitude;
			Harmonic = harmonic;
			Phase = phase;
			state = phase;
			DCOffset = dcOffset;
			SpectralTilt = tilt;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override void Update(ref float sample)
		{
			float f = synth.Fundamental * (Harmonic + 1) + HarmonicOffsetFactor * Harmonic;
			state = (state + synth.TimeStep * f) % 1.0f;
			sample += (_ptrSamples[(int)(state * (_dataLength - 1))] * Amplitude).Tilt(f, SpectralTilt, SpectralUpperBound) + DCOffset;
		}
	}
}