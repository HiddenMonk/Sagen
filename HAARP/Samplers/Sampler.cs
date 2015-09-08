namespace HAARP.Samplers
{
    internal abstract class Sampler
    {
        public abstract void Update(Synthesizer synth, ref float sample);
    }
}