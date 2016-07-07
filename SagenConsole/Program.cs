using System;
using Sagen;
using Sagen.Pronunciation;
using Sagen.Samplers;

namespace HaarpConsole
{
	class Program
	{
		static void Main(string[] args)
		{
			var synth = new Synthesizer { Fundamental = 155f };

			const float amp = .015f;
			const float tilt = -3.00f;

			// Generate 100 harmonics
			for (int i = 0; i < 100; i++)
				synth.AddSampler(new HarmonicSampler(synth, i, amp, .14f * i, tilt));
			synth.AddSampler(new VocalSampler(synth, Phoneme.GetPresetIPA("e")));
			
			synth.Play(4.0f);

			Console.ReadLine();
		}
	}
}
