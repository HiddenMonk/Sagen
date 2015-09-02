namespace HAARP.Generators
{
    public abstract class Generator
    {
        public abstract float Sample(float timeSeconds);

        public float Sample(int sample, float sampleRate) => Sample(sample / sampleRate);

        public void Sample(float[] data, int sample, float sampleRate) => data[sample] = Sample(sample / sampleRate);

        public void SampleAdd(float[] data, int sample, float sampleRate) => data[sample] += Sample(sample / sampleRate);
    }
}