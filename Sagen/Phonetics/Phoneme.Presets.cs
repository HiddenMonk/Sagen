using System.Collections.Generic;

namespace Sagen.Phonetics
{
	internal partial class Phoneme
	{
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
			Vowel("i", CLOSE, FRONT, UNROUNDED); // 'ie' in 'wie' (German)
			Vowel("y", CLOSE, FRONT, ROUNDED); // 'ü' in 'über' (German)
			Vowel("I\\", CLOSE, CENTRAL, UNROUNDED);
			Vowel("U\\", CLOSE, CENTRAL, ROUNDED);
			Vowel("M", CLOSE, BACK, UNROUNDED);
			Vowel("u", CLOSE, BACK, ROUNDED); // 'u' in 'Fuß' (German)

			// Nease vowels
			Vowel("U", NEAR_CLOSE, NEAR_BACK, ROUNDED);
			Vowel("I", NEAR_CLOSE, NEAR_FRONT, UNROUNDED);
			Vowel("Y", NEAR_CLOSE, NEAR_FRONT, ROUNDED);

			// Clod vowels
			Vowel("e", CLOSE_MID, FRONT, UNROUNDED); // 'ee' in 'Seele' (German)
			Vowel("2", CLOSE_MID, FRONT, ROUNDED); // 'ö' in 'schön' (German)
			Vowel("@\\", CLOSE_MID, CENTRAL, UNROUNDED); // 'e' in 'bitte' (German)
			Vowel("8", CLOSE_MID, CENTRAL, ROUNDED); // 'oo' in 'foot' (UK English)
			Vowel("7", CLOSE_MID, BACK, UNROUNDED);
			Vowel("o", CLOSE_MID, BACK, ROUNDED); // 'o' in 'oder' (German)

			// Midls
			Vowel("e_o", MID, FRONT, UNROUNDED);
			Vowel("@", MID, CENTRAL, UNROUNDED); // 'u' in 'under' (US English)
			Vowel("2_o", MID, BACK, ROUNDED);

			// Open-mid vowels
			Vowel("E", OPEN_MID, FRONT, UNROUNDED); // 'e' in 'ten' (US English)
			Vowel("9", OPEN_MID, FRONT, ROUNDED); // 'ö' in 'löschen' (German)
			Vowel("3", OPEN_MID, CENTRAL, UNROUNDED); // 'ir' in 'bird' (UK English)
			Vowel("3\\", OPEN_MID, CENTRAL, ROUNDED); // 'um' in 'Parfum' (German)
			Vowel("V", OPEN_MID, BACK, UNROUNDED);
			Vowel("O", OPEN_MID, BACK, ROUNDED); // 'o' in 'voll' (German)

			// Near-open vowels
			Vowel("{", NEAR_OPEN, FRONT, UNROUNDED); // 'a' in 'cat' (US English)
			Vowel("6", NEAR_OPEN, CENTRAL, UNROUNDED); // 'er' in 'oder' (German)

			// Open vowels
			Vowel("a", OPEN, FRONT, UNROUNDED); // 'a' in 'aber' (German)
			Vowel("&", OPEN, FRONT, ROUNDED);
			Vowel("A", OPEN, BACK, UNROUNDED); // 'o' in 'hot' (US English)
			Vowel("Q", OPEN, BACK, ROUNDED); // 'o' in 'not' (UK English)

			// Nasals
			Consonant("m", ArticulationPlace.Bilabial, ArticulationManner.Nasal, true);
			Consonant("n", ArticulationPlace.Alveolar, ArticulationManner.Nasal, true);
			Consonant("N", ArticulationPlace.Velar, ArticulationManner.Nasal, true);

			// Stops
			Consonant("p", ArticulationPlace.Bilabial, ArticulationManner.Stop, false);
			Consonant("b", ArticulationPlace.Bilabial, ArticulationManner.Stop, true);
			Consonant("t", ArticulationPlace.Alveolar, ArticulationManner.Stop, false);
			Consonant("d", ArticulationPlace.Alveolar, ArticulationManner.Stop, true);
			Consonant("k", ArticulationPlace.Velar, ArticulationManner.Stop, false);
			Consonant("g", ArticulationPlace.Velar, ArticulationManner.Stop, true);

			// Fricati
			Consonant("f", ArticulationPlace.Labiodental, ArticulationManner.Fricative, false);
			Consonant("v", ArticulationPlace.Labiodental, ArticulationManner.Fricative, true);
			Consonant("T", ArticulationPlace.Dental, ArticulationManner.Fricative, false);
			Consonant("D", ArticulationPlace.Dental, ArticulationManner.Fricative, true);
			Consonant("s", ArticulationPlace.Alveolar, ArticulationManner.Fricative, false);
			Consonant("z", ArticulationPlace.Alveolar, ArticulationManner.Fricative, true);
			Consonant("S", ArticulationPlace.PalatoAlveolar, ArticulationManner.Fricative, false);
			Consonant("Z", ArticulationPlace.PalatoAlveolar, ArticulationManner.Fricative, true);
			Consonant("C", ArticulationPlace.Palatal, ArticulationManner.Fricative, false);
			Consonant("x", ArticulationPlace.Velar, ArticulationManner.Fricative, false);
			Consonant("R", ArticulationPlace.Uvular, ArticulationManner.Fricative, true);
			Consonant("h", ArticulationPlace.Glottal, ArticulationManner.Fricative, false);
			
			// Affricates
			Consonant("tS", ArticulationPlace.PalatoAlveolar, ArticulationManner.Affricate, false); // 'ch' in 'chocolate' (US English)
			Consonant("dZ", ArticulationPlace.PalatoAlveolar, ArticulationManner.Affricate, true); // 'j' in 'jump' (US English)

			// Approximants
			Consonant("r*", ArticulationPlace.Alveolar, ArticulationManner.Approximant, true);
			Consonant("r-", ArticulationPlace.Postalveolar, ArticulationManner.Approximant, true);
			Consonant("j", ArticulationPlace.Palatal, ArticulationManner.Approximant, true);
			Consonant("w", ArticulationPlace.LabializedVelar, ArticulationManner.Approximant, true);

			// Lateral approximants
			Consonant("l", ArticulationPlace.Alveolar, ArticulationManner.LateralApproximant, true);
			Consonant("5", ArticulationPlace.VelarizedAlveolar, ArticulationManner.LateralApproximant, true); // 'l' in 'feel' (US English)
		}

		private static void Vowel(string xsampa, float openness, float backness, float roundedness)
		{
			var vowel = new Phoneme(openness, backness, roundedness);
			_presetsXSAMPA[xsampa] = vowel;
		}

		private static void Consonant(string xsampa, ArticulationPlace place, ArticulationManner type, bool voiced)
		{
			var consonant = new Phoneme(place, type, voiced);
			_presetsXSAMPA[xsampa] = consonant;
		}

		public static Phoneme GetPreset(string xsampa)
		{
			Phoneme p;
			return _presetsXSAMPA.TryGetValue(xsampa, out p) ? p : null;
		}
	}
}