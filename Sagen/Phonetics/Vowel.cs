namespace Sagen.Phonetics
{
	internal class Vowel
	{
		public double Height, Backness, Roundedness, Nasalization;

		public Vowel(double height, double backness, double roundedness, double nasalization)
		{
			Height = height;
			Backness = backness;
			Roundedness = roundedness;
			Nasalization = nasalization;
		}
	}
}
