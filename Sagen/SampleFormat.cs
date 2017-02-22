namespace Sagen
{
	/// <summary>
	/// Defines sample formats for PCM speech data.
	/// </summary>
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
		/// 16-bit signed integers normalized to [-32768, 32767].
		/// </summary>
		Signed16 = 16,

		/// <summary>
		/// 8-bit unsigned integers normalized to [0, 255] for that extra-butchered sample quality you love.
		/// </summary>
		Unsigned8 = 8
	}
}