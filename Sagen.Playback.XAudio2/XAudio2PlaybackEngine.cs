#region License

// https://github.com/TheBerkin/Sagen
// 
// Copyright (c) 2017 Nicholas Fleck
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in the
// Software without restriction, including without limitation the rights to use, copy,
// modify, merge, publish, distribute, sublicense, and/or sell copies of the Software,
// and to permit persons to whom the Software is furnished to do so, subject to the
// following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A
// PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE
// OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

#endregion

using System;
using System.IO;
using System.Text;
using System.Threading;

using Sagen.Extensibility;

using SlimDX.Multimedia;
using SlimDX.XAudio2;

using AudioDevice = SlimDX.XAudio2.XAudio2;

namespace Sagen.Playback.XAudio2
{
    /// <summary>
    /// Represents a speech playback engine that implements the XAudio2 API.
    /// </summary>
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

        public void MarkComplete(RemoveActivePlaybackEngineCallback callback)
        {
            while (source == null || source.State.BuffersQueued > 0)
                Thread.Sleep(10);

            source.Dispose();
            callback();
            GC.KeepAlive(this);
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
            using (var ms = new MemoryStream(length * BYTES_PER_SAMPLE))
            {
                using (var writer = new BinaryWriter(ms, Encoding.Default, true))
                {
                    for (int i = 0; i < length; i++) writer.Write(buffer[i]);
                    writer.Flush();
                }
                ms.Position = 0;

                // Queue the buffer
                source.SubmitSourceBuffer(new AudioBuffer { AudioData = ms, AudioBytes = length * BYTES_PER_SAMPLE });
            }

            // Make sure it's playing
            source.Start();
        }
    }
}