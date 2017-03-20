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

using Sagen.Core;
using Sagen.Core.Layers;
using Sagen.Extensibility;

namespace Sagen
{
    /// <summary>
    /// Represents a single instance of the Sagen TTS Engine, exposing speech output functionality.
    /// </summary>
    public class TTS
    {
        private const string DefaultLanguage = "en_US";

        public static VoiceQuality Quality = VoiceQuality.VeryHigh;

        private static readonly Dictionary<string, SagenLanguage> _languages = new Dictionary<string, SagenLanguage>(StringComparer.InvariantCultureIgnoreCase);

        private SagenLanguage _lang;

        private Voice _voice;

        static TTS()
        {
            #region Plugin loading

            foreach (string path in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "Sagen.*.dll"))
            {
                try
                {
                    var asm = Assembly.LoadFile(path);
                    var attrPluginClass = asm.GetCustomAttribute<SagenPluginClassAttribute>();
                    if (attrPluginClass == null) continue;

                    var plugin = Activator.CreateInstance(attrPluginClass.PluginClassType) as SagenLanguage;

                    if (plugin == null)
                    {
                        Console.WriteLine($"(Sagen) Plugin type {attrPluginClass.PluginClassType} in {Path.GetFileName(path)} was unable to be initialized. Skipping.");
                        continue;
                    }

                    if (String.IsNullOrWhiteSpace(plugin.LanguageCode))
                    {
                        Console.WriteLine($"(Sagen) Plugin type {attrPluginClass.PluginClassType} in {Path.GetFileName(path)} does not have a valid language code defined. Skipping.");
                        continue;
                    }

                    _languages[plugin.LanguageCode] = plugin;
#if DEBUG
					System.Console.WriteLine($"(Sagen) Loaded plugin: {attrPluginClass.PluginName}");
#endif
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"(Sagen) An exception of type {ex.GetType().Name} was thrown while loading plugin {Path.GetFileName(path)}: {ex}");
                }
            }

            #endregion
        }

        public TTS()
        {
            if (!_languages.TryGetValue(DefaultLanguage, out _lang))
                throw new FileNotFoundException($"Default language '{DefaultLanguage}' could not be loaded.");
            _voice = Voice.Jimmy;
        }

        public TTS(Voice voice)
        {
            if (!_languages.TryGetValue(DefaultLanguage, out _lang))
                throw new FileNotFoundException($"Default language '{DefaultLanguage}' could not be loaded.");
            _voice = voice ?? Voice.Jimmy;
        }

        public TTS(Voice voice, string language)
        {
            if (!_languages.TryGetValue(language, out _lang))
                throw new FileNotFoundException($"Language '{language}' could not be loaded.");
            _voice = voice ?? Voice.Jimmy;
        }

        public Voice Voice
        {
            get { return _voice; }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                _voice = value;
            }
        }

        public string GetLanguageCode() => _lang?.LanguageCode;

        public void SetLanguage(string languageCode)
        {
            if (!_languages.TryGetValue(languageCode, out _lang))
                throw new FileNotFoundException($"Language '{languageCode}' could not be loaded.");
        }

        public IEnumerable<string> GetAvailableLanguageCodes()
        {
            foreach (var pair in _languages) yield return pair.Key;
        }

        public void SpeakToFile(string path, string text)
        {
            var synth = CreateSynth(text);
            using (var stream = File.Create(path))
            {
                synth.Generate(stream, SampleFormat.Float32);
            }
        }

        public string GetPhoneticString(string text)
        {
            return _lang.ToPhonemes(text);
        }

        internal Synthesizer CreateSynth(string text)
        {
            var timeline = new Timeline();
            _lang.Parse(_lang.ToPhonemes(text), timeline);
            var synth = new Synthesizer(this, timeline);

            const float amp = .4f;

            synth.AddSampler(new PitchLayer(synth));
            synth.AddSampler(new PhonationLayer(synth, 30, amp, 0.003));
            synth.AddSampler(new ArticulatorLayer(synth));

            return synth;
        }
    }
}