using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;

using Sagen.Extensibility;
using Sagen.Internals;
using Sagen.Internals.Layers;

namespace Sagen
{
	/// <summary>
	/// Represents a single instance of the Sagen TTS Engine, exposing speech output functionality.
	/// </summary>
	public class TTS
	{
		public static VoiceQuality Quality = VoiceQuality.VeryHigh;

		private static readonly Dictionary<string, SagenLanguage> _languages = new Dictionary<string, SagenLanguage>();

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
			_voice = Voice.Jimmy;
		}

		public TTS(Voice voice)
		{
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

		public void SpeakToFile(string path, string text)
		{
			var synth = CreateSynth();

			synth.Generate(5, File.Create(path), SampleFormat.Float32, true);
		}

		internal Synthesizer CreateSynth()
		{
			var synth = new Synthesizer(this);

			RNG rng = new RNG();

			const float amp = .25f;

			synth.AddSampler(new PitchLayer(synth));
			synth.AddSampler(new PhonationLayer(synth, 20, amp, 0.003));
			synth.AddSampler(new ArticulatorLayer(synth));

			return synth;
		}
	}
}