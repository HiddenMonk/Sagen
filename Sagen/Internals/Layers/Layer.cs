namespace Sagen.Internals.Layers
{
    internal abstract class Layer
    {
        protected readonly Synthesizer synth;

        public bool Enabled { get; set; } = true;
        public abstract void Update(ref double sample);

        public Layer(Synthesizer synthesizer)
        {
            synth = synthesizer;
        }
    }
}