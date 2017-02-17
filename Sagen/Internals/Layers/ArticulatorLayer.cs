using System;

using Sagen.Phonetics;
using Sagen.Internals.Filters;

namespace Sagen.Internals.Layers
{
	/// <summary>
	/// Handles filtering, voicing/noise synthesis, and articulation given a pre-generated timeline of phonation events.
	/// </summary>
	internal unsafe class ArticulatorLayer : Layer
	{
		private double sampleIn, sampleOut, noiseIn;

		private readonly BandPassFilter bpf1;
		private readonly BandPassFilter bpf2;
		private readonly BandPassFilter bpf3;
		private readonly BandPassFilter bpf4;
        private readonly BandPassFilter bpf5;
		private readonly ButterworthFilter lpNoise, lpOverlay;

		// Attenuation for filters
		private double LEVEL_F1 = .10000;
		private double LEVEL_F2 = .13000;
		private double LEVEL_F3 = .10000;
		private double LEVEL_F4 = .12000;
        private double LEVEL_F5 = .10000;
		
		private double LEVEL_LPNOISE = 9;
		private double LEVEL_LPO = 0.04;
		private double LEVEL_NOISE = 0.075;

		private double f1, f2, f3, f4, f5;

		// Half-bandwidths for each formant
		private double bwhF1, bwhF2, bwhF3;

		// Temporary constants for controlling articulation parameters
		private double Height = 0.0;
		private double Backness = 0.0;
		private double Roundedness = 0.0;
		private double Rhotacization = 0.0;
		private double Nasalization = 0.0;

        private const double F4BaseMale = 3250.0;
        private const double F4BaseFemale = 3700.0;
        private const double F5MinDifference = 300.0;

        // Resonance levels
        private const double RES_F1 = .09;
        private const double RES_F2 = .1;
        private const double RES_F3 = .09;
        private const double RES_F4 = .12;
        private const double RES_F5 = .1;
        private const double RES_LPO = .16;
		private const double RES_LPNOISE = .35;

		// Gender-specific bandwidths for formants
		private const double BW_F1_MALE = 20.0;
		private const double BW_F2_MALE = 20.0;
		private const double BW_F3_MALE = 100.0;

		private const double BW_F1_FEMALE = 100.0;
		private const double BW_F2_FEMALE = 100.0;
		private const double BW_F3_FEMALE = 150.0;

        // Frequency constants
        private const double FREQ_LPNOISE = 280;
		private const double FREQ_LPO = 180;

        // Formants 3 and 4 are amplified by lerp(1.0, x, backness) where x is this constant, to simulate the "dark" quality of back vowels
        private const double BacknessF34AttenuationFactor = 0.3;

		// F3 - F2 will be multiplied by lerp(1.0, x, rhotacization) where x is this constant
		private const double RhotF3LowerFactor = 0.2;

		private const double NasalF1DiminishFactor = 0.2;
		private const double NasalF2LowerFactor = 0.3;
		private const double NasalF3RaiseFactor = 0.1;

		public ArticulatorLayer(Synthesizer synth) : base(synth)
		{
			bpf1 = new BandPassFilter(0, 0, synth.SampleRate, RES_F1, RES_F1);
			bpf2 = new BandPassFilter(0, 0, synth.SampleRate, RES_F2, RES_F2);
			bpf3 = new BandPassFilter(0, 0, synth.SampleRate, RES_F3, RES_F3);
			bpf4 = new BandPassFilter(0, 0, synth.SampleRate, RES_F4, RES_F4);
            bpf5 = new BandPassFilter(0, 0, synth.SampleRate, RES_F5, RES_F5);

			lpNoise = new ButterworthFilter(FREQ_LPNOISE, synth.SampleRate, PassFilterType.LowPass, RES_LPNOISE);
			lpOverlay = new ButterworthFilter(FREQ_LPO, synth.SampleRate, PassFilterType.LowPass, RES_LPO);

			UpdateLowerBandwidths();
			UpdateLowerFormants();
            UpdateHigherFormants();
			Console.WriteLine($"{f1:0.00}Hz, {f2:0.00}Hz, {f3:0.00}Hz, {f4:0.00}Hz, {f5:0.00}Hz");
		}

		private void UpdateLowerFormants()
		{
			// Calculate initial formant frequencies
			VowelConverter.GetFormants(Height, Backness, Roundedness, ref f1, ref f2, ref f3);

			// Apply head size
			double factor = 1.0 / synth.Voice.HeadSize;
			f1 *= factor;
			f2 *= factor;
			f3 *= factor;

			// Nasalize
			if (Nasalization > 0)
			{
				LEVEL_F1 *= Util.Lerp(1.0, NasalF1DiminishFactor, Nasalization);

				f2 = f1 + (f2 - f1) * Util.Lerp(1.0, NasalF2LowerFactor, Nasalization);
			}

			// Rhotacize
			if (Rhotacization > 0)
			{
				f3 = f2 + (f3 - f2) * Util.Lerp(1.0, RhotF3LowerFactor, Rhotacization);
			}

			// Update filters with formant frequencies / amplitudes
			bpf1.LowerBound = f1 - bwhF1;
			bpf1.UpperBound = f1 + bwhF1;
			bpf2.LowerBound = f2 - bwhF2;
			bpf2.UpperBound = f2 + bwhF2;
			bpf3.LowerBound = f3 - bwhF3;
			bpf3.UpperBound = f3 + bwhF3;

			bpf1.Volume = LEVEL_F1;
			bpf2.Volume = LEVEL_F2;
			bpf3.Volume = LEVEL_F3 * Util.Lerp(1.0, BacknessF34AttenuationFactor, Backness);
			bpf4.Volume = LEVEL_F4 * Util.Lerp(1.0, BacknessF34AttenuationFactor, Backness);
		}

        private void UpdateHigherFormants()
        {
            double bwh4, bwh5;
            double scale = 1.0 / synth.Voice.HeadSize;

            switch(synth.Voice.Gender)
            {   
                case VoiceGender.Male:
                    f4 = (F4BaseMale + synth.Voice.FrequencyOffsetF4) * scale;
                    f5 = (F4BaseMale + F5MinDifference + synth.Voice.FrequencyOffsetF5) * scale;
                    break;
                default:
                    f4 = (F4BaseFemale + synth.Voice.FrequencyOffsetF4) * scale;
                    f5 = (F4BaseFemale + F5MinDifference + synth.Voice.FrequencyOffsetF5) * scale;
                    break;
            }

            bwh4 = synth.Voice.BandwidthF4 / 2.0;
            bwh5 = synth.Voice.BandwidthF5 / 2.0;

            bpf4.LowerBound = f4 - bwh4;
            bpf4.UpperBound = f4 + bwh4;
            bpf5.LowerBound = f5 - bwh5;
            bpf5.UpperBound = f5 + bwh5;

            bpf4.Volume = LEVEL_F4;
            bpf5.Volume = LEVEL_F5;
        }

		private void UpdateLowerBandwidths()
		{
			switch(synth.Voice.Gender)
			{
				case VoiceGender.Male:
					bwhF1 = BW_F1_MALE * 0.5;
					bwhF2 = BW_F2_MALE * 0.5;
					bwhF3 = BW_F3_MALE * 0.5;
					break;
				default:
					bwhF1 = BW_F1_FEMALE * 0.5;
					bwhF2 = BW_F2_FEMALE * 0.5;
					bwhF3 = BW_F3_FEMALE * 0.5;
					break;
			}
		}

		public override void Update(ref double sample)
		{	
			sampleOut = 0.0;
			sampleIn = 0.0;
			synth.Pitch += 0.10f * synth.TimeStep;
			UpdateLowerFormants();

			// Combine noise with glottal sample
			Noise.SampleProcedural(ref noiseIn);
			lpNoise.Update(noiseIn);
			sampleIn = sample + (noiseIn + lpNoise.Value * LEVEL_LPNOISE) * LEVEL_NOISE * sample;

			// Update filters
			bpf1.Update(sampleIn);
			sampleOut += bpf1.Value * synth.Voice.FormantGain;

			bpf2.Update(sampleIn);
			sampleOut += bpf2.Value * synth.Voice.FormantGain;

			bpf3.Update(sampleIn);
			sampleOut += bpf3.Value * synth.Voice.FormantGain;

			bpf4.Update(sampleIn);
			sampleOut += bpf4.Value * synth.Voice.FormantGain;

			if (synth.Voice.Gender != VoiceGender.Female)
			{
				bpf5.Update(sampleIn);
				sampleOut += bpf5.Value * synth.Voice.FormantGain;
			}

			lpOverlay.Update(sample);
			sampleOut += lpOverlay.Value * LEVEL_LPO;

			if (synth.Voice.Quantization > 0)
			{
				sampleOut = Math.Round(synth.Voice.Quantization * sampleOut) / synth.Voice.Quantization;
			}

			sample = sampleOut;
		}
	}
}