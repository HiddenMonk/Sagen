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
		private const double AttenuationPerOctave = 0.25;
		private const double SecondOctaveAttenuation = 0.65;

		private readonly double[] envelope;
		private readonly int numHarmonics;
		private double state, frequency;
		private readonly GlottalPulse glottalPulse = new GlottalPulse(4, 0.2);
		
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

		public PhonationLayer(Synthesizer synth, int harmonics, double amplitude, double phase = 0.0f, double dcOffset = 0.0f) : base(synth)
		{
			Amplitude = amplitude;
			Phase = phase;
			state = phase;
			DCOffset = dcOffset;

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
					sample += glottalPulse.Sample((state + Phase * i) % 1.0) * envelope[i] + DCOffset;
				}

				state = (state + synth.TimeStep * synth.Fundamental) % 1.0;
			}
		}
	}
}