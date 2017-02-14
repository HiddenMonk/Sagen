using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sagen.Internals.Layers
{
	/// <summary>
	/// This layer handles the production of the raw vocal harmonics, which are generated in the larynx via vibration of the vocal folds.
	/// </summary>
	internal unsafe class PhonationLayer : Layer
	{		
		private static readonly int _dataLength, _dataLastIndex;
		private static readonly double* _ptrSamples;

		private const double AttenuationPerOctave = 0.25;
		private const double SecondOctaveAttenuation = 0.3;

		private readonly double[] envelope;
		private readonly int numHarmonics;
		private double state, frequency;
		private readonly GlottalPulse glottalPulse = new GlottalPulse(4, 0.75);

		static PhonationLayer()
		{
			using (
				var stream =
					Assembly.GetExecutingAssembly().GetManifestResourceStream("Sagen.Data.vocal.raw") as UnmanagedMemoryStream)
			{
				_ptrSamples = (double*)stream.PositionPointer;
				_dataLength = (int)stream.Length / sizeof(double);
				_dataLastIndex = _dataLength - 1;
			}
		}

		/// <summary>
		/// The amplitude of the fundamental frequency.
		/// </summary>
		public double Amplitude { get; } = 0.5;

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

		public PhonationLayer(Synthesizer synth, int harmonics, double amplitude, double phase = 0.0f, double tilt = 0.0f, double dcOffset = 0.0f) : base(synth)
		{
			Amplitude = amplitude;
			Phase = phase;
			state = phase;
			DCOffset = dcOffset;
			SpectralTilt = tilt;

			// Generate envelope
			numHarmonics = harmonics;
			envelope = new double[harmonics];
			envelope[0] = amplitude;

			for(int i = 1; i < harmonics; i++)
			{
				if (i == 1)
				{
					envelope[1] = amplitude * SecondOctaveAttenuation;
				}
				else
				{
					envelope[i] = envelope[1] * Math.Pow(AttenuationPerOctave, Math.Log(i - 1, 2));
				}
			}
		}
		
		public override void Update(ref double sample)
		{
			unchecked
			{
				for(int i = 0; i < numHarmonics; i++)
				{
					frequency = synth.Fundamental * (i + 1);
					//sample += _ptrSamples[(int)(((state + Phase * i) % 1.0) * _dataLastIndex)] * envelope[i] + DCOffset;
					sample += glottalPulse.Sample((state + Phase * i) % 1.0) * envelope[i] + DCOffset;
				}

				state = (state + synth.TimeStep * synth.Fundamental) % 1.0;
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