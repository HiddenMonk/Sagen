using System;

namespace Sagen.Internals.Layers
{
    class PitchLayer : Layer
    {
        private const double F0Male = 120;
        private const double F0Female = 240;
		
        private bool shakeAscend = false;
        private double shakeOffset = 0.0;
        private double shakeTimer = 0.0;
        private double shakeOffsetAtten = 0.9;
		private double vibTimer = 0.0;
        private readonly RNG rng;

        public PitchLayer(Synthesizer synthesizer) : base(synthesizer)
        {
            rng = new RNG();
        }

        public override void Update(ref double sample)
        {
            if ((shakeTimer -= synth.TimeStep) <= 0.0)
            {
                shakeTimer = rng.NextDouble(0.01, 0.2);
                shakeOffsetAtten = rng.NextDouble(0.9, 0.99999999999);
                shakeAscend = !shakeAscend;
            }

            // Update normalized shake offset
            if (shakeAscend)
            {
                shakeOffset += (1.0 - shakeOffset) * shakeOffsetAtten * synth.TimeStep * synth.Voice.VoiceShakeAscendRate;
            }
            else
            {
                shakeOffset += (-1.0 - shakeOffset) * shakeOffsetAtten * synth.TimeStep * synth.Voice.VoiceShakeDescendRate;
            }

            // Apply base frequency for selected gender
            switch(synth.Voice.Gender)
            {
                case VoiceGender.Male:
                    synth.Fundamental = F0Male * synth.Voice.FundamentalFrequencyMultiplier;
                    break;
                case VoiceGender.Female:
                    synth.Fundamental = F0Female * synth.Voice.FundamentalFrequencyMultiplier;
                    break;
                default:
                    synth.Fundamental = 150.0 * synth.Voice.FundamentalFrequencyMultiplier;
                    break;
            }

            synth.Fundamental *= Math.Pow(2.0, synth.Pitch);

			// Apply vibrato if available
			if (synth.Voice.VibratoAmount > 0)
			{
				vibTimer = (vibTimer + synth.TimeStep) % 1.0;

				synth.Fundamental *= Math.Pow(2.0, synth.Voice.VibratoAmount * Math.Sin(vibTimer * Math.PI * 2.0 * synth.Voice.VibratoSpeed));
			}

            // Apply offset
            synth.Fundamental += shakeOffset * synth.Voice.VoiceShakeAmountHz;
        }
    }
}
