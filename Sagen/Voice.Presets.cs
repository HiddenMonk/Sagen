namespace Sagen
{
	public partial class Voice
	{
		/// <summary>
		/// Jimmy is a young man of 21. He is partial to the delicate flavor of wallpaper.
		/// </summary>
		public static readonly Voice Jimmy = new Voice
		{
			Gender = VoiceGender.Male,
			HeadSize = 1.03
		};

		/// <summary>
		/// Susan is a world-renowned psychic whose sole ability is accurately detecting 
		/// an individual's affinity to garlic bread from their brain waves alone.
		/// </summary>
		public static readonly Voice Susan = new Voice
		{
			Gender = VoiceGender.Female,
			HeadSize = 0.93,
			Breathiness = 0.5
		};

		/// <summary>
		/// Now happily retired, Rosa spends her time singing along to her personal orchestra of vacuum cleaners.
		/// </summary>
		public static readonly Voice Rosa = new Voice
		{
			Gender = VoiceGender.Female,
			HeadSize = 1.02,
			VoiceShakeAmountHz = 5.0,
			VoiceShakeAscendRate = 2.0,
			VoiceShakeDescendRate = 10.0,
			VibratoAmount = .03,
			VibratoSpeed = 10,
			FundamentalFrequencyMultiplier = 2.0
		};

		/// <summary>
		/// Jane is a doctor fresh out of grad school who specializes in the treatment of Rotary Saw Teeth Disorder.
		/// </summary>
		public static readonly Voice Jane = new Voice
		{
			Gender = VoiceGender.Female,
			HeadSize = 0.90
		};

		/// <summary>
		/// Moist Peter's body sweats pure butter, coincidentally matching his buttery voice.
		/// </summary>
		public static readonly Voice MoistPeter = new Voice
		{
			Gender = VoiceGender.Male,
			HeadSize = 1.3,
			FundamentalFrequencyMultiplier = .5,
			VoiceShakeAmountHz = 1.0
		};

		/// <summary>
		/// A humble, throaty man, Bob has witnessed several innocent zucchinis perish during his plumbing career.
		/// </summary>
		public static readonly Voice Bob = new Voice
		{
			Gender = VoiceGender.Male,
			FundamentalFrequencyMultiplier = 1.2,
			HeadSize = 1.35,
            Breathiness = 0.35
		};

		/// <summary>
		/// At the early age of six, Cade perfected the art of talking only through his nasal passages
		/// with perfect articulation, making him the world's most accomplished ventriloquist.
		/// </summary>
		public static readonly Voice Cade = new Voice
		{
			Gender = VoiceGender.Male,
			FundamentalFrequencyMultiplier = 2.0,
			HeadSize = 0.75,
			Nasalization = 0.65
		};
	}
}