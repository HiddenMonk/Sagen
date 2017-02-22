using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using Sagen.Core;
using Sagen.Core.Layers;
using Sagen.Core.Nodes;
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

		static TTS()
		{
			#region Plugin loading
			foreach (var path in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "Sagen.*.dll"))
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

		private Voice _voice;

		public TTS()
		{
			if (!_languages.TryGetValue(DefaultLanguage, out _lang))
			{
				throw new FileNotFoundException($"Default language '{DefaultLanguage}' could not be loaded.");
			}
			_voice = Voice.Jimmy;
		}

		public TTS(Voice voice)
		{
			if (!_languages.TryGetValue(DefaultLanguage, out _lang))
			{
				throw new FileNotFoundException($"Default language '{DefaultLanguage}' could not be loaded.");
			}
			_voice = voice ?? Voice.Jimmy;
		}

		public TTS(Voice voice, string language)
		{
			if (!_languages.TryGetValue(language, out _lang))
			{
				throw new FileNotFoundException($"Language '{language}' could not be loaded.");
			}
			_voice = voice ?? Voice.Jimmy;
		}

		public string GetLanguageCode() => _lang?.LanguageCode;

		public void SetLanguage(string languageCode)
		{
			if (!_languages.TryGetValue(languageCode, out _lang))
			{
				throw new FileNotFoundException($"Language '{languageCode}' could not be loaded.");
			}
		}

		public IEnumerable<string> GetAvailableLanguageCodes()
		{
			foreach (var pair in _languages) yield return pair.Key;
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

		public void SpeakToFile(string path, string text)
		{
			var synth = CreateSynth(text);
			synth.Generate(File.Create(path), SampleFormat.Float32);
		}

		internal Synthesizer CreateSynth(string text)
		{
			var timeline = new SpeechTimeline();
			_lang.Parse(text, timeline);
			var synth = new Synthesizer(this, timeline);

			const float amp = .5f;

			synth.AddSampler(new PitchLayer(synth));
			synth.AddSampler(new PhonationLayer(synth, 20, amp, 0.003));
			synth.AddSampler(new ArticulatorLayer(synth));

			return synth;
		}
	}
}