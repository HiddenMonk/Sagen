namespace Sagen.Extensibility
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
        /// <para>Example: "console [to provide comfort from grief]" is the non-primary stress heteronym of "console [control unit]".</para>
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
