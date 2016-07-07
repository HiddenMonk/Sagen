using System;

namespace Sagen.Pronunciation
{
	public sealed class SagenLanguageAttribute : Attribute
	{
		public SagenLanguageAttribute(string languageCode)
		{
			LanguageCode = languageCode;
		}

		public string LanguageCode { get; }
		public string LexiconPath { get; set; }
	}
}