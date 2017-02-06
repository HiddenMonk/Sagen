namespace Sagen.Internals.FunctionCurves
{
    internal class CosineCurve : Curve
    {
        public CosineCurve(Keyframe[] keyframes) : base(keyframes)
        {
        }

        protected override float Interpolate(Keyframe left, Keyframe right, float delta)
        {
            return Util.CosineInterpolate(left.Value, right.Value, delta);
        }
    }
}