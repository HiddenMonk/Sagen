namespace Sagen
{
	/// <summary>
	/// Configures the phonetic characteristics of a voice.
	/// </summary>
	public partial class VoiceParams
	{
		public double SentenceBetweenTime { get; set; }
		public double EllipsisPause { get; set; }
		public double ClausePauseTime { get; set; }
		public bool VerbalizeNumbers { get; set; } = true;

		public VoiceGender Gender { get; set; } = VoiceGender.Male;

		public double Pitch { get; set; } = 1.0;

		/// <summary>
		/// Controls the voicing strength. Values below one create a whispering effect.
		/// </summary>
		public double VoicingLevel { get; set; } = 1.0;

		/// <summary>
		/// Controls the strength of glottal consonants.
		/// </summary>
		public double GlottalForce { get; set; } = 1.0;

		/// <summary>
		/// Controls the overall strength of fricatives.
		/// </summary>
		public double FricativeForce { get; set; } = 1.0;

		/// <summary>
		/// Controls the strength of dental fricative bands. Set to zero for toothless mode.
		/// </summary>
		public double DentalForce { get; set; } = 1.0;

		/// <summary>
		/// Controls the strength of plosives. Set to zero for lipless mode. Set to above one for spitting animal.
		/// </summary>
		public double PlosiveForce { get; set; } = 1.0;

		/// <summary>
		/// Controls the strength of bands in fricatives, which give them their characteristic sounds.
		/// Values below one create a lisp effect. Values above one create a really annoying effect.
		/// </summary>
		public double FricativeBandStrength { get; set; } = 1.0;

		/// <summary>
		/// The spectral tilt of the glottal pulse train (vibrations of the vocal folds).
		/// Values below one will diminish higher harmonics. Values above one will amplify them.
		/// </summary>
		public double SpectralTilt { get; set; } = 0.8;

        /// <summary>
        /// Head size represents the vocal tract length of the speaker, which affects the offset of the formant frequencies.
        /// Higher values cause the voice to sound more "throaty". Lower values are ideal for more feminine voices.
        /// </summary>
        public double HeadSize { get; set; } = 1.0;

        public double VoiceShakeAscendRate { get; set; } = 12.0;

        public double VoiceShakeDescendRate { get; set; } = 6.0;
	}

	public enum VoiceGender
	{
		Male,
		Female,
		Unknown
	}
}
