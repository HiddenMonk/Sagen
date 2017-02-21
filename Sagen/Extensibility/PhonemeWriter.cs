using System.Collections.Generic;

using Sagen.Phonetics;

namespace Sagen.Extensibility
{
	public sealed class PhonemeWriter
	{
		private readonly PhoneticAlphabet _alphabet;
		private bool _queuedSyllableBreak = false;
		private Stress _queuedStress = Stress.None;


		public PhonemeWriter(PhoneticAlphabet alphabet)
		{
			_alphabet = alphabet;
		}

		public void Write(string symbols)
		{
			
		}
	}
}