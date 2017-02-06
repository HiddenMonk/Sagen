namespace Sagen.Phonetics
{
	internal sealed class PhoneticEvent
	{
		private readonly bool _syllableBreak;
		private readonly Stress _stress;
		private readonly Phoneme _phoneme;

		public PhoneticEvent(Phoneme phoneme)
		{
			_phoneme = phoneme;
		}
	}
}