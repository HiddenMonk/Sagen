using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sagen.Internals.Filters
{
    class IntonationFilter : Filter
    {
        private const double ShakeAmountHz = 5.0;

        private bool up = false;
        private double offset = 0.0;
        private double timer = 0.0;
        private double attenuation = 0.9;
        private const double BaseFrequency = 250;
        private readonly RNG rng;

        public IntonationFilter(Synthesizer synthesizer) : base(synthesizer)
        {
            rng = new RNG();
        }

        public override void Update(ref double sample)
        {
            if ((timer -= synth.TimeStep) <= 0.0)
            {
                timer = rng.NextDouble(0.01, 0.2);
                attenuation = rng.NextDouble(0.9, 0.99999999999);
                up = !up;
            }

            if (up)
            {
                offset += (1.0 - offset) * attenuation * synth.TimeStep * synth.Voice.VoiceShakeAscendRate;
            }
            else
            {
                offset += (-1.0 - offset) * attenuation * synth.TimeStep * synth.Voice.VoiceShakeDescendRate;
            }

            synth.Fundamental = BaseFrequency + offset * ShakeAmountHz;
        }
    }
}
