namespace HAARP.Samplers
{
    internal abstract class Sampler
    {
        protected readonly Synthesizer synth;

        public bool Enabled { get; set; } = true;
        public abstract void Update(ref float sample);

        public Sampler(Synthesizer synthesizer)
        {
            synth = synthesizer;
        }
    }
}