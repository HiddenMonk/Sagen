using Sagen.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sagen
{
    public sealed class TTS<TPlaybackEngine> : TTS where TPlaybackEngine : IPlaybackEngine, new()
    {
        private readonly HashSet<IPlaybackEngine> _activeStreams = new HashSet<IPlaybackEngine>();
        private readonly Dictionary<IPlaybackEngine, ManualResetEventSlim> _resetEvents = new Dictionary<IPlaybackEngine, ManualResetEventSlim>();

        public TTS() : base()
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
            CreateSynth().Play<TPlaybackEngine>(5.0);
        }

        public void Sync()
        {
            lock (_activeStreams)
            {
                foreach (var audio in _activeStreams)
                {
                    _resetEvents[audio].Wait();
                }
            }
        }
    }
}
