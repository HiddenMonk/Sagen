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
using System.IO;
using System.Reflection;

using Sagen.Phonetics;

namespace Sagen.Extensibility
{
    public abstract class SagenLanguage
    {
        protected readonly string _languageCode;
        private readonly SagenLexicon _lexicon;

        protected SagenLanguage()
        {
            var attr = GetType().GetCustomAttribute<LanguageCodeAttribute>();
            if (attr == null)
                throw new InvalidOperationException($"Missing {nameof(LanguageCodeAttribute)} on extension {GetType()}");
            _languageCode = attr.LanguageCode;

            string dicFileName = $"{_languageCode}.dic";
            using (var stream = Assembly.GetAssembly(GetType()).GetManifestResourceStream(GetType(), dicFileName))
            {
                if (stream == null)
                    throw new FileNotFoundException($"Could not find lexicon ({dicFileName}) in assembly {Assembly.GetAssembly(GetType()).FullName}.");
                _lexicon = SagenLexicon.FromStream(stream);
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
                            if ((p = Phoneme.GetPreset($"{phrase[i]}\\")) != null)
                                timeline.AddPhoneme(0.4, p.Height, p.Backness, p.Roundedness);
                            i++;
                        }
                        else
                        {
                            if ((p = Phoneme.GetPreset(phrase[i].ToString())) != null)
                                timeline.AddPhoneme(0.4, p.Height, p.Backness, p.Roundedness);
                        }

                        break;
                }
            }
            timeline.AddSilence(0.2);
        }
    }
}