#region License

// https://github.com/TheBerkin/Sagen
// 
// Copyright (c) 2017 Nicholas Fleck
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in the
// Software without restriction, including without limitation the rights to use, copy,
// modify, merge, publish, distribute, sublicense, and/or sell copies of the Software,
// and to permit persons to whom the Software is furnished to do so, subject to the
// following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A
// PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE
// OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

using Sagen.Phonetics;

namespace Sagen.Extensibility
{
    public abstract class SagenLanguage
    {
        protected readonly string _languageCode;
        private readonly Lexicon _lexicon;
        private readonly List<KeyValuePair<string, string>> _ruleSet;

        protected SagenLanguage()
        {
            var attr = GetType().GetCustomAttribute<LanguageCodeAttribute>();
            if (attr == null)
                throw new InvalidOperationException($"Missing {nameof(LanguageCodeAttribute)} on extension {GetType()}");
            _languageCode = attr.LanguageCode;

            string dicFileName = $"{_languageCode}.dic";
            string rulesFileName = $"{_languageCode}.rules";
            using (var stream = Assembly.GetAssembly(GetType()).GetManifestResourceStream(GetType(), dicFileName))
            {
                if (stream == null)
                    throw new FileNotFoundException($"Could not find lexicon ({dicFileName}) in assembly {Assembly.GetAssembly(GetType()).FullName}.");
                _lexicon = Lexicon.FromStream(stream);
            }

            _ruleSet = new List<KeyValuePair<string, string>>();
            using (var stream = Assembly.GetAssembly(GetType()).GetManifestResourceStream(GetType(), rulesFileName))
            {
                if (stream == null)
                    throw new FileNotFoundException($"Could not find ruleset ({rulesFileName}) in assembly {Assembly.GetAssembly(GetType()).FullName}.");
                using (var reader = new StreamReader(stream, Encoding.UTF8, true, 128, true))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        if (String.IsNullOrWhiteSpace(line)) continue;
                        var entry = line.Split(new[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);
                        _ruleSet.Add(new KeyValuePair<string, string>(entry[0], entry[1].Trim('/')));
                    }
                    _ruleSet.Sort((x, y) => y.Key.Length.CompareTo(x.Key.Length));
                }
            }
        }

        public string LanguageCode => _languageCode;

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
                            if ((p = Phoneme.GetPreset($"{phrase[i]}\\")) != null && p.ArticulationPlace == ConsonantPlace.None)
                                timeline.AddPhoneme(0.4, p.Height, p.Backness, p.Roundedness);
                            i++;
                        }
                        else
                        {
                            if ((p = Phoneme.GetPreset(phrase[i].ToString())) != null && p.ArticulationPlace == ConsonantPlace.None)
                                timeline.AddPhoneme(0.4, p.Height, p.Backness, p.Roundedness);
                        }

                        break;
                }
            }
            timeline.AddSilence(0.2);
        }

        public string ToPhonemes(string text)
        {
            var sb = new StringBuilder();
            var words = text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var word in words)
            {
                if (sb.Length > 0) sb.Append(' ');

                int i = 0;
                while (i < word.Length)
                {
                    bool used = false;
                    foreach (var rule in _ruleSet)
                    {
                        if (word.IndexOf(rule.Key, i, StringComparison.InvariantCultureIgnoreCase) == i)
                        {
                            sb.Append(rule.Value);
                            i += rule.Key.Length;
                            used = true;
                            break;
                        }
                    }
                    if (!used) i++;
                }
            }

            return sb.ToString();
        }
    }
}