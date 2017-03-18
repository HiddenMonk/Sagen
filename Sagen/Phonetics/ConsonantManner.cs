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
    /// <summary>
    /// The manner of articulation determines how the articulators behave when producing sound.
    /// </summary>
    public enum ConsonantManner : byte
    {
        /// <summary>
        /// Nasals are consonants produced by directing airflow through the nasal passageway.
        /// </summary>
        Nasal,

        /// <summary>
        /// Stops are consonants produced by a sudden obstruction of airflow through the vocal tract.
        /// </summary>
        Stop,

        /// <summary>
        /// Affricates are consonants produced by transitioning from a stop to a fricative.
        /// </summary>
        Affricate,

        /// <summary>
        /// Fricatives are consonants produced by creating turbulent airflow through a narrow opening.
        /// </summary>
        Fricative,

        /// <summary>
        /// Approximants are consonants produced by creating airflow through an opening just large enough not to create turbulence.
        /// </summary>
        Approximant,

        /// <summary>
        /// Lateral approximants are approximants where the airflow is directed along the sides of the tongue instead of over it.
        /// </summary>
        LateralApproximant
    }
}