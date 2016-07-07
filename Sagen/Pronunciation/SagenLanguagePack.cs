using System;
using System.IO;
using System.Reflection;

namespace Sagen.Pronunciation
{
	public abstract class SagenLanguagePack
	{
		protected readonly string _languageCode;
		private readonly Lexicon _lexicon;

		private SagenLanguagePack()
		{
			var attr = GetType().GetCustomAttribute<SagenLanguageAttribute>();
			if (attr == null)
				throw new InvalidOperationException($"Missing SagenLanguage attribute on extension {GetType()}");
			_languageCode = attr.LanguageCode;
			
		}
	}
}