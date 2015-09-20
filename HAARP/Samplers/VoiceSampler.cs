using System;

using HAARP.FunctionCurves;

namespace HAARP.Samplers
{
    internal class VoiceSampler : Sampler
    {
        private readonly RNG rng;
        private float outputSample;
        private float curveSample;

        private readonly VocalHarmonics vocal;
        private readonly BandPassFilter[] bands;
        private readonly Curve curve;
        private readonly int numBands;

        public VoiceSampler(Synthesizer synth, long seed) : base(synth)
        {
            rng = new RNG(seed);
            bands = new[]
            {
                new BandPassFilter(610, 610, synth.SampleRate, 1.0f, 1.0f),
                new BandPassFilter(1900, 1900, synth.SampleRate, 1.0f, 1.0f),  

                //new BandPassFilter(200, 1200, synth.SampleRate, 1.0f, 1.0f) { Volume = 0.075f }, 
                //new BandPassFilter(1900, 5500, synth.SampleRate, .35f, .35f) { Volume = 0.01f }, 

                //new BandPassFilter(6700, 7200, synth.SampleRate, .1f, .1f) { Volume = 0.0060f }, 
                //new BandPassFilter(4600, 5400, synth.SampleRate, .18f, .18f) { Volume = 0.060f }, 
                //new BandPassFilter(3700, 4100, synth.SampleRate, .2f, .2f) { Volume = 0.050f }, 
                //new BandPassFilter(2100, 2500, synth.SampleRate, .2f, .2f) { Volume = 0.1f }
            };

            numBands = bands.Length;

            curve = new CosineCurve(new[]
            {
                new Keyframe(0.0f, 0.0f),
                new Keyframe(.17f, 1.0f),
                new Keyframe(.4f, .75f),
                new Keyframe(1.4f, .85f),
                new Keyframe(1.6f, .7f),
                new Keyframe(2.2f, .9f),
                new Keyframe(2.7f, .7f),
                new Keyframe(2.8f, 0.8f),
                new Keyframe(3.0f, 0.0f)
            });

            vocal = new VocalHarmonics(32, 100, synth.Voice.SpectralTilt);
        }

        public override void Update(ref float sample)
        {
            vocal.Update(synth.TimeStep);
            curveSample = curve[synth.TimePosition];
            outputSample = vocal.Output * curveSample * 0.05f;
            //outputSample += rng.NextSingle(-1, 1) * curveSample * synth.Voice.FricativeForce;
            sample += outputSample;
            for (int i = 0; i < numBands; i++)
            {
                bands[i].Update(outputSample);
                sample += bands[i].Value * (1 - vocal.FundamentalState);
            }
        }
    }
}