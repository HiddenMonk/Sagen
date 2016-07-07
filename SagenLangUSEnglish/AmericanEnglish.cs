using System.Collections.Generic;

using Sagen.Pronunciation;

namespace Sagen.Languages
{
	[LanguageCode("en_US")]
    public sealed class AmericanEnglish : SagenLanguage
    {
		protected override void ReadUnknownWord(string word, List<VoiceKey> keys)
		{
			
		}
    }
}
