using System;

using Sagen.Phonetics;

namespace Sagen.Internals.Layers
{
	/// <summary>
	/// Handles filtering, voicing/noise synthesis, and articulation given a pre-generated timeline of phonation events.
	/// </summary>
	internal unsafe class ArticulatorLayer : Layer
	{
		private double sampleIn, sampleOut;

		private readonly BandPassFilter bpf1;
		private readonly BandPassFilter bpf2;
		private readonly BandPassFilter bpf3;
		private readonly BandPassFilter bpf4;
		private readonly ButterworthFilter lpOverlay;
		private readonly NotchFilter notch;

		// Attenuation for filters
		private double LEVEL_F1 = .05000;
		private double LEVEL_F2 = .10000;
		private double LEVEL_F3 = .06000;
		private double LEVEL_F4 = .06000;
		
		private double LEVEL_LPO = .125;

		// Frequency constants
		private const double FREQ_LPO = 350;

		// Resonance levels
		private const double RES_F1 = .1;
		private const double RES_F2 = .09;
		private const double RES_F3 = .075;
		private const double RES_F4 = .15;
		private const double RES_LPO = .2;

		// Half-bandwidths for each formant
		private double BWH_F1 = 50.0;
		private double BWH_F2 = 50.0;
		private double BWH_F3 = 125.0;
		private double BWH_F4 = 200.0;

		// Temporary constants for controlling articulation parameters
		private double Height = 0;
		private double Backness = 0.0;
		private double Roundedness = 0.0;
		private double Rhotacization = 0.0;
		private double Nasalization = 0.0;

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
			notch = new NotchFilter(synth.SampleRate, 6100, 0.25);

			lpOverlay = new ButterworthFilter(FREQ_LPO, synth.SampleRate, PassFilterType.LowPass, RES_LPO);

			UpdateFormants();
		}

		private void UpdateFormants()
		{
			double f1 = 0.0;
			double f2 = 0.0;
			double f3 = 0.0;
			double f4 = 0.0;

			// Calculate initial formant frequencies
			VowelConverter.GetFormants(Height, Backness, Roundedness, ref f1, ref f2, ref f3, ref f4);

			// Apply head size
			double factor = 1.0 / synth.Voice.HeadSize;
			f1 *= factor;
			f2 *= factor;
			f3 *= factor;
			f4 *= factor;

			// Nasalize
			if (Nasalization > 0)
			{
				LEVEL_F1 *= Util.Lerp(1.0, NasalF1DiminishFactor, Nasalization);

				f2 = f1 + (f2 - f1) * Util.Lerp(1.0, NasalF2LowerFactor, Nasalization);
				f3 = f4 - (f4 - f3) * Util.Lerp(1.0, NasalF3RaiseFactor, Nasalization);
			}

			// Rhotacize
			if (Rhotacization > 0)
			{
				f3 = f2 + (f3 - f2) * Util.Lerp(1.0, RhotF3LowerFactor, Rhotacization);
			}

			// Update filters with formant frequencies / amplitudes
			bpf1.LowerBound = f1 - BWH_F1;
			bpf1.UpperBound = f1 + BWH_F1;
			bpf2.LowerBound = f2 - BWH_F2;
			bpf2.UpperBound = f2 + BWH_F2;
			bpf3.LowerBound = f3 - BWH_F3;
			bpf3.UpperBound = f3 + BWH_F3;
			bpf4.LowerBound = f4 - BWH_F4;
			bpf4.UpperBound = f4 + BWH_F4;

			bpf1.Volume = LEVEL_F1;
			bpf2.Volume = LEVEL_F2;
			bpf3.Volume = LEVEL_F3 * Util.Lerp(1.0, BacknessF34AttenuationFactor, Backness);
			bpf4.Volume = LEVEL_F4 * Util.Lerp(1.0, BacknessF34AttenuationFactor, Backness);

			//Console.WriteLine($"{f1:0.00}Hz, {f2:0.00}Hz, {f3:0.00}Hz, {f4:0.00}Hz");
		}

		public override void Update(ref double sample)
		{	
			synth.Pitch += 0.1f * synth.TimeStep;
			UpdateFormants();

			sampleOut = 0.0;
			sampleIn = 0.0;
			Noise.SampleProcedural(ref sampleIn);
			sampleIn *= 0.1 * sample;
			sampleIn += sample;

			// Update filters
			bpf1.Update(sampleIn);
			bpf2.Update(sampleIn);
			bpf3.Update(sampleIn);
			bpf4.Update(sampleIn);
			lpOverlay.Update(sampleIn);

			// Combine filter results
			sampleOut += bpf1.Value;
			sampleOut += bpf2.Value;
			sampleOut += bpf3.Value;
			sampleOut += bpf4.Value;
			sampleOut += lpOverlay.Value * LEVEL_LPO;

			//sample = sampleOut;
			sample = notch.Update(sampleOut);

			if (synth.Voice.Quantization > 0)
			{
				sample = Math.Round(synth.Voice.Quantization * sample) / synth.Voice.Quantization;
			}
		}
	}
}