namespace Sagen
{
    public partial class Voice
    {
        /// <summary>
        /// Jimmy is a young man, no older than 21. He is quite partial to the delicate flavor of wallpaper.
        /// </summary>
        public static readonly Voice Jimmy = new Voice
        {
            Gender = VoiceGender.Male
        };

        /// <summary>
        /// Susan is a world-renowned psychic whose sole ability is being able to accurately
        /// determine an individual's affinity to garlic bread from their brain waves alone.
        /// </summary>
        public static readonly Voice Susan = new Voice
        {
            Gender = VoiceGender.Female,
            HeadSize = 0.96
        };

		/// <summary>
		/// Middle-aged woman
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
            HeadSize = 1.35
        };

        /// <summary>
        /// At the early age of ten, Cade has perfected the art of talking only through his nasal passages.
        /// </summary>
        public static readonly Voice Cade = new Voice
        {
            Gender = VoiceGender.Male,
            FundamentalFrequencyMultiplier = 2.0,
            HeadSize = 0.75
        };
    }
}