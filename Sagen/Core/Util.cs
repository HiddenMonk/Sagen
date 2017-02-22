using System;

namespace Sagen.Core
{
    internal static class Util
    {
        public static float PIf = (float)Math.PI;

        public static float Lerp(float a, float b, float delta) => a + (b - a) * delta;
        public static double Lerp(double a, double b, double delta) => a + (b - a) * delta;

        public static double CosineInterpolate(double a, double b, double delta)
        {
            delta = (1.0f - Math.Cos(delta * Math.PI)) / 2.0f;
            return a * (1.0f - delta) + b * delta;
        }

        public static float CosineInterpolate(float a, float b, float delta)
        {
            delta = (1.0f - (float)Math.Cos(delta * PIf)) / 2.0f;
            return a * (1.0f - delta) + b * delta;
        }

        public static double FactorToDecibels(double factor) => Math.Log10(factor) * 10.0;

        public static double DecibelsToFactor(double dB) => Math.Pow(10.0, dB / 10.0);
    }
}