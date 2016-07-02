using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sagen.Samplers
{
	internal unsafe class HarmonicSampler : Sampler
	{
		private double state, frequency;
		private Converter64 converter;
		private static readonly int _dataLength, _dataLastIndex;
		private static readonly double* _ptrSamples;

		static HarmonicSampler()
		{
			using (
				var stream =
					Assembly.GetExecutingAssembly().GetManifestResourceStream("HAARP.Data.vocal.raw") as UnmanagedMemoryStream)
			{
				_ptrSamples = (double*)stream.PositionPointer;
				_dataLength = (int)stream.Length / sizeof(double);
				_dataLastIndex = _dataLength - 1;
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
		public double Amplitude { get; set; } = 0.5;

		/// <summary>
		/// The phase offset of the wave.
		/// </summary>
		public double Phase { get; } = 0.0;

		/// <summary>
		/// The DC (vertical) offset of the wave.
		/// </summary>
		public double DCOffset { get; set; } = 0.0;

		/// <summary>
		/// The spectral tilt of the wave.
		/// </summary>
		public double SpectralTilt { get; set; } = 0.0;

		public HarmonicSampler(Synthesizer synth) : base(synth)
		{

		}

		public HarmonicSampler(Synthesizer synth, int harmonic, double amplitude, double phase = 0.0f, double tilt = 0.0f, double dcOffset = 0.0f) : base(synth)
		{
			Amplitude = amplitude;
			Harmonic = harmonic;
			Phase = phase;
			state = phase;
			DCOffset = dcOffset;
			SpectralTilt = tilt;
		}
		
		public override void Update(ref double sample)
		{
			unchecked
			{
				frequency = synth.Fundamental * (Harmonic + 1) + HarmonicOffsetFactor * Harmonic;
				state = (state + synth.TimeStep * frequency) % 1.0;
				//sample += Tilt(_ptrSamples[(int)(state * (_dataLength - 1))] * Amplitude, f, SpectralTilt) + DCOffset;
				converter.ValueInteger = (long)(((frequency * NYQUIST * 2.0 - 1.0) * SpectralTilt) * MAGICAL_NUMBER + 1072632447);
				sample += _ptrSamples[(int)(state * _dataLastIndex)] * Amplitude * converter.ValueDouble + DCOffset;
			}
		}

		private static readonly int MAGICAL_NUMBER = (int)(BitConverter.DoubleToInt64Bits(2.0) >> 32) - 1072632447;
		private const double NYQUIST = 1.0 / 22050.0;

		[StructLayout(LayoutKind.Explicit)]
		private struct Converter64
		{
			[FieldOffset(0)]
			public double ValueDouble;
			[FieldOffset(4)]
			public long ValueInteger;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static double Tilt(double sample, double frequency, double spectralTilt)
		{
			return sample * new Converter64 { ValueInteger = (long)(((frequency * NYQUIST * 2.0 - 1.0) * spectralTilt) * MAGICAL_NUMBER + 1072632447) }.ValueDouble;
		}
	}
}