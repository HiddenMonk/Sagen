namespace HAARP
{
    internal class Generator
    {
        public Synthesizer Synth { get; }
        public float PitchDelta { get; set; } = 1.0f;

        public Generator(Synthesizer synth)
        {
            Synth = synth;
        }


    }
}