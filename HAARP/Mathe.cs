using System;

namespace HAARP
{
    internal static class Mathe
    {
        public static float PIf = (float)Math.PI;

        public static float Lerp(float a, float b, float delta) => a + (b - a) * delta;
        public static double Lerp(double a, double b, double delta) => a + (b - a) * delta;

        public static double CosineInterpolate(double a, double b, double delta)
        {
            delta = (1 - Math.Cos(delta * Math.PI)) / 2;
            return (a * (1 - delta) + b * delta);
        }

        public static float CosineInterpolate(float a, float b, float delta)
        {
            delta = (1 - (float)Math.Cos(delta * PIf)) / 2;
            return (a * (1 - delta) + b * delta);
        }
    }
}