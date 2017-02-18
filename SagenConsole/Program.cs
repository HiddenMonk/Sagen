using System;

using Sagen;

namespace SagenConsole
{
	class Program
	{
		private const string TestString = "playground";

		static void Main(string[] args)
		{
			var tts = new TTS(Voice.Jimmy);

			Console.WriteLine("Playing...");
#if DEBUG
            tts.SpeakToFile("speech.wav", TestString);
#endif
            tts.Speak(TestString);
            
			tts.Sync();

			Console.WriteLine("Done. Press any key to exit.");
			Console.ReadKey();

			tts.Dispose();
		}
	}
}
