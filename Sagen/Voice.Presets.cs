#region License

// https://github.com/TheBerkin/Sagen
// 
// Copyright (c) 2017 Nicholas Fleck
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in the
// Software without restriction, including without limitation the rights to use, copy,
// modify, merge, publish, distribute, sublicense, and/or sell copies of the Software,
// and to permit persons to whom the Software is furnished to do so, subject to the
// following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A
// PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE
// OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

#endregion

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
            HeadSize = 1.0,
            FrequencyOffsetF4 = 410,
            FrequencyOffsetF5 = 520
        };

        /// <summary>
        /// Greg is a hardened criminal serving time for multiple charges of breaking into houses and ravenously
        /// eating the dust out of vacuum cleaners.
        /// <para>Upon being caught, he raspily exclaimed: "Another one bites the dust!"</para>
        /// </summary>
        public static readonly Voice Greg = new Voice
        {
            Gender = VoiceGender.Male,
            HeadSize = 1.11,
            VibratoAmount = 0.01,
            Breathiness = 0.85,
            VoicingGain = 0.75,
            VoiceShakeAmountHz = 2.0,
            VoiceShakeAscendRate = 100.0,
            VoiceShakeDescendRate = 100.0
        };

        /// <summary>
        /// Susan is a world-renowned psychic whose sole ability is accurately detecting
        /// an individual's affinity to garlic bread from their brain waves alone.
        /// </summary>
        public static readonly Voice Susan = new Voice
        {
            Gender = VoiceGender.Female,
            HeadSize = 0.83,
            Breathiness = 0.65
        };

        /// <summary>
        /// Now happily retired, Rosa spends her time humming along to the sweet tune of tornado sirens.
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
            FundamentalFrequencyMultiplier = 2.0,
            FormantGain = 0.5
        };

        /// <summary>
        /// Jane is a doctor fresh out of grad school who specializes in the treatment of Rotary Saw Teeth Disorder.
        /// </summary>
        public static readonly Voice Jane = new Voice
        {
            Gender = VoiceGender.Female,
            HeadSize = 0.85
        };

        /// <summary>
        /// Moist Peter's body sweats pure butter, coincidentally matching his buttery voice.
        /// </summary>
        public static readonly Voice MoistPeter = new Voice
        {
            Gender = VoiceGender.Male,
            HeadSize = 1.3,
            FundamentalFrequencyMultiplier = .5,
            VoiceShakeAmountHz = 1.0,
            Breathiness = 0.1
        };

        /// <summary>
        /// A humble, throaty man, Bob has witnessed several innocent zucchinis perish during his plumbing career.
        /// </summary>
        public static readonly Voice Bob = new Voice
        {
            Gender = VoiceGender.Male,
            FundamentalFrequencyMultiplier = 2.0,
            HeadSize = 1.3,
            Breathiness = 0.75,
            FormantGain = 0.75
        };

        /// <summary>
        /// At the early age of six, Cade perfected the art of talking only through his nasal passages
        /// with perfect articulation, making him the world's most accomplished ventriloquist.
        /// </summary>
        public static readonly Voice Cade = new Voice
        {
            Gender = VoiceGender.Child,
            HeadSize = 0.67,
            //Nasalization = 0.75,
            Breathiness = 0.14
        };
    }
}