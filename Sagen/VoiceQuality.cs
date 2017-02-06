namespace Sagen
{
	/// <summary>
	/// Specifies available audio quality levels (sample rates) for the TTS engine.
	/// </summary>
	public enum VoiceQuality
	{
		/// <summary>
		/// Very high quality (48000 Hz). Slowest performance.
		/// </summary>
		VeryHigh = 48000,
		/// <summary>
		/// High quality (44100 Hz). This is the default setting.
		/// </summary>
		High = 44100,
		/// <summary>
		/// Medium quality (22050 Hz). Fast performance.
		/// </summary>
		Medium = 22050,
		/// <summary>
		/// Low quality (11025 Hz). Faster performance.
		/// </summary>
		Low = 11025,
		/// <summary>
		/// Awful quality (8000 Hz). Best performance.
		/// </summary>
		Awful = 8000
	}
}