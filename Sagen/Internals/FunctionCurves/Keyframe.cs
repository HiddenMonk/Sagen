namespace Sagen.Internals.FunctionCurves
{
    internal struct Keyframe
    {
        public float Time;
        public float Value;

        public Keyframe(float time, float value)
        {
            Time = time;
            Value = value;
        }
    }
}