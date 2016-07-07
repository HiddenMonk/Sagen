using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

using Sagen.Pronunciation;
using Sagen.Samplers;

namespace Sagen
{
	public sealed class TTS
	{
		public static VoiceQuality Quality = VoiceQuality.VeryHigh;

		private static Dictionary<string, SagenDictionary> _languages = new Dictionary<string, SagenDictionary>();

		private readonly HashSet<AudioStream> _activeStreams = new HashSet<AudioStream>();
		private readonly HashSet<ManualResetEventSlim> _resetEvents = new HashSet<ManualResetEventSlim>();

		static TTS()
		{
			foreach (var path in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "Sagen.*.dll"))
			{

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

			var synth = new Synthesizer(this)
			{
				Fundamental = 165
			};

			const float amp = .015f;
			const float tilt = -3.00f;

			// Generate 100 harmonics
			for (int i = 0; i < 100; i++)
				synth.AddSampler(new HarmonicSampler(synth, i, amp, .14f * i, tilt));
			synth.AddSampler(new VocalSampler(synth, Phoneme.GetPresetIPA("e")));

			synth.CreateAudioStream();

			ThreadPool.QueueUserWorkItem(PlaySynthFunc, synth);
		}

		private void PlaySynthFunc(object synthObj)
		{
			var synth = synthObj as Synthesizer;
			if (synth == null) return;
			synth.Play(4.5);
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