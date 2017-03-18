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
    /// The place of articulation determines the passive articulator, as well as the active articulator's orientation within
    /// the vocal tract.
    /// </summary>
    public enum ConsonantPlace : byte
    {
        /// <summary>
        /// No articulation.
        /// </summary>
        None,

        /// <summary>
        /// Bilabial articulation is created by the lips.
        /// </summary>
        Bilabial,

        /// <summary>
        /// Labiodental articulation is created by interaction between the teeth and lips.
        /// </summary>
        Labiodental,

        /// <summary>
        /// Dental articulation is created by the teeth.
        /// </summary>
        Dental,

        /// <summary>
        /// Alveolar articulation is created by interaction between the tip of the tongue and the alveolar ridge.
        /// </summary>
        Alveolar,

        /// <summary>
        /// Postalveolar articulation is created by interaction between the tip of the tongue and the back of the alveolar ridge.
        /// </summary>
        Postalveolar,

        /// <summary>
        /// Palato-alveolar articulation is created by interaction between the tongue and the space between the alveolar ridge and
        /// the hard palate.
        /// This type of articulation is typically formed with a domed (slightly contracted) tongue.
        /// </summary>
        PalatoAlveolar,

        /// <summary>
        /// Palatal articulation is created by interaction between the tongue and hard palate.
        /// </summary>
        Palatal,

        /// <summary>
        /// Velarized alveolar articulation is created by a combination of velar and alveolar articulation.
        /// </summary>
        VelarizedAlveolar,

        /// <summary>
        /// Velar articulation is created by interaction between the back of the tongue and the soft palate (velum).
        /// </summary>
        Velar,

        /// <summary>
        /// Labialized velar articulation is created by two articulations, at the velum and the lips.
        /// </summary>
        LabializedVelar,

        /// <summary>
        /// Uvular articulation is created by interaction between the back of the tongue and the uvula.
        /// </summary>
        Uvular,

        /// <summary>
        /// Glottal articulation is created by the glottis.
        /// </summary>
        Glottal
    }
}