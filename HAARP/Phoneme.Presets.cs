using System.Collections.Generic;

namespace HAARP
{
	internal partial class Phoneme
	{
		private static readonly Dictionary<string, Phoneme> _presetsIPA = new Dictionary<string, Phoneme>();
		private static readonly Dictionary<string, Phoneme> _presetsXSAMPA = new Dictionary<string, Phoneme>();

		// Height
		private const float CLOSE = 1.0f;
		private const float NEAR_CLOSE = 0.85f;
		private const float CLOSE_MID = 0.65f;
		private const float MID = 0.5f;
		private const float OPEN_MID = 0.35f;
		private const float NEAR_OPEN = 0.15f;
		private const float OPEN = 0.0f;

		// Backness
		private const float FRONT = 0.0f;
		private const float NEAR_FRONT = 0.25f;
		private const float CENTRAL = 0.5f;
		private const float NEAR_BACK = 0.75f;
		private const float BACK = 1.0f;

		// Roundedness
		private const float ROUNDED = 1.0f;
		private const float UNROUNDED = 0.0f;

		static Phoneme()
		{	
			// Close vowels
			AddVowel(@"i", @"i", CLOSE, FRONT, UNROUNDED); // 'ie' in 'wie' (German)
			AddVowel(@"y", @"y", CLOSE, FRONT, ROUNDED); // 'ü' in 'über' (German)
			AddVowel(@"ɨ", @"I\", CLOSE, CENTRAL, UNROUNDED);
			AddVowel(@"ʉ", @"U\", CLOSE, CENTRAL, ROUNDED);
			AddVowel(@"ɯ", @"M", CLOSE, BACK, UNROUNDED);
			AddVowel(@"u", @"u", CLOSE, BACK, ROUNDED); // 'u' in 'Fuß' (German)

			// Near-close vowels
			AddVowel(@"ʊ", @"U", NEAR_CLOSE, NEAR_BACK, ROUNDED);

			// Close-mid vowels
			AddVowel(@"e", @"e", CLOSE_MID, FRONT, UNROUNDED); // 'ee' in 'Seele' (German)
			AddVowel(@"ø", @"2", CLOSE_MID, FRONT, ROUNDED); // 'ö' in 'schön' (German)
			AddVowel(@"ɘ", @"@\", CLOSE_MID, CENTRAL, UNROUNDED); // 'e' in 'bitte' (German)
			AddVowel(@"ɵ", @"8", CLOSE_MID, CENTRAL, ROUNDED); // 'oo' in 'foot' (UK English)
			AddVowel(@"ɤ", @"7", CLOSE_MID, BACK, UNROUNDED);
			AddVowel(@"o", @"o", CLOSE_MID, BACK, ROUNDED); // 'o' in 'oder' (German)

			// Mid vowels
			AddVowel(@"e̞", @"e_o", MID, FRONT, UNROUNDED);
			AddVowel(@"ə", @"@", MID, CENTRAL, UNROUNDED); // 'u' in 'under' (US English)
			AddVowel(@"o̞", @"2_o", MID, BACK, ROUNDED);

			// Open-mid vowels
			AddVowel(@"ɛ", @"E", OPEN_MID, FRONT, UNROUNDED); // 'e' in 'ten' (US English)
			AddVowel(@"œ", @"9", OPEN_MID, FRONT, ROUNDED); // 'ö' in 'löschen' (German)
			AddVowel(@"ɜ", @"3", OPEN_MID, CENTRAL, UNROUNDED); // 'ir' in 'bird' (UK English)
			AddVowel(@"ɞ", @"3\", OPEN_MID, CENTRAL, ROUNDED); // 'um' in 'Parfum' (German)
			AddVowel(@"ʌ", @"V", OPEN_MID, BACK, UNROUNDED);
			AddVowel(@"ɔ", @"O", OPEN_MID, BACK, ROUNDED); // 'o' in 'voll' (German)

			// Near-open vowels
			AddVowel(@"æ", @"{", NEAR_OPEN, FRONT, UNROUNDED); // 'a' in 'cat' (US English)
			AddVowel(@"ɐ", @"6", NEAR_OPEN, CENTRAL, UNROUNDED); // 'er' in 'oder' (German)

			// Open vowels
			AddVowel(@"a", @"a", OPEN, FRONT, UNROUNDED); // 'a' in 'aber' (German)
			AddVowel(@"ɶ", @"&", OPEN, FRONT, ROUNDED);
			AddVowel(@"ɑ", @"A", OPEN, BACK, UNROUNDED); // 'o' in 'hot' (US English)
			AddVowel(@"ɒ", @"Q", OPEN, BACK, ROUNDED); // 'o' in 'not' (UK English)

			// Fricatives
			AddConsonant(@"θ", @"T", ArticulationPlace.Dental, ArticulationType.Fricative, false);
			AddConsonant(@"ð", @"D", ArticulationPlace.Dental, ArticulationType.Fricative, true);
			AddConsonant(@"s", @"s", ArticulationPlace.Alveolar, ArticulationType.Fricative, false);
			AddConsonant(@"z", @"z", ArticulationPlace.Alveolar, ArticulationType.Fricative, true);
			AddConsonant(@"ʃ", @"S", ArticulationPlace.PalatalAlveolar, ArticulationType.Fricative, false);
			
		}

		private static void AddVowel(string ipa, string xsampa, float openness, float backness, float roundedness)
		{
			var vowel = new Phoneme(openness, backness, roundedness);
			_presetsIPA[ipa] = vowel;
			_presetsXSAMPA[xsampa] = vowel;
		}

		private static void AddConsonant(string ipa, string xsampa, ArticulationPlace place, ArticulationType type, bool voiced)
		{
			var consonant = new Phoneme(place, type, voiced);
			_presetsIPA[ipa] = consonant;
			_presetsXSAMPA[xsampa] = consonant;
		}

		public static Phoneme GetPresetIPA(string ipa)
		{
			Phoneme p;
			return _presetsIPA.TryGetValue(ipa, out p) ? p : null;
		}

		public static Phoneme GetPresetXSAMPA(string xsampa)
		{
			Phoneme p;
			return _presetsXSAMPA.TryGetValue(xsampa, out p) ? p : null;
		}
	}
}