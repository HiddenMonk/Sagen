#region License

// https://github.com/TheBerkin/Sagen
// 
// Copyright (c) 2017 Nicholas Fleck
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in the
// Software without restriction, including without limitation the rights to use, copy,
// modify, merge, publish, distribute, sublicense, and/or sell copies of the Software,
// and to permit persons to whom the Software is furnished to do so, subject to the
// following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A
// PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE
// OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

#endregion

using System.Collections.Generic;

namespace Sagen.Phonetics
{
    internal partial class Phoneme
    {
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
        private static readonly Dictionary<string, Phoneme> _presetsXSAMPA = new Dictionary<string, Phoneme>();

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
            Consonant("m", ConsonantPlace.Bilabial, ConsonantManner.Nasal, true);
            Consonant("n", ConsonantPlace.Alveolar, ConsonantManner.Nasal, true);
            Consonant("N", ConsonantPlace.Velar, ConsonantManner.Nasal, true);

            // Stops
            Consonant("p", ConsonantPlace.Bilabial, ConsonantManner.Stop, false);
            Consonant("b", ConsonantPlace.Bilabial, ConsonantManner.Stop, true);
            Consonant("t", ConsonantPlace.Alveolar, ConsonantManner.Stop, false);
            Consonant("d", ConsonantPlace.Alveolar, ConsonantManner.Stop, true);
            Consonant("k", ConsonantPlace.Velar, ConsonantManner.Stop, false);
            Consonant("g", ConsonantPlace.Velar, ConsonantManner.Stop, true);

            // Fricati
            Consonant("f", ConsonantPlace.Labiodental, ConsonantManner.Fricative, false);
            Consonant("v", ConsonantPlace.Labiodental, ConsonantManner.Fricative, true);
            Consonant("T", ConsonantPlace.Dental, ConsonantManner.Fricative, false);
            Consonant("D", ConsonantPlace.Dental, ConsonantManner.Fricative, true);
            Consonant("s", ConsonantPlace.Alveolar, ConsonantManner.Fricative, false);
            Consonant("z", ConsonantPlace.Alveolar, ConsonantManner.Fricative, true);
            Consonant("S", ConsonantPlace.PalatoAlveolar, ConsonantManner.Fricative, false);
            Consonant("Z", ConsonantPlace.PalatoAlveolar, ConsonantManner.Fricative, true);
            Consonant("C", ConsonantPlace.Palatal, ConsonantManner.Fricative, false);
            Consonant("x", ConsonantPlace.Velar, ConsonantManner.Fricative, false);
            Consonant("R", ConsonantPlace.Uvular, ConsonantManner.Fricative, true);
            Consonant("h", ConsonantPlace.Glottal, ConsonantManner.Fricative, false);

            // Affricates
            Consonant("tS", ConsonantPlace.PalatoAlveolar, ConsonantManner.Affricate, false); // 'ch' in 'chocolate' (US English)
            Consonant("dZ", ConsonantPlace.PalatoAlveolar, ConsonantManner.Affricate, true); // 'j' in 'jump' (US English)

            // Approximants
            Consonant("r*", ConsonantPlace.Alveolar, ConsonantManner.Approximant, true);
            Consonant("r-", ConsonantPlace.Postalveolar, ConsonantManner.Approximant, true);
            Consonant("j", ConsonantPlace.Palatal, ConsonantManner.Approximant, true);
            Consonant("w", ConsonantPlace.LabializedVelar, ConsonantManner.Approximant, true);

            // Lateral approximants
            Consonant("l", ConsonantPlace.Alveolar, ConsonantManner.LateralApproximant, true);
            Consonant("5", ConsonantPlace.VelarizedAlveolar, ConsonantManner.LateralApproximant, true); // 'l' in 'feel' (US English)
        }

        private static void Vowel(string xsampa, float openness, float backness, float roundedness)
        {
            var vowel = new Phoneme(openness, backness, roundedness);
            _presetsXSAMPA[xsampa] = vowel;
        }

        private static void Consonant(string xsampa, ConsonantPlace place, ConsonantManner type, bool voiced)
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