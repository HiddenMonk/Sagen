namespace Sagen
{
	/// <summary>
	/// Configures the phonetic characteristics of a voice.
	/// </summary>
	public partial class Voice
	{
		public double SentenceBetweenTime { get; set; }
		public double EllipsisPause { get; set; }
		public double ClausePauseTime { get; set; }
		public bool VerbalizeNumbers { get; set; } = true;

		/// <summary>
		/// Sets the gender of the voice, which affects the frequency range for each vocal register.
		/// </summary>
		public VoiceGender Gender { get; set; } = VoiceGender.Male;

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
		/// Head size represents the vocal tract length of the speaker, which will shift the formants up or down.
		/// Values greater than 1.0 cause the voice to sound more "throaty". Values less than 1.0 are ideal for more feminine/childlike voices.
		/// <para>The recommended maximum value for this parameter is 2.0, as higher values may cause speech to become incomprehensible.</para>
		/// <para>Range = (0, Infinity), Default = 1.0</para>
		/// </summary>
		public double HeadSize { get; set; } = 1.0;

		/// <summary>
		/// How fast F0 ascends when shaking
		/// <para>Default: 12.0</para>
		/// </summary>
		public double VoiceShakeAscendRate { get; set; } = 12.0;

		/// <summary>
		/// How fast F0 descends when shaking
		/// <para>Default: 5.0</para>
		/// </summary>
		public double VoiceShakeDescendRate { get; set; } = 5.0;

		/// <summary>
		/// The maximum offset, in hertz, that F0 can ascend or descend when shaking.
		/// </summary>
		public double VoiceShakeAmountHz { get; set; } = 3.0;

		/// <summary>
		/// Master multiplier of fundamental frequency.
		/// <para>Default: 1.0</para>
		/// </summary>
		public double FundamentalFrequencyMultiplier { get; set; } = 1.0;

		/// <summary>
		/// Vibrato amount in octaves.
		/// <para>Range: [0, Infinity)</para>
		/// <para>Default: 0.0</para>
		/// </summary>
		public double VibratoAmount { get; set; } = 0.0;

		/// <summary>
		/// Vibrato speed in hertz.
		/// <para>Default: 8.0</para>
		/// </summary>
		public double VibratoSpeed { get; set; } = 8.0;

		/// <summary>
		/// Master nasalization level.
		/// <para>Range: [0, 1]</para>
		/// <para>Default: 0.0</para>
		/// </summary>
		public double Nasalization { get; set; } = 0.0;

		/// <summary>
		/// Breathiness level.
		/// <para>Range: [0, 1]</para>
		/// <para>Default: 0.0</para>
		/// </summary>
		public double Breathiness { get; set; } = 0.0;

		/// <summary>
		/// Quantization quality. Set to 0 to disable.
		/// <para>Range: [0, Infinity)</para>
		/// </summary>
		public int Quantization { get; set; } = 0;

		/// <summary>
		/// The frequency offset of F4 in Hertz. This value is inversely scaled by head size.
		/// </summary>
		public double FrequencyOffsetF4 { get; set; } = 0.0;

		/// <summary>
		/// The frequency offset of F5 in Hertz. This value is inversely scaled by head size.
		/// </summary>
		public double FrequencyOffsetF5 { get; set; } = 0.0;

		/// <summary>
		/// The bandwidth of the fourth formant in Hertz.
		/// </summary>
		public double BandwidthF4 { get; set; } = 280.0;

		/// <summary>
		/// The bandwidth of the fifth formant in Hertz.
		/// </summary>
		public double BandwidthF5 { get; set; } = 300.0;

		public double FormantGain { get; set; } = 1.0;
	}

	public enum VoiceGender
	{
		Male,
		Female
	}
}
