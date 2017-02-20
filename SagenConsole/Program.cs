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
			Console.WriteLine("Loading...");
			var tts = new TTS<XAudio2PlaybackEngine>(Voice.Greg);
			
#if DEBUG
			Console.WriteLine("Exporting to WAV...");
			tts.SpeakToFile("speech.wav", TestString);
#endif
			Console.WriteLine("Playing...");
			tts.Speak(TestString);
			tts.Sync();

			Console.WriteLine("Done. Press any key to exit.");
			Console.ReadKey();
		}
	}
}
