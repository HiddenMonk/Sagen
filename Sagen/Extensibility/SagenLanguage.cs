using System;
using System.IO;
using System.Reflection;

using Sagen.Phonetics;

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
			Phoneme p;
			for (int i = 0; i < phrase.Length; i++)
			{
				switch (phrase[i])
				{
					case ' ':
						timeline.AddSilence(0.2);
					break;
					default:
						if (i < phrase.Length - 1 && phrase[i + 1] == '\\')
						{
							if ((p = Phoneme.GetPreset($"{phrase[i]}\\")) != null)
								timeline.AddPhonation(0.4, p.Height, p.Backness, p.Roundedness);
							i++;
						}
						else
						{
							if ((p = Phoneme.GetPreset(phrase[i].ToString())) != null)
								timeline.AddPhonation(0.4, p.Height, p.Backness, p.Roundedness);
						}

					break;
				}
			}
			timeline.AddSilence(0.2);
		}
	}
}