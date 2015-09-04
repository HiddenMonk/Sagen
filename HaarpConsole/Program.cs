using System;
using System.IO;

using HAARP;
using HAARP.Generators;

using IrrKlang;

namespace HaarpConsole
{
    class Program
    {
        public static ISoundEngine Engine = new ISoundEngine(SoundOutputDriver.AutoDetect, SoundEngineOptionFlag.MultiThreaded);

        private const float VocalPower = 0.015f;
        private const float OralPower = 0.005f;

        private static readonly float Fundamental = 100;
        private static readonly float Pitch = 1.0f;
        private static readonly int[] VocalHarmonics = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };
        private static readonly int[] OralHarmonics = { 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 39, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50 };
        private static readonly float OralHarmonicSeparation = .0001f;
        private static readonly float VocalSpectralTilt = -0.5f;
        private static readonly float OralSpectralTilt = 0.05f;

        static void Main(string[] args)
        {
            const int sampleRate = 44100;
            const float seconds = 5.0f;

            var rng = new RNG();

            var vocals = new SineGenerator[VocalHarmonics.Length];
            for(int i = 0; i < vocals.Length; i++)
                vocals[i] = new SineGenerator(Fundamental * VocalHarmonics[i] * Pitch, VocalPower)
                {
                    SpectralTilt = VocalSpectralTilt
                };

            var oral = new SineGenerator[OralHarmonics.Length];
            for(int i = 0; i < oral.Length; i++)
                oral[i] = new SineGenerator(Fundamental * OralHarmonics[i], OralPower, (float)rng.NextDouble(OralHarmonicSeparation))
                {
                    SpectralTilt = OralSpectralTilt,
                    SpectralUpperBound = 10000
                };

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

        public static MemoryStream Create(float[] Samples, int SampleRate = 44100)
        {
            int SampleCount = Samples.Length;
            var Buff = new short[SampleCount];
            var Ret = new byte[Buff.Length * sizeof(short)];

            for (int i = 0; i < Buff.Length; i++)
            {
                Buff[i] = (short)(Samples[i] * short.MaxValue);
            }

            Buffer.BlockCopy(Buff, 0, Ret, 0, Ret.Length);
            return new MemoryStream(Wav.PrependHeader(Ret, SampleRate));
        }
    }
}
