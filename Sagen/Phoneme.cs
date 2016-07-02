namespace Sagen
{
	internal partial class Phoneme
	{
		/// <summary>
		/// Determines if the phoneme is voiced.
		/// </summary>
		public readonly bool IsVoiced;

		/// <summary>
		/// The height (tongue height).
		/// The first formant, F1, inversely corresponds to this vocal quality.
		/// </summary>
		public readonly float Height;

		/// <summary>
		/// The backness (position of the tongue relative to the back of the mouth).
		/// The second formant, F2, corresponds directly to this vocal quality.
		/// </summary>
		public readonly float Backness;

		/// <summary>
		/// The roundedness (of the lips).
		/// The third formant, F3, typically represents roundedness.
		/// </summary>
		public readonly float Roundedness;

		/// <summary>
		/// The place of articulation.
		/// </summary>
		public readonly ArticulationPlace ArticulationPlace;

		/// <summary>
		/// The type of articulation.
		/// </summary>
		public readonly ArticulationManner ArticulationType;

		public Phoneme(float height, float backness, float roundedness, bool voiced = true,
			ArticulationPlace artPlace = ArticulationPlace.None, ArticulationManner artType = ArticulationManner.Vowel)
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
			ArticulationPlace = ArticulationPlace.None;
			ArticulationType = ArticulationManner.Vowel;
			Height = height;
			Backness = backness;
			Roundedness = roundedness;
		}

		public Phoneme(ArticulationPlace artPlace, ArticulationManner artType, bool voiced = false)
		{
			IsVoiced = voiced;
			ArticulationPlace = artPlace;
			ArticulationType = artType;
		}
	}
}