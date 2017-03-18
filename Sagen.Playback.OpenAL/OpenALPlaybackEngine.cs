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
using System.Collections.Generic;
using System.Threading;

using Sagen.Extensibility;

using static Sagen.Playback.OpenAL.AL10;
using static Sagen.Playback.OpenAL.ALC10;

namespace Sagen.Playback.OpenAL
{
    /// <summary>
    /// Represents a speech playback engine that implements the OpenAL API.
    /// </summary>
    public sealed class OpenALPlaybackEngine : IPlaybackEngine
    {
        private const int BUFFER_COUNT = 64;
        private static readonly IntPtr pDevice, pContext;

        private readonly uint[] buffers = new uint[BUFFER_COUNT];
        private readonly Stack<uint> freeBuffers = new Stack<uint>();
        private readonly uint[] removedBuffers = new uint[BUFFER_COUNT];
        private uint source;

        static OpenALPlaybackEngine()
        {
            pDevice = alcOpenDevice(null);
            pContext = alcCreateContext(pDevice, null);
            alcMakeContextCurrent(pContext);
            AppDomain.CurrentDomain.ProcessExit += (sender, args) =>
            {
                alcMakeContextCurrent(IntPtr.Zero);
                alcDestroyContext(pContext); // Destroys sources too?
                alcCloseDevice(pDevice);
            };
        }

        public OpenALPlaybackEngine()
        {
            alGenBuffers(new IntPtr(BUFFER_COUNT), buffers);
            alGenSources(new IntPtr(1), out source);
            for (int i = 0; i < BUFFER_COUNT; i++) freeBuffers.Push(buffers[i]);
        }

        public void QueueDataBlock(short[] buffer, int length, int sampleRate)
        {
            // Free processed buffers
            int numEmpty;
            alGetSourcei(source, AL_BUFFERS_PROCESSED, out numEmpty);
            if (numEmpty > 0)
            {
                alSourceUnqueueBuffers(source, new IntPtr(numEmpty), removedBuffers);
                for (int i = 0; i < numEmpty; i++)
                    freeBuffers.Push(removedBuffers[i]);
            }

            uint b = freeBuffers.Pop();
            alBufferData(b, AL_FORMAT_MONO16, buffer, new IntPtr(length * 2), new IntPtr(sampleRate));

            alSourceQueueBuffers(source, new IntPtr(1), ref b);

            int state;
            alGetSourcei(source, AL_SOURCE_STATE, out state);
            if (state != AL_PLAYING) alSourcePlay(source);
        }

        public void MarkComplete(RemoveActivePlaybackEngineCallback callback)
        {
            ThreadPool.QueueUserWorkItem(DisposeOnFinish, callback);
        }

        private void DisposeOnFinish(object cbObject)
        {
            var removeAsActive = cbObject as RemoveActivePlaybackEngineCallback;
            int state;

            do
            {
                alGetSourcei(source, AL_SOURCE_STATE, out state);
                Thread.Sleep(10);
            } while (state == AL_PLAYING);

            alSourcei(source, AL_BUFFER, 0);
            alDeleteSources(new IntPtr(1), ref source);
            alDeleteBuffers(new IntPtr(BUFFER_COUNT), buffers);

            removeAsActive();

            GC.KeepAlive(this);
        }
    }
}