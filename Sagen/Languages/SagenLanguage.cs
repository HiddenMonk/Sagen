using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using Sagen.Pronunciation;

namespace Sagen.Languages
{
	public abstract class SagenLanguage
	{
		protected readonly string _languageCode;
		private readonly SagenDictionary _lexicon;

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
				_lexicon = SagenDictionary.FromStream(stream);
			}
		}

		protected abstract void ReadUnknownWord(string word, List<VoiceKey> keys);

		public IEnumerable<VoiceKey> Process(string phrase)
		{
			throw new NotImplementedException();
		}  
	}
}