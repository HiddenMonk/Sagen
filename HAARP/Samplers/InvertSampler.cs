namespace HAARP.Samplers
{
    internal class InvertSampler : Sampler
    {
        public override void Update(Synthesizer synth, ref float sample)
        {
            sample = -sample;
        }
    }
}