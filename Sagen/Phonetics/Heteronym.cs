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
    /// Provides types of distinguishing characteristics for resolving heteronyms.
    /// </summary>
    public enum Heteronym
    {
        /// <summary>
        /// Indicates a heteronym that is distinguished by being a noun.
        /// <para>Example: "desert [geography]" is the nominal heteronym of "desert [to abandon]".</para>
        /// </summary>
        Noun,

        /// <summary>
        /// Indicates a heteronym that is distinguished by being an adjective.
        /// <para>Example: "content [satisfied]" is the adjectival heteronym of "content [information]".</para>
        /// </summary>
        Adjective,

        /// <summary>
        /// Indicates a heteronym that is distinguished by being a verb.
        /// <para>Example: "conduct [to lead]" is the verbal heteronym of "conduct [action]".</para>
        /// </summary>
        Verb,

        /// <summary>
        /// Indicates a heteronym that is distinguished by being an adverb.
        /// <para>Example: "multiply [in multiple ways]" is the adverbial heteronym of "multiply [to increase]".</para>
        /// </summary>
        Adverb,

        /// <summary>
        /// Indicates a heteronym that is distinguished by representing an animate object.
        /// <para>Example: "bass [fish]" is the animate heteronym of "bass [acoustics]".</para>
        /// </summary>
        Animate,

        /// <summary>
        /// Indicates a heteronym that is distinguished by representing an inanimate object.
        /// <para>Example: "sewer [drainage pipes]" is the inanimate heteronym of "sewer [one who sews]".</para>
        /// </summary>
        Inanimate,

        /// <summary>
        /// Indicates a heteronym that is distinguished by stressing any non-primary syllable.
        /// <para>
        /// Example: "console [to provide comfort from grief]" is the non-primary stress heteronym of "console [control
        /// unit]".
        /// </para>
        /// </summary>
        NonPrimaryStress,

        /// <summary>
        /// Indicates a heteronym that is distinguished by stressing the first syllable.
        /// <para>Example: "defense [sports]" is the primary-stress heteronym of "defense [protection]".</para>
        /// </summary>
        PrimaryStress,

        /// <summary>
        /// Indicates a heteronym that is distinguished a rounded primary vowel.
        /// <para>Example: "bow [weapon]" is the rounded heteronym of "bow [front of a ship]".</para>
        /// </summary>
        Rounded,

        /// <summary>
        /// Indicates a heteronym that is distinguished by an unrounded primary vowel.
        /// <para>Example: "putting [golf]" is the unrounded heteronym of "putting [to put]".</para>
        /// </summary>
        Unrounded
    }
}