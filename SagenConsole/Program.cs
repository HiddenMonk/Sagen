using System;

using Sagen;

namespace SagenConsole
{
	class Program
	{
		static void Main(string[] args)
		{
			var tts = new TTS(Voice.Jimmy);

			Console.WriteLine("Playing...");
            //tts.SpeakToFile("speech.wav", "playground");
			tts.Speak("playground");
            
			tts.Sync();

			Console.WriteLine("Done. Press any key to exit.");
			Console.ReadKey();
		}
	}
}
