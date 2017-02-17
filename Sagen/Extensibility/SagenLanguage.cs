﻿using System;
using System.IO;
using System.Reflection;

namespace Sagen.Extensibility
{
    public abstract class SagenLanguage
	{
		protected readonly string _languageCode;
		private readonly SagenLexicon _lexicon;

		public string LanguageCode => _languageCode;

		protected SagenLanguage()
		{
			var attr = GetType().GetCustomAttribute<LanguageCodeAttribute>();
			if (attr == null)
				throw new InvalidOperationException($"Missing SagenLanguage attribute on extension {GetType()}");
			_languageCode = attr.LanguageCode;

			var dicFileName = $"{_languageCode}.dic";
			using (var stream = Assembly.GetAssembly(GetType()).GetManifestResourceStream(GetType(), dicFileName))
			{
				if (stream == null)
					throw new FileNotFoundException($"Could not find dictionary ({dicFileName}) in assembly {Assembly.GetAssembly(GetType()).FullName}.");
				_lexicon = SagenLexicon.FromStream(stream);
			}
		}

		protected abstract void ReadUnknownWord(string word, PhonemeWriter writer);

		public void Process(string phrase)
		{
			throw new NotImplementedException();
		}  
	}
}