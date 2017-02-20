using Sagen.Extensibility;
using System;
using System.Text;
using AudioDevice = SlimDX.XAudio2.XAudio2;
using SlimDX.XAudio2;
using SlimDX.Multimedia;
using System.IO;
using System.Threading;

namespace Sagen.Playback.XAudio2
{
    public sealed class XAudio2PlaybackEngine : IPlaybackEngine
    {
        private const short BITS_PER_SAMPLE = 16;
        private const short BYTES_PER_SAMPLE = BITS_PER_SAMPLE / 8;
        private const short NUM_CHANNELS = 1;
        private const short BLOCK_ALIGN = NUM_CHANNELS * BYTES_PER_SAMPLE;

        private static readonly AudioDevice device;
        private static readonly MasteringVoice voice;

        private SourceVoice source;

        static XAudio2PlaybackEngine()
        {
            device = new AudioDevice();
            voice = new MasteringVoice(device);
            AppDomain.CurrentDomain.ProcessExit += (o, args) =>
            {
                voice.Dispose();
                device.Dispose();
            };
        }

        public XAudio2PlaybackEngine()
        {
        }

        public void MarkComplete(RemoveActivePlaybackEngineCallback callback)
        {
            while(source == null || source.State.BuffersQueued > 0)
            {
                Thread.Sleep(10);
            }
            source.Dispose();
	        callback();
        }

        public void QueueDataBlock(short[] buffer, int length, int sampleRate)
        {
            // Initialize source if it's null
            if (source == null)
            {
	            var fmt = new WaveFormat
	            {
		            SamplesPerSecond = sampleRate,
		            BitsPerSample = BITS_PER_SAMPLE,
		            AverageBytesPerSecond = sampleRate * BYTES_PER_SAMPLE,
		            Channels = NUM_CHANNELS,
		            BlockAlignment = BLOCK_ALIGN,
					FormatTag = WaveFormatTag.Pcm
	            };
                source = new SourceVoice(device, fmt);
            }

            // Copy the samples to a stream
            using (var ms = new MemoryStream(length * 2))
            {
                using (var writer = new BinaryWriter(ms, Encoding.Default, true))
                {
                    for (int i = 0; i < length; i++)
                    {
                        writer.Write(buffer[i]);
                    }
                    writer.Flush();
                }
                ms.Position = 0;
	            ms.Seek(0, SeekOrigin.Begin);
	            
                // Queue the buffer
                source.SubmitSourceBuffer(new AudioBuffer { AudioData = ms, AudioBytes = length * 2});
            }

            // Make sure it's playing
            source.Start();
        }
    }
}
