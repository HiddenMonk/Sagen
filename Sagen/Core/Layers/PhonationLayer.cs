﻿using System;

using Sagen.Core.Audio;

namespace Sagen.Core.Layers
{
	/// <summary>
	/// This layer handles the production of the raw vocal harmonics, which are generated in the larynx via vibration of the vocal folds.
	/// </summary>
	internal class PhonationLayer : Layer
	{		
		private const double AttenuationPerOctave = 0.251;
		private const double SecondOctaveAttenuation = 0.35;

		private const double OPEN_GLOTTIS_SUSTAIN = 0.1;

		private const double CLEAR_GLOTTAL_DESCENT = 6;
		private const double CLEAR_GLOTTAL_PEAK = 0.4;

		private const double WHISPER_GLOTTAL_DESCENT = 1;
		private const double WHISPER_GLOTTAL_PEAK = 0.5;

		private readonly double[] envelope;
		private readonly int numHarmonics;
		private double state;
		private readonly GlottalPulse glottalPulse;
		
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

			glottalPulse = new GlottalPulse(
				Util.Lerp(CLEAR_GLOTTAL_DESCENT, WHISPER_GLOTTAL_DESCENT, synth.Voice.Breathiness), 
				OPEN_GLOTTIS_SUSTAIN, 
				Util.Lerp(CLEAR_GLOTTAL_PEAK, WHISPER_GLOTTAL_PEAK, synth.Voice.Breathiness)
				);

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
					sample += glottalPulse.Sample((state + Phase * i) % 1.0) * envelope[i] + DCOffset;
				}

				state = (state + synth.TimeStep * synth.Fundamental) % 1.0;

                // As voicing decreases, flatten pulse towards y = 1. This allows more noise through while still allowing phonation.
                sample += (1.0 - synth.Voice.VoicingGain) * (1.0 - sample);
            }
		}
	}
}