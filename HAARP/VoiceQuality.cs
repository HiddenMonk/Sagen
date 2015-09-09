namespace HAARP
{
    public enum VoiceQuality
    {
        /// <summary>
        /// Very high quality (44800 Hz). Poorest performance.
        /// </summary>
        VeryHigh = 44800,
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