using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HAARP;
using HAARP.Generators;

using IrrKlang;

namespace HaarpConsole
{
    class Program
    {
        public static ISoundEngine Engine = new ISoundEngine(SoundOutputDriver.AutoDetect, SoundEngineOptionFlag.MultiThreaded);

        private const float VocalPower = 0.025f;
        private const float OralPower = 0.025f;

        private static readonly float Fundamental = 100;
        private static readonly int[] VocalHarmonics = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };
        private static readonly int[] OralHarmonics = { 26, 27, 36, 37 };
        private static readonly float VocalHarmonicSeparation = .1f;
        private static readonly float OralHarmonicSeparation = .1f;

        static void Main(string[] args)
        {
            const int sampleRate = 44100;
            const float seconds = 5.0f;

            var rng = new RNG();

            var vocals = new SineGenerator[VocalHarmonics.Length];
            for(int i = 0; i < vocals.Length; i++)
                vocals[i] = new SineGenerator(Fundamental * VocalHarmonics[i], VocalPower, (float)rng.NextDouble(VocalHarmonicSeparation));

            var oral = new SineGenerator[OralHarmonics.Length];
            for(int i = 0; i < oral.Length; i++)
                oral[i] = new SineGenerator(Fundamental * OralHarmonics[i], OralPower, (float)rng.NextDouble(OralHarmonicSeparation));

            var Samples = new float[(int)(sampleRate * seconds)];
            for (int i = 0; i < Samples.Length; i++)
            {
                foreach (var gen in vocals)
                    gen.SampleAdd(Samples, i, sampleRate);
                foreach (var gen in oral)
                    gen.SampleAdd(Samples, i, sampleRate);
            }

            var Snd = Create(Samples);
            var FS = File.Create("test.wav");

            Snd.CopyTo(FS);
            FS.Flush();
            FS.Close();
            Play(Snd);

            Console.WriteLine("Done!");
            Console.ReadLine();
        }

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
    }
}
