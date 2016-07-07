namespace Sagen
{
	public enum SampleFormat : short
	{
		/// <summary>
		/// 64-bit floating-point numbers normalized to [-1, 1].
		/// </summary>
		Float64 = 64,
		/// <summary>
		/// 32-bit floating-point numbers normalized to [-1, 1].
		/// </summary>
		Float32 = 32,
		/// <summary>
		/// 16-bit signed integers.
		/// </summary>
		Signed16 = 16,
		/// <summary>
		/// 8-bit signed integers, for when you really want to butcher the quality.
		/// </summary>
		Unsigned8 = 8
	}
}