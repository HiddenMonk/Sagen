namespace HAARP
{
    internal static class Mathe
    {
        public static float Lerp(float a, float b, float delta) => a + (b - a) * delta;
        public static double Lerp(double a, double b, double delta) => a + (b - a) * delta;
    }
}