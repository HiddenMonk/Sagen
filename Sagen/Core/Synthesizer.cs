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

using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

using Sagen.Core.Audio;
using Sagen.Core.Layers;
using Sagen.Extensibility;

namespace Sagen.Core
{
    /// <summary>
    /// Handles state updates and audio generation for a voice synthesizer instance.
    /// </summary>
    internal class Synthesizer : ISynthesizer
    {
        public const int MaxSamplers = 128;
        public const SampleFormat PlaybackFormat = SampleFormat.Signed16;
        public const int PlaybackFormatBytes = (int)PlaybackFormat / 8;
        private const double StreamChunkDurationSeconds = 0.1;

        private readonly HashSet<Layer> samplers = new HashSet<Layer>();
        private readonly List<Layer> samplerSequence = new List<Layer>();

        public Synthesizer(TTS engine, Timeline timeline)
        {
            TimeStep = 1.0f / SampleRate;
            TTS = engine;
            Timeline = timeline;
            State = new VocalState();
        }

        /// <summary>
        /// Pitch, measured in relative octaves.
        /// </summary>
        public double Pitch { get; set; } = 0.0f;

        public VocalState State { get; }

        public Voice Voice => TTS.Voice;

        internal TTS TTS { get; }

		/// <summary>
		/// The quality level of the synthesis.
		/// </summary>
        public VoiceQuality Quality { get; } = TTS.Quality;

		/// <summary>
		/// The node timeline used to control synthesizer parameters.
		/// </summary>
        public Timeline Timeline { get; }

		/// <summary>
		/// The current sample position.
		/// </summary>
        public int Position { get; set; } = 0;

        /// <summary>
        /// Fundamental frequency calculated from gender, pitch, and intonation.
        /// </summary>
        public double Fundamental { get; set; } = 100.0f;

        /// <summary>
        /// The time, in seconds, already elapsed before the current sample
        /// </summary>
        public double TimePosition => (double)Position / SampleRate;

        /// <summary>
        /// The amount of time, in seconds, elapsed per sample
        /// </summary>
        public double TimeStep { get; }

        public int SampleRate { get; } = (int)TTS.Quality;

        public void AddSampler(Layer sampler)
        {
            if (sampler != null && samplers.Add(sampler))
                samplerSequence.Add(sampler);
        }

        public void Generate(Stream output, SampleFormat format, bool includeWavHeader = true)
        {
            unchecked
            {
                using (var writer = new BinaryWriter(output, Encoding.Default, true))
                {
                    int sampleCount = (int)(SampleRate * Timeline.LengthSeconds);

                    if (includeWavHeader)
                        Wav.GenerateWavHeader(this, output, sampleCount, format);

                    var nodeEnumerator = Timeline.GetEnumerator();
                    nodeEnumerator.MoveNext();

                    for (Position = 0; Position < sampleCount; Position++)
                    {
                        // Traverse nodes
                        if (nodeEnumerator.Current == null) break;
                        while (TimePosition >= nodeEnumerator.Current.EndTime)
                        {
                            nodeEnumerator.Current.OnExit(this);
                            State.LastGlottisLevel = State.GlottisLevel;
                            if (!nodeEnumerator.MoveNext()) goto done;
                            nodeEnumerator.Current.OnEnter(this);
                        }

                        nodeEnumerator.Current.OnUpdate(this);

                        // Run synthesizer on current sample
                        double currentSample = 0f;
                        foreach (var sampler in samplerSequence)
                        {
                            if (!sampler.Enabled) continue;
                            sampler.Update(ref currentSample);
                        }

                        // Limit signal
                        Util.Sigmoid(ref currentSample);

                        // Convert sample to desired format and write to stream
                        switch (format)
                        {
                            case SampleFormat.Float64:
                                writer.Write(currentSample);
                                break;
                            case SampleFormat.Float32:
                                writer.Write((float)currentSample);
                                break;
                            case SampleFormat.Signed16:
                                writer.Write((short)(currentSample * short.MaxValue));
                                break;
                            case SampleFormat.Unsigned8:
                                writer.Write((byte)((currentSample + 1.0f) / 2.0f * byte.MaxValue));
                                break;
                        }
                    }

                    done:

                    writer.Flush();
                    nodeEnumerator.Dispose();
                }
            }
        }

        public void Play<TPlaybackEngine>(TTS<TPlaybackEngine> ttsPlayback) where TPlaybackEngine : IPlaybackEngine, new()
        {
            var playback = new TPlaybackEngine();
            ttsPlayback.AddActiveAudio(playback);
            ThreadPool.QueueUserWorkItem(o =>
            {
                int blockSize = (int)(SampleRate * StreamChunkDurationSeconds) * PlaybackFormatBytes;
                var data = new short[blockSize];
                int len = 0;

                var nodeEnumerator = Timeline.GetEnumerator();
                nodeEnumerator.MoveNext();

                while (nodeEnumerator.Current != null)
                {
                    // Traverse nodes
                    while (TimePosition >= nodeEnumerator.Current.EndTime)
                    {
                        nodeEnumerator.Current.OnExit(this);
                        State.LastGlottisLevel = State.GlottisLevel;
                        if (!nodeEnumerator.MoveNext()) goto done;
                        nodeEnumerator.Current.OnEnter(this);
                    }

                    nodeEnumerator.Current.OnUpdate(this);

                    // Run synthesizer on current sample
                    double currentSample = 0f;
                    foreach (var sampler in samplerSequence)
                    {
                        if (!sampler.Enabled) continue;
                        sampler.Update(ref currentSample);
                    }

                    // Limit signal to mitigate SCReeEEEEEEEEEEEEEE
                    Util.Sigmoid(ref currentSample);

                    data[len++] = unchecked((short)(currentSample * short.MaxValue));

                    // If a chunk is completed, push it out
                    if (len >= blockSize)
                    {
                        playback.QueueDataBlock(data, len, SampleRate);
                        len = 0;
                    }

                    Position++;
                }
                done:
                // Push out any remaining chunk, even if it's not full
                if (len > 0) playback.QueueDataBlock(data, len, SampleRate);

                playback.MarkComplete(() => ttsPlayback.RemoveActiveAudio(playback));

                nodeEnumerator.Dispose();
            });
        }
    }
}