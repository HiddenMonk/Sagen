namespace Sagen.Pronunciation
{
	/// <summary>
	/// The place of articulation determines the active articulator, as well as its positioning within the vocal tract.
	/// </summary>
	public enum ArticulationPlace : byte
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
		/// Palato-alveolar articulation is created by interaction between the tongue and the space between the alveolar ridge and the hard palate.
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