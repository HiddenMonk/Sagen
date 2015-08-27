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

		public static void Play(MemoryStream SoundStream, bool Hang = true)
		{
			SoundStream.Seek(0, SeekOrigin.Begin);
			ISoundSource Src = Engine.AddSoundSourceFromIOStream(SoundStream, "snd");
			Console.WriteLine("Play length: {0} s", (float)Src.PlayLength / 1000);

			Engine.Play2D(Src, false, false, false);
			while (Hang && Engine.IsCurrentlyPlaying("snd"))
				;
			Engine.RemoveSoundSource("snd");
		}

		public static MemoryStream Create(int SampleRate, double Freq, double Magnitude, uint LenMS, Func<double, double> Func)
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

		static void Main(string[] args)
		{

			MemoryStream Snd = Create(44100, 120, 1.0, 1000, (Cur) =>
			{
				return Math.Cos(Cur);
			});

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