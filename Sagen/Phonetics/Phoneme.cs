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

namespace Sagen.Phonetics
{
    internal partial class Phoneme
    {
        /// <summary>
        /// The place of articulation.
        /// </summary>
        public readonly ConsonantPlace ArticulationPlace;

        /// <summary>
        /// The type of articulation.
        /// </summary>
        public readonly ConsonantManner ArticulationType;

        /// <summary>
        /// The backness (position of the tongue relative to the back of the mouth).
        /// The second formant, F2, correlates directly with this vocal quality.
        /// </summary>
        public readonly float Backness;

        /// <summary>
        /// The height (tongue height).
        /// The first formant, F1, inversely correlates with this vocal quality.
        /// </summary>
        public readonly float Height;

        /// <summary>
        /// Determines if the phoneme is voiced.
        /// </summary>
        public readonly bool IsVoiced;

        /// <summary>
        /// The roundedness (of the lips).
        /// The second formant is typically affected by roundedness.
        /// </summary>
        public readonly float Roundedness;

        public Phoneme(float height, float backness, float roundedness, bool voiced = true,
            ConsonantPlace artPlace = ConsonantPlace.None, ConsonantManner artType = ConsonantManner.Fricative)
        {
            IsVoiced = voiced;
            Height = height;
            Backness = backness;
            Roundedness = roundedness;
            ArticulationPlace = artPlace;
            ArticulationType = artType;
        }

        public Phoneme(float height, float backness, float roundedness)
        {
            IsVoiced = true;
            ArticulationPlace = ConsonantPlace.None;
            ArticulationType = ConsonantManner.Fricative;
            Height = height;
            Backness = backness;
            Roundedness = roundedness;
        }

        public Phoneme(ConsonantPlace artPlace, ConsonantManner artType, bool voiced = false)
        {
            IsVoiced = voiced;
            ArticulationPlace = artPlace;
            ArticulationType = artType;
        }
    }
}