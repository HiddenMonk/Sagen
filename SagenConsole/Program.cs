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
			var tts = new TTS<OpenALPlaybackEngine>(Voice.Jimmy);
			string input;

			while (true)
			{
				Console.Write("> ");
				input = Console.ReadLine().Trim();
#if DEBUG
				Console.WriteLine("Exporting to WAV...");
				tts.SpeakToFile("speech.wav", input);
#endif
				Console.WriteLine("Playing...");
				tts.Speak(input);
				tts.Sync();
			}


			Console.WriteLine("Done. Press any key to exit.");
			Console.ReadKey();
		}
	}
}
