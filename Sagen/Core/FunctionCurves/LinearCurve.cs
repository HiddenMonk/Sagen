namespace Sagen.Core.FunctionCurves
{
    internal class LinearCurve : Curve
    {
        public LinearCurve(Keyframe[] keyframes) : base(keyframes)
        {
        }

        protected override float Interpolate(Keyframe left, Keyframe right, float delta)
        {
            return Util.Lerp(left.Value, right.Value, delta);
        }
    }
}