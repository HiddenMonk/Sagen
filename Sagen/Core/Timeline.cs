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

using System.Collections;
using System.Collections.Generic;

using Sagen.Core.Nodes;
using Sagen.Extensibility;

namespace Sagen.Core
{
    /// <summary>
    /// Implements a speech timeline, which uses a chain of event nodes to instruct the synthesizer how to generate audio over
    /// time.
    /// </summary>
    internal sealed class Timeline : IEnumerable<Node>, ISpeechTimeline
    {
        private int _count;
        private Node _first, _last;

        public double LengthSeconds => _last.StartTime + _last.Duration;

        public int Count => _count;

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<Node> GetEnumerator()
        {
            var current = _first;
            while (current != null)
            {
                yield return current;
                current = current.Next;
            }
        }

        public void AddSilence(double seconds) => Add(new SilenceNode(seconds));
        public void AddPhoneme(double seconds, double h, double b, double r) => Add(new PhonemeNode(seconds, h, b, r));

        public void Add(Node node)
        {
            _count++;

            if (_first == null)
            {
                _first = node;
                _last = node;
                return;
            }

            node.SetPrev(_last);
            _last = node;
        }
    }
}