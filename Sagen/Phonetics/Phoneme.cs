namespace Sagen.Phonetics
{
	internal partial class Phoneme
	{
		/// <summary>
		/// Determines if the phoneme is voiced.
		/// </summary>
		public readonly bool IsVoiced;

		/// <summary>
		/// The height (tongue height).
		/// The first formant, F1, inversely correlates with this vocal quality.
		/// </summary>
		public readonly float Height;

		/// <summary>
		/// The backness (position of the tongue relative to the back of the mouth).
		/// The second formant, F2, correlates directly with this vocal quality.
		/// </summary>
		public readonly float Backness;

		/// <summary>
		/// The roundedness (of the lips).
		/// The second formant is typically affected by roundedness.
		/// </summary>
		public readonly float Roundedness;

		/// <summary>
		/// The place of articulation.
		/// </summary>
		public readonly ConsonantPlace ArticulationPlace;

		/// <summary>
		/// The type of articulation.
		/// </summary>
		public readonly ConsonantManner ArticulationType;

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