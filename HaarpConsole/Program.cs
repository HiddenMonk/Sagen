using System;
using System.IO;

using HAARP;
using HAARP.Samplers;

using IrrKlang;

namespace HaarpConsole
{
	class Program
	{
		public static ISoundEngine Engine = new ISoundEngine(SoundOutputDriver.AutoDetect, SoundEngineOptionFlag.MultiThreaded);

		static void Main(string[] args)
		{
			var synth = new Synthesizer(4) { Fundamental = 155f };

			const float amp = .015f;
			const float tilt = -3.00f;

			// Generate 100 harmonics
			for (int i = 0; i < 100; i++)
				synth.AddSampler(new HarmonicSampler(synth, i, amp, .14f * i, tilt));
			synth.AddSampler(new VocalSampler(synth, 0));

			Console.WriteLine("Generating...");

			var sound = Create(synth.Generate(), synth.SampleRate);

			// Write sound to file
			using (var file = File.Create("sample.wav"))
			{
				sound.CopyTo(file);
				file.Flush();
			}

			Play(sound);

			Console.WriteLine("Done!");
			Console.ReadLine();
		}

		private class StopEventReceiver : ISoundStopEventReceiver
		{
			public Action<ISound, StopEventCause> StopAction;

			public StopEventReceiver(Action<ISound, StopEventCause> action)
			{
				StopAction = action;
			}

			public void OnSoundStopped(ISound sound, StopEventCause reason, object userData)
			{

				StopAction?.Invoke(sound, reason);
			}
		}

		public static void Play(MemoryStream SoundStream)
		{
			SoundStream.Seek(0, SeekOrigin.Begin);
			var src = Engine.AddSoundSourceFromIOStream(SoundStream, "snd");
			Console.WriteLine("Play length: {0}s", (float)src.PlayLength / 1000);
			var snd = Engine.Play2D(src, false, false, false);
			snd.setSoundStopEventReceiver(new StopEventReceiver((sound, cause) =>
			{
				src.Dispose();
			}));
		}

		public static MemoryStream Create(float[] Samples, int SampleRate)
		{
			int SampleCount = Samples.Length;
			var samples = new short[SampleCount];
			var buffer = new byte[samples.Length * sizeof(short)];

			for (int i = 0; i < samples.Length; i++)
			{
				samples[i] = (short)(Samples[i] * short.MaxValue);
			}

			Buffer.BlockCopy(samples, 0, buffer, 0, buffer.Length);
			return new MemoryStream(Wav.PrependHeader(buffer, SampleRate));
		}
	}
}
