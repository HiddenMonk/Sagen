namespace Sagen
{
	public sealed class TTS
	{
		public static VoiceQuality Quality = VoiceQuality.VeryHigh;

		public void Speak(string text)
		{

		}

		public static void Sync()
		{
			AudioStream.SyncAll();
		}
	}
}