namespace HAARP.Samplers
{
    internal class FadeSampler : Sampler
    {
        public FadeType FadeType { get; set; } = FadeType.FadeIn;
        public float FadeLength { get; set; } = 1.0f;

        public FadeSampler(Synthesizer synth) : base(synth)
        {
            
        }

        public override void Update(ref float sample)
        {
            switch (FadeType)
            {
                case FadeType.FadeIn:
                    if (synth.TimePosition < FadeLength)
                    {
                        sample = Mathe.CosineInterpolate(0.0f, sample, synth.TimePosition / FadeLength);
                    }
                    break;
                case FadeType.FadeOut:
                    if (synth.Length - synth.TimePosition < FadeLength)
                    {
                        sample = Mathe.CosineInterpolate(0.0f, sample, (synth.Length - synth.TimePosition) / FadeLength);
                    }
                    break;
            }
        }
    }

    internal enum FadeType
    {
        FadeIn,
        FadeOut
    }
}