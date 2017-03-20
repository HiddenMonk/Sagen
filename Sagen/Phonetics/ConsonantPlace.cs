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

using System;

namespace Sagen.Phonetics
{
    /// <summary>
    /// The place of articulation determines the passive articulator, as well as the active articulator's orientation within
    /// the vocal tract.
    /// </summary>
    [Flags]
    public enum ConsonantPlace
    {
        /// <summary>
        /// No consonant.
        /// </summary>
        None = 0b000_000_00_0000_0000,

        /// <summary>
        /// Bilabial articulation is created by the lips.
        /// </summary>
        Bilabial = 0b000_000_00_0000_0001,

        /// <summary>
        /// Labiodental articulation is created by interaction between the teeth and lips.
        /// </summary>
        Labiodental = 0b000_000_00_0000_0010,

        /// <summary>
        /// Dental articulation is created by the teeth.
        /// </summary>
        Dental = 0b000_000_00_0000_0100,

        /// <summary>
        /// Alveolar articulation is created by interaction between the tip of the tongue and the alveolar ridge.
        /// </summary>
        Alveolar = 0b000_000_00_0000_1000,

        /// <summary>
        /// Postalveolar articulation is created by interaction between the tip of the tongue and the back of the alveolar ridge.
        /// </summary>
        Postalveolar = 0b000_000_00_0001_0000,

        /// <summary>
        /// Palato-alveolar articulation is created by interaction between the tongue and the space between the alveolar ridge and
        /// the hard palate.
        /// This type of articulation is typically formed with a domed (slightly contracted) tongue.
        /// </summary>
        PalatoAlveolar = 0b000_000_00_0010_0000,

        /// <summary>
        /// Palatal articulation is created by interaction between the tongue and hard palate.
        /// </summary>
        Palatal = 0b000_000_00_0100_0000,

        /// <summary>
        /// Velar articulation is created by interaction between the back of the tongue and the soft palate (velum).
        /// </summary>
        Velar = 0b000_000_00_1000_0000,

        /// <summary>
        /// Uvular articulation is created by interaction between the back of the tongue and the uvula.
        /// </summary>
        Uvular = 0b000_000_01_0000_0000,

        /// <summary>
        /// Glottal articulation is created by the glottis.
        /// </summary>
        Glottal = 0b000_000_10_0000_0000
    }
}