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
            var synth = new Synthesizer(3);
            // Background
            synth.AddSampler(new NoiseSampler(0, 200, 1200, 1.0f) { Amplitude = 0.075f });
            synth.AddSampler(new NoiseSampler(0, 1900, 5500, .35f) { Amplitude = 0.01f });

            // Bands
            synth.AddSampler(new NoiseSampler(0, 6700, 7200, .1f) { Amplitude = 0.003f });
            synth.AddSampler(new NoiseSampler(0, 4600, 5400, .18f) { Amplitude = 0.050f });
            synth.AddSampler(new NoiseSampler(0, 3700, 4100, .2f) { Amplitude = 0.050f });
            synth.AddSampler(new NoiseSampler(0, 2100, 2500, .2f) { Amplitude = 0.1f });

            synth.AddSampler(new FadeSampler { FadeType = FadeType.FadeIn, FadeLength = 0.25f });
            synth.AddSampler(new FadeSampler { FadeType = FadeType.FadeOut, FadeLength = 0.2f });

            var sound = Create(synth.Generate(), synth.SampleRate);

            // Write sound to file
            using (var file = File.Create("test.wav"))
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
