namespace HAARP
{
	/// <summary>
	/// Configures the phonetic characteristics of a TTS voice.
	/// </summary>
	public partial class VoiceParams
	{
		public float SentenceBetweenTime { get; set; }
		public float EllipsisPause { get; set; }
		public float ClausePauseTime { get; set; }
		public bool VerbalizeNumbers { get; set; } = true;

		public VoiceGender Gender { get; set; } = VoiceGender.Male;

		public float Pitch { get; set; } = 1.0f;

		/// <summary>
		/// Controls the voicing strength. Values below one create a whispering effect.
		/// </summary>
		public float VoicingLevel { get; set; } = 1.0f;

		/// <summary>
		/// Controls the strength of glottal consonants.
		/// </summary>
		public float GlottalForce { get; set; } = 1.0f;

		/// <summary>
		/// Controls the overall strength of fricatives.
		/// </summary>
		public float FricativeForce { get; set; } = 1.0f;

		/// <summary>
		/// Controls the strength of dental fricative bands. Set to zero for toothless mode.
		/// </summary>
		public float DentalForce { get; set; } = 1.0f;

		/// <summary>
		/// Controls the strength of plosives. Set to zero for lipless mode. Higher values sound like a spitting animal.
		/// </summary>
		public float PlosiveForce { get; set; } = 1.0f;

		/// <summary>
		/// Controls the strength of bands in fricatives, which give them their characteristic sounds.
		/// Values below one create a lisp effect. Values above one create a really annoying effect.
		/// </summary>
		public float FricativeBandStrength { get; set; } = 1.0f;

		/// <summary>
		/// The spectral tilt of the glottal pulse train (vibrations of the vocal folds).
		/// Values below one will diminish higher harmonics. Values above one will amplify them.
		/// </summary>
		public float SpectralTilt { get; set; } = 0.8f;
	}

	public enum VoiceGender
	{
		Male,
		Female,
		Unknown
	}
}
