using System;

namespace Sagen.Extensibility
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