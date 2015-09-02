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
        private const float ArticulatePower = 0.025f;

        private static readonly SineGenerator[] generators =
        {
            new SineGenerator(100, VocalPower * 2f, .2f),
            new SineGenerator(200, VocalPower * 1.5f, .25f),
            new SineGenerator(300, VocalPower, .75f),
            new SineGenerator(400, VocalPower, .9f),
            new SineGenerator(500, VocalPower, .8f),
            new SineGenerator(600, VocalPower * .7f),
            new SineGenerator(700, VocalPower * .6f),
            new SineGenerator(800, VocalPower * .5f),
            new SineGenerator(900, VocalPower * .4f),
            new SineGenerator(1000, VocalPower * .5f),
            new SineGenerator(1100, VocalPower * .4f),
            new SineGenerator(1200, VocalPower * .3f),
            new SineGenerator(1300, VocalPower * .2f),
            new SineGenerator(1400, VocalPower * .1f),
            
            new SineGenerator(2600, ArticulatePower, .8f),
            new SineGenerator(2700, ArticulatePower, .8f),
            new SineGenerator(3600, ArticulatePower, .8f),
            new SineGenerator(3700, ArticulatePower, .8f),
        };

        static void Main(string[] args)
        {
            const int sampleRate = 44100;
            const float seconds = 1.5f;

            var Samples = new float[(int)(sampleRate * seconds)];
            for (int i = 0; i < Samples.Length; i++)
            {
                foreach (var gen in generators)
                {
                    gen.SampleAdd(Samples, i, sampleRate);
                }
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
