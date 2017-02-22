using System;
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
				throw new InvalidOperationException($"Missing {nameof(LanguageCodeAttribute)} on extension {GetType()}");
			_languageCode = attr.LanguageCode;

			var dicFileName = $"{_languageCode}.dic";
			using (var stream = Assembly.GetAssembly(GetType()).GetManifestResourceStream(GetType(), dicFileName))
			{
				if (stream == null)
					throw new FileNotFoundException($"Could not find lexicon ({dicFileName}) in assembly {Assembly.GetAssembly(GetType()).FullName}.");
				_lexicon = SagenLexicon.FromStream(stream);
			}
		}

		protected abstract void ReadUnknownWord(string word, ISpeechTimeline writer);

		public void Parse(string phrase, ISpeechTimeline timeline)
		{
			timeline.AddPhonation(0.1);
			timeline.AddSilence(0.2);
			timeline.AddPhonation(0.1);
			timeline.AddSilence(0.2);
			timeline.AddPhonation(0.1);
			timeline.AddSilence(0.2);
			timeline.AddPhonation(0.14);
			timeline.AddSilence(0.2);
			timeline.AddPhonation(0.4);
			timeline.AddSilence(0.2);
		}  
	}
}