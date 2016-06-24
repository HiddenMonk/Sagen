namespace HAARP
{
	public class Phoneme
	{
		/// <summary>
		/// Determines if the phoneme is voiced.
		/// </summary>
		public bool IsVoiced;

		/// <summary>
		/// The height (tongue height).
		/// The first formant, F1, inversely corresponds to this vocal quality.
		/// </summary>
		public float Height;

		/// <summary>
		/// The backness (position of the tongue relative to the back of the mouth).
		/// The second formant, F2, corresponds directly to this vocal quality.
		/// </summary>
		public float Backness;

		/// <summary>
		/// The roundedness (of the lips).
		/// The third formant, F3, typically represents roundedness.
		/// </summary>
		public float Roundedness;

		/// <summary>
		/// The place of articulation.
		/// </summary>
		public ArticulationPlace ArticulationPlace;

		/// <summary>
		/// The type of articulation.
		/// </summary>
		public ArticulationType ArticulationType;
	}
}