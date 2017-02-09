namespace Sagen.Internals.Filters
{
    internal abstract class Filter
    {
        protected readonly Synthesizer synth;

        public bool Enabled { get; set; } = true;
        public abstract void Update(ref double sample);

        public Filter(Synthesizer synthesizer)
        {
            synth = synthesizer;
        }
    }
}