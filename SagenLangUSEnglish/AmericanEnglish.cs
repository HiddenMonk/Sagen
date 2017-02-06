using System.Collections.Generic;

using Sagen.Extensibility;
using Sagen.Phonetics;

namespace Sagen.Languages
{
	[LanguageCode("en_US")]
    public sealed class AmericanEnglish : SagenLanguage
    {
			protected override void ReadUnknownWord(string word, PhonemeWriter writer)
			{
				
			}
    }
}
