using System;

using Sagen.Core.Audio;
using Sagen.Core.Filters;
using Sagen.Phonetics;

namespace Sagen.Core.Layers
{
	/// <summary>
	/// Handles filtering, voicing/noise synthesis, and articulation given a pre-generated timeline of phonation events.
	/// </summary>
	internal class ArticulatorLayer : Layer
	{
		private const double F4BaseMale = 3250.0;
		private const double F4BaseFemale = 3700.0;
		private const double F5MinDifference = 300.0;

		// Resonance levels
		private const double RES_F1 = .1;
		private const double RES_F2 = .1;
		private const double RES_F3 = .1;
		private const double RES_F4 = .12;
		private const double RES_F5 = .1;
		private const double RES_LPO = .2;
		private const double RES_HPO = .15;
		private const double RES_LPNOISE = .35;

		// Gender-specific bandwidths for formants
		private const double BW_F1_MALE = 20.0;
		private const double BW_F2_MALE = 30.0;
		private const double BW_F3_MALE = 40.0;

		private const double BW_F1_FEMALE = 100.0;
		private const double BW_F2_FEMALE = 100.0;
		private const double BW_F3_FEMALE = 80.0;

		private const double BW_F1_CHILD = 100.0;
		private const double BW_F2_CHILD = 100.0;
		private const double BW_F3_CHILD = 120.0;

		// Frequency constants
		private const double FREQ_LPNOISE = 280;
		private const double FREQ_HPO = 5500;

		// Formants 3 and 4 are amplified by lerp(1.0, x, backness) where x is this constant, to simulate the "dark" quality of back vowels
		private const double BacknessF34AttenuationFactor = 0.5;

		// F3 - F2 will be multiplied by lerp(1.0, x, rhotacization) where x is this constant
		private const double RhotF3LowerFactor = 0.2;

		private const double NasalF1DiminishFactor = 0.2;
		private const double NasalF2LowerFactor = 0.3;

		private readonly BandPassFilter bpf1, bpf2, bpf3, bpf4, bpf5;
		private readonly ButterworthFilter lpNoise, lpOverlay, hpOverlay;

		// Half-bandwidths for each formant
		private double bwhF1, bwhF2, bwhF3;

		private double f1, f2, f3, f4, f5;

		// Temporary constants for controlling articulation parameters
		
		private readonly double LEVEL_ASPIRATION = 0.075;
		private readonly double LEVEL_ASPIRATION_LPO = 9;

		// Attenuation for filters
		private double LEVEL_F1 = .10000;
		private readonly double LEVEL_F2 = .25000;
		private readonly double LEVEL_F3 = .14000;
		private readonly double LEVEL_F4 = .12000;
		private readonly double LEVEL_F5 = .10000;
		private readonly double LEVEL_LPO = 0.06;
		private readonly double LEVEL_HPO = 0.275;
		
		private readonly double Rhotacization = 0.0;
		private readonly double Nasalization = 0.0;

		private double nasalMix;
		
		private double sampleIn, sampleOut, aspirationIn, fricationIn;

		public ArticulatorLayer(Synthesizer synth) : base(synth)
		{
			bpf1 = new BandPassFilter(0, 0, synth.SampleRate, RES_F1, RES_F1);
			bpf2 = new BandPassFilter(0, 0, synth.SampleRate, RES_F2, RES_F2);
			bpf3 = new BandPassFilter(0, 0, synth.SampleRate, RES_F3, RES_F3);
			bpf4 = new BandPassFilter(0, 0, synth.SampleRate, RES_F4, RES_F4);
			bpf5 = new BandPassFilter(0, 0, synth.SampleRate, RES_F5, RES_F5);

			lpNoise = new ButterworthFilter(FREQ_LPNOISE, synth.SampleRate, PassFilterType.LowPass, RES_LPNOISE);
			hpOverlay = new ButterworthFilter(FREQ_HPO, synth.SampleRate, PassFilterType.HighPass, RES_HPO);
			lpOverlay = new ButterworthFilter(1, synth.SampleRate, PassFilterType.LowPass, RES_LPO);

			UpdateLowerBandwidths();
			UpdateLowerFormants();
			UpdateHigherFormants();
		}

		private void UpdateLowerFormants()
		{
			// Calculate initial formant frequencies
			VowelConverter.GetFormants(synth.Voice.Gender, synth.State.Height, synth.State.Backness, synth.State.Roundedness, ref f1, ref f2, ref f3);

			// Apply head size
			f1 /= synth.Voice.HeadSize;
			f2 /= synth.Voice.HeadSize;
			f3 /= synth.Voice.HeadSize;

			// Nasalize
			if ((nasalMix = synth.Voice.Nasalization * (1.0 - Nasalization) + synth.Voice.Nasalization) > 0.0)
			{
				LEVEL_F1 *= Util.Lerp(1.0, NasalF1DiminishFactor, nasalMix);

				f2 = f1 + (f2 - f1) * Util.Lerp(1.0, NasalF2LowerFactor, nasalMix);
			}

			// Rhotacize
			if (Rhotacization > 0)
				f3 = f2 + (f3 - f2) * Util.Lerp(1.0, RhotF3LowerFactor, Rhotacization);

			// Update filters with formant frequencies / amplitudes
			bpf1.LowerBound = f1 - bwhF1;
			bpf1.UpperBound = f1 + bwhF1;
			bpf2.LowerBound = f2 - bwhF2;
			bpf2.UpperBound = f2 + bwhF2;
			bpf3.LowerBound = f3 - bwhF3;
			bpf3.UpperBound = f3 + bwhF3;

			bpf1.Volume = LEVEL_F1 * Util.Lerp(1.0, 0.75, synth.State.Height);
			bpf2.Volume = LEVEL_F2;
			bpf3.Volume = LEVEL_F3 * Util.Lerp(1.0, BacknessF34AttenuationFactor, synth.State.Backness);
			bpf4.Volume = LEVEL_F4 * Util.Lerp(1.0, BacknessF34AttenuationFactor, synth.State.Backness);
		}

		private void UpdateHigherFormants()
		{
			double bwh4 = synth.Voice.BandwidthF4 / 2.0;
			double bwh5 = synth.Voice.BandwidthF5 / 2.0;
			
			switch (synth.Voice.Gender)
			{
				case VoiceGender.Male:
					f4 = (F4BaseMale + synth.Voice.FrequencyOffsetF4) / synth.Voice.HeadSize;
					f5 = (F4BaseMale + F5MinDifference + synth.Voice.FrequencyOffsetF5) / synth.Voice.HeadSize;
					break;
				case VoiceGender.Female:
					f4 = (F4BaseFemale + synth.Voice.FrequencyOffsetF4) / synth.Voice.HeadSize;
					break;
			}

			bpf4.LowerBound = f4 - bwh4;
			bpf4.UpperBound = f4 + bwh4;
			bpf5.LowerBound = f5 - bwh5;
			bpf5.UpperBound = f5 + bwh5;

			bpf4.Volume = LEVEL_F4;
			bpf5.Volume = LEVEL_F5;
		}

		private void UpdateLowerBandwidths()
		{
			switch (synth.Voice.Gender)
			{
				case VoiceGender.Male:
					bwhF1 = BW_F1_MALE * 0.5;
					bwhF2 = BW_F2_MALE * 0.5;
					bwhF3 = BW_F3_MALE * 0.5;
					break;
				case VoiceGender.Female:
					bwhF1 = BW_F1_FEMALE * 0.5;
					bwhF2 = BW_F2_FEMALE * 0.5;
					bwhF3 = BW_F3_FEMALE * 0.5;
					break;
				case VoiceGender.Child:
					bwhF1 = BW_F1_CHILD * 0.5;
					bwhF2 = BW_F2_CHILD * 0.5;
					bwhF3 = BW_F3_CHILD * 0.5;
					break;
			}
		}

		public override void Update(ref double sample)
		{
			sampleOut = 0.0;
			sampleIn = 0.0;
			UpdateLowerFormants();

			// Combine aspiration with glottal pulse
			Noise.SampleProcedural(ref aspirationIn);
			lpNoise.Update(aspirationIn);
			sampleIn = sample + (aspirationIn + lpNoise.Value * LEVEL_ASPIRATION_LPO) 
				* LEVEL_ASPIRATION * synth.State.AspirationLevel * sample * synth.Voice.Breathiness;

			// Update filters
			bpf1.Update(sampleIn);
			sampleOut += bpf1.Value * synth.Voice.FormantGain;

			bpf2.Update(sampleIn);
			sampleOut += bpf2.Value * synth.Voice.FormantGain;

			bpf3.Update(sampleIn);
			sampleOut += bpf3.Value * synth.Voice.FormantGain;

			// Young children do not have higher formants
			switch (synth.Voice.Gender)
			{
				case VoiceGender.Male:
					bpf5.Update(sampleIn);
					sampleOut += bpf5.Value * synth.Voice.FormantGain;
					goto case VoiceGender.Female;
				case VoiceGender.Female:
					bpf4.Update(sampleIn);
					sampleOut += bpf4.Value * synth.Voice.FormantGain;
					break;
			}

			lpOverlay.Frequency = f1 * 0.5;
			lpOverlay.Update(sample);
			sampleOut += lpOverlay.Value * LEVEL_LPO / synth.Voice.HeadSize;
			
			hpOverlay.Update(sampleIn);
			sampleOut += hpOverlay.Value * LEVEL_HPO;

			if (synth.Voice.Quantization > 0)
				sampleOut = Math.Round(synth.Voice.Quantization * sampleOut) / synth.Voice.Quantization;

			sample = sampleOut * synth.State.GlottisLevel;
		}
	}
}