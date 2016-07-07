using System;

using Sagen;

namespace HaarpConsole
{
	class Program
	{
		static void Main(string[] args)
		{
			var tts = new TTS();

			Console.WriteLine("Playing...");
			tts.Speak("This is a test.");
			TTS.Sync();

			Console.WriteLine("Done. Press any key to exit.");
			Console.ReadKey();
		}
	}
}
