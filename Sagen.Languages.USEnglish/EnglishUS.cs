using System.Collections.Generic;

using Sagen.Extensibility;

namespace Sagen.Languages
{
	[LanguageCode("en_US")]
	public sealed class EnglishUS : SagenLanguage
	{
		public EnglishUS()
		{

		}

		protected override void ReadUnknownWord(string word, ISpeechTimeline writer)
		{

		}
	}
}
