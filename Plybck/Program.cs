using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using IrrKlang;

namespace Plybck
{
	public static class PlybckSound
	{
		public static ISoundEngine Engine = new ISoundEngine(SoundOutputDriver.AutoDetect, SoundEngineOptionFlag.MultiThreaded);

		public static void Play(MemoryStream SoundStream)
		{
			SoundStream.Seek(0, SeekOrigin.Begin);
			ISoundSource Src = Engine.AddSoundSourceFromIOStream(SoundStream, "snd");
			Console.WriteLine("Play length: {0} s", (float)Src.PlayLength / 1000);

			Engine.Play2D(Src, false, false, false);
			while (Engine.IsCurrentlyPlaying("snd"))
				;
			Engine.RemoveSoundSource("snd");
			Src.Dispose();
		}

		public static MemoryStream Create(Func<double, double> Func, uint LenMS,
			int SampleRate = 44100, double Freq = 120, double Magnitude = 1)
		{
			int SampleCount = (int)((double)SampleRate * ((double)LenMS / 1000.0f));
			short[] Buff = new short[SampleCount];
			byte[] Ret = new byte[Buff.Length * sizeof(short)];
			double Step = Math.PI * 2.0d / Freq;
			double Current = 0;

			for (int i = 0; i < Buff.Length; i++)
			{
				Buff[i] = (short)(Func(Current) * Magnitude * short.MaxValue);
				Current += Step;
			}

			Buffer.BlockCopy(Buff, 0, Ret, 0, Ret.Length);
			return new MemoryStream(Wav.PrependHeader(Ret, SampleRate));
		}

		public static MemoryStream Create(float[] Samples,
			int SampleRate = 44100, double Freq = 120, double Magnitude = 1)
		{
			int SampleCount = Samples.Length;
			short[] Buff = new short[SampleCount];
			byte[] Ret = new byte[Buff.Length * sizeof(short)];
			double Step = Math.PI * 2.0d / Freq;
			double Current = 0;

			for (int i = 0; i < Buff.Length; i++)
			{
				Buff[i] = (short)(Samples[i] * Magnitude * short.MaxValue);
				Current += Step;
			}

			Buffer.BlockCopy(Buff, 0, Ret, 0, Ret.Length);
			return new MemoryStream(Wav.PrependHeader(Ret, SampleRate));
		}

		static void Main(string[] args)
		{

			float[] Samples = new float[(int)(44100 * 0.8)];
			for (int i = 0; i < Samples.Length; i++)
				Samples[i] = (float)Math.Sin((float)i / 10);
			MemoryStream Snd = Create(Samples);

			FileStream FS = File.Create("test.wav");
			Snd.CopyTo(FS);
			FS.Flush();
			FS.Close();
			Play(Snd);

			Console.WriteLine("Done!");
			Console.ReadLine();
		}
	}
}