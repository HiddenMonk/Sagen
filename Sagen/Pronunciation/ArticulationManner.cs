namespace Sagen.Pronunciation
{
	/// <summary>
	/// The manner of articulation determines how the articulators behave when producing sound.
	/// </summary>
	public enum ArticulationManner : byte
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
		/// Lateral approximants are approximants with lateralization, meaning that airflow is directed past the sides of the tongue rather than over it.
		/// </summary>
		LateralApproximant,

		/// <summary>
		/// Vowels are voiced sounds produced by unobstructed, unarticulated airflow flowing through the vocal tract.
		/// </summary>
		Vowel
	}
}