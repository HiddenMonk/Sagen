using System;

using Sagen;
using Sagen.Playback.OpenAL;
using Sagen.Playback.XAudio2;

namespace SagenConsole
{
	class Program
	{
		private const string TestString = "playground";

		static void Main(string[] args)
		{
			var tts = new TTS<XAudio2PlaybackEngine>(Voice.Greg);

			Console.WriteLine("Playing...");
#if DEBUG
			tts.SpeakToFile("speech.wav", TestString);
#endif
			tts.Speak(TestString);
			tts.Sync();

			Console.WriteLine("Done. Press any key to exit.");
			Console.ReadKey();
		}
	}
}
