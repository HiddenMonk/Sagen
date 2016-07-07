using System;

namespace Sagen.Languages
{
	public sealed class LanguageCodeAttribute : Attribute
	{
		public LanguageCodeAttribute(string languageCode)
		{
			LanguageCode = languageCode;
		}

		public string LanguageCode { get; }
	}
}