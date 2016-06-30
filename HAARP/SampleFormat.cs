namespace HAARP
{
	public enum SampleFormat
	{
		/// <summary>
		/// 64-bit floating-point numbers normalized to [-1, 1].
		/// </summary>
		Float64,
		/// <summary>
		/// 32-bit floating-point numbers normalized to [-1, 1].
		/// </summary>
		Float32,
		/// <summary>
		/// 16-bit signed integers.
		/// </summary>
		Signed16,
		/// <summary>
		/// 8-bit signed integers, for when you really want to butcher the quality.
		/// </summary>
		Unsigned8
	}
}