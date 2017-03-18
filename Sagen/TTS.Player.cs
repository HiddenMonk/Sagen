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
using System.Threading;

using Sagen.Extensibility;

namespace Sagen
{
    public sealed class TTS<TPlaybackEngine> : TTS where TPlaybackEngine : IPlaybackEngine, new()
    {
        private readonly HashSet<IPlaybackEngine> _activeStreams = new HashSet<IPlaybackEngine>();
        private readonly Dictionary<IPlaybackEngine, ManualResetEventSlim> _resetEvents = new Dictionary<IPlaybackEngine, ManualResetEventSlim>();

        public TTS()
        {
        }

        public TTS(Voice voice) : base(voice)
        {
        }

        internal void AddActiveAudio(IPlaybackEngine audio)
        {
            lock (_activeStreams)
            {
                _activeStreams.Add(audio);
                _resetEvents[audio] = new ManualResetEventSlim();
            }
        }

        internal void RemoveActiveAudio(IPlaybackEngine audio)
        {
            var resetEvent = _resetEvents[audio];
            resetEvent.Set();
            lock (_activeStreams)
            {
                _activeStreams.Remove(audio);
                _resetEvents.Remove(audio);
                resetEvent.Dispose();
            }
        }

        public void Speak(string text)
        {
            // Actual speaking isn't supported yet. This is debug code for testing vocal properties.			
            CreateSynth(text).Play(this);
        }

        public void Sync()
        {
            lock (_activeStreams)
            {
                foreach (var audio in _activeStreams)
                    _resetEvents[audio].Wait();
            }
        }
    }
}