using System;

namespace HAARP
{
    public struct SampleTransform
    {
        public float SourceLower;
        public float SourceUpper;
        public float DestLower;
        public float DestUpper;

        public SampleTransform(float sourceLower, float sourceUpper, float destLower, float destUpper)
        {
            SourceLower = sourceLower;
            SourceUpper = sourceUpper;
            DestLower = destLower;
            DestUpper = destUpper;
        }

        public static readonly SampleTransform Default = new SampleTransform(-1.0f, 1.0f, -1.0f, 1.0f);

        public void GetTransformConstants(out float offset, out float mult)
        {
            mult = (SourceUpper - SourceLower) / (DestUpper - DestLower);
            offset = Math.Min(DestLower, DestUpper) - Math.Min(SourceLower, SourceUpper);
        }
    }
}