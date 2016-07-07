using Sagen.Pronunciation;
using Sagen.Samplers;

namespace Sagen
{
	public sealed class TTS
	{
		public static VoiceQuality Quality = VoiceQuality.VeryHigh;

		public void Speak(string text)
		{
			// Actual speaking isn't supported yet. This is debug code for testing vocal properties.

			var synth = new Synthesizer
			{
				Fundamental = 165
			};

			const float amp = .015f;
			const float tilt = -3.00f;

			// Generate 100 harmonics
			for (int i = 0; i < 100; i++)
				synth.AddSampler(new HarmonicSampler(synth, i, amp, .14f * i, tilt));
			synth.AddSampler(new VocalSampler(synth, Phoneme.GetPresetIPA("e")));

			synth.Play(4.5);
		}

		public static void Sync()
		{
			AudioStream.SyncAll();
		}
	}
}