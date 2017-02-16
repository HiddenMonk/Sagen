using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;

using Sagen.Extensibility;
using Sagen.Internals;
using Sagen.Internals.Layers;
using Sagen.Phonetics;

namespace Sagen
{
	/// <summary>
	/// Represents a single instance of the Sagen TTS Engine, exposing speech output functionality.
	/// </summary>
	public sealed class TTS
	{
		public static VoiceQuality Quality = VoiceQuality.VeryHigh;

		private static readonly Dictionary<string, SagenLanguage> _languages = new Dictionary<string, SagenLanguage>();

		private readonly HashSet<AudioStream> _activeStreams = new HashSet<AudioStream>();

		static TTS()
		{
			foreach (var path in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "Sagen.*.dll"))
			{
				try
				{
					var asm = Assembly.LoadFile(path);
					var attrPluginClass = asm.GetCustomAttribute<SagenPluginClassAttribute>();
					if (attrPluginClass == null)
					{
						Console.WriteLine($"(Sagen) Plugin {path} does not have a [SagenPluginClass] attribute defined on the assembly. Skipping.");
						continue;
					}

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
					System.Console.WriteLine($"(Sagen) Loaded plugin: {attrPluginClass.PluginClassType}");
#endif
				}
				catch (Exception ex)
				{
					Console.WriteLine($"(Sagen) An exception of type {ex.GetType().Name} was thrown while loading plugin {Path.GetFileName(path)}: {ex.Message}");
				}
			}
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

		internal void AddActiveAudio(AudioStream audio) => _activeStreams.Add(audio);
		internal void RemoveActiveAudio(AudioStream audio)
		{
			_activeStreams.Remove(audio);
		}

		public void Speak(string text)
		{
			// Actual speaking isn't supported yet. This is debug code for testing vocal properties.

			var synth = CreateSynth();

			synth.CreateAudioStream();

			ThreadPool.QueueUserWorkItem(PlaySynthFunc, synth);
		}

		public void SpeakToFile(string path, string text)
		{
			var synth = CreateSynth();

			synth.Generate(5, File.Create(path), SampleFormat.Float32, true);
		}

		private Synthesizer CreateSynth()
		{
			var synth = new Synthesizer(this);

			RNG rng = new RNG();

			const float amp = .25f;

			synth.AddSampler(new PitchLayer(synth));			
			synth.AddSampler(new PhonationLayer(synth, 20, amp, 0.003));			
			synth.AddSampler(new ArticulatorLayer(synth));

			return synth;
		}

		private void PlaySynthFunc(object synthObj)
		{
			var synth = synthObj as Synthesizer;
			if (synth == null) return;
			synth.Play(5);
		}

		public void Sync()
		{
			foreach (var audio in _activeStreams)
			{
				audio.WaitToFinish();
			}
		}
	}
}