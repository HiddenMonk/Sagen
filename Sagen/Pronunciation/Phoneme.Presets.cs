using System.Collections.Generic;

namespace Sagen.Pronunciation
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
			Vowel("i", "i", CLOSE, FRONT, UNROUNDED); // 'ie' in 'wie' (German)
			Vowel("y", "y", CLOSE, FRONT, ROUNDED); // 'ü' in 'über' (German)
			Vowel("ɨ", "I*", CLOSE, CENTRAL, UNROUNDED);
			Vowel("ʉ", "U*", CLOSE, CENTRAL, ROUNDED);
			Vowel("ɯ", "M", CLOSE, BACK, UNROUNDED);
			Vowel("u", "u", CLOSE, BACK, ROUNDED); // 'u' in 'Fuß' (German)

			// Near-close vowels
			Vowel("ʊ", "U", NEAR_CLOSE, NEAR_BACK, ROUNDED);

			// Close-mid vowels
			Vowel("e", "e", CLOSE_MID, FRONT, UNROUNDED); // 'ee' in 'Seele' (German)
			Vowel("ø", "2", CLOSE_MID, FRONT, ROUNDED); // 'ö' in 'schön' (German)
			Vowel("ɘ", "@*", CLOSE_MID, CENTRAL, UNROUNDED); // 'e' in 'bitte' (German)
			Vowel("ɵ", "8", CLOSE_MID, CENTRAL, ROUNDED); // 'oo' in 'foot' (UK English)
			Vowel("ɤ", "7", CLOSE_MID, BACK, UNROUNDED);
			Vowel("o", "o", CLOSE_MID, BACK, ROUNDED); // 'o' in 'oder' (German)

			// Mid vowels
			Vowel("e̞", "e_o", MID, FRONT, UNROUNDED);
			Vowel("ə", "@", MID, CENTRAL, UNROUNDED); // 'u' in 'under' (US English)
			Vowel("o̞", "2_o", MID, BACK, ROUNDED);

			// Open-mid vowels
			Vowel("ɛ", "E", OPEN_MID, FRONT, UNROUNDED); // 'e' in 'ten' (US English)
			Vowel("œ", "9", OPEN_MID, FRONT, ROUNDED); // 'ö' in 'löschen' (German)
			Vowel("ɜ", "3", OPEN_MID, CENTRAL, UNROUNDED); // 'ir' in 'bird' (UK English)
			Vowel("ɞ", "3*", OPEN_MID, CENTRAL, ROUNDED); // 'um' in 'Parfum' (German)
			Vowel("ʌ", "V", OPEN_MID, BACK, UNROUNDED);
			Vowel("ɔ", "O", OPEN_MID, BACK, ROUNDED); // 'o' in 'voll' (German)

			// Near-open vowels
			Vowel("æ", "{", NEAR_OPEN, FRONT, UNROUNDED); // 'a' in 'cat' (US English)
			Vowel("ɐ", "6", NEAR_OPEN, CENTRAL, UNROUNDED); // 'er' in 'oder' (German)

			// Open vowels
			Vowel("a", "a", OPEN, FRONT, UNROUNDED); // 'a' in 'aber' (German)
			Vowel("ɶ", "&", OPEN, FRONT, ROUNDED);
			Vowel("ɑ", "A", OPEN, BACK, UNROUNDED); // 'o' in 'hot' (US English)
			Vowel("ɒ", "Q", OPEN, BACK, ROUNDED); // 'o' in 'not' (UK English)

			// Nasals
			Consonant("m", "m", ArticulationPlace.Bilabial, ArticulationManner.Nasal, true);
			Consonant("n", "n", ArticulationPlace.Alveolar, ArticulationManner.Nasal, true);
			Consonant("ŋ", "N", ArticulationPlace.Velar, ArticulationManner.Nasal, true);

			// Stops
			Consonant("p", "p", ArticulationPlace.Bilabial, ArticulationManner.Stop, false);
			Consonant("b", "b", ArticulationPlace.Bilabial, ArticulationManner.Stop, true);
			Consonant("t", "t", ArticulationPlace.Alveolar, ArticulationManner.Stop, false);
			Consonant("d", "d", ArticulationPlace.Alveolar, ArticulationManner.Stop, true);
			Consonant("k", "k", ArticulationPlace.Velar, ArticulationManner.Stop, false);
			Consonant("g", "g", ArticulationPlace.Velar, ArticulationManner.Stop, true);

			// Fricatives
			Consonant("f", "f", ArticulationPlace.Labiodental, ArticulationManner.Fricative, false);
			Consonant("v", "v", ArticulationPlace.Labiodental, ArticulationManner.Fricative, true);
			Consonant("θ", "T", ArticulationPlace.Dental, ArticulationManner.Fricative, false);
			Consonant("ð", "D", ArticulationPlace.Dental, ArticulationManner.Fricative, true);
			Consonant("s", "s", ArticulationPlace.Alveolar, ArticulationManner.Fricative, false);
			Consonant("z", "z", ArticulationPlace.Alveolar, ArticulationManner.Fricative, true);
			Consonant("ʃ", "S", ArticulationPlace.PalatoAlveolar, ArticulationManner.Fricative, false);
			Consonant("ʒ", "Z", ArticulationPlace.PalatoAlveolar, ArticulationManner.Fricative, true);
			Consonant("ç", "C", ArticulationPlace.Palatal, ArticulationManner.Fricative, false);
			Consonant("x", "x", ArticulationPlace.Velar, ArticulationManner.Fricative, false);
			Consonant("ʁ", "R", ArticulationPlace.Uvular, ArticulationManner.Fricative, true);
			Consonant("h", "h", ArticulationPlace.Glottal, ArticulationManner.Fricative, false);
			
			// Affricates
			Consonant("tʃ", "tS", ArticulationPlace.PalatoAlveolar, ArticulationManner.Affricate, false); // 'ch' in 'chocolate' (US English)
			Consonant("dʒ", "dZ", ArticulationPlace.PalatoAlveolar, ArticulationManner.Affricate, true); // 'j' in 'jump' (US English)

			// Approximants
			Consonant("ɹ", "r*", ArticulationPlace.Alveolar, ArticulationManner.Approximant, true);
			Consonant("ɹ̠", "r-", ArticulationPlace.Postalveolar, ArticulationManner.Approximant, true);
			Consonant("j", "j", ArticulationPlace.Palatal, ArticulationManner.Approximant, true);
			Consonant("w", "w", ArticulationPlace.LabializedVelar, ArticulationManner.Approximant, true);

			// Lateral approximants
			Consonant("l", "l", ArticulationPlace.Alveolar, ArticulationManner.LateralApproximant, true);
			Consonant("ɫ", "5", ArticulationPlace.VelarizedAlveolar, ArticulationManner.LateralApproximant, true); // 'l' in 'feel' (US English)
		}

		private static void Vowel(string ipa, string xsampa, float openness, float backness, float roundedness)
		{
			var vowel = new Phoneme(openness, backness, roundedness);
			_presetsIPA[ipa] = vowel;
			_presetsXSAMPA[xsampa] = vowel;
		}

		private static void Consonant(string ipa, string xsampa, ArticulationPlace place, ArticulationManner type, bool voiced)
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