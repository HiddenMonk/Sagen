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
using System.Runtime.CompilerServices;

namespace Sagen.Core.Nodes
{
    /// <summary>
    /// Represents a node in a speech event timeline used for controlling synthesis parameters over time.
    /// Nodes can be connected together like a linked list, such that the synthesizer will traverse them in series.
    /// </summary>
    internal abstract class Node
    {
        public Node(double duration)
        {
            if (duration <= 0) throw new ArgumentOutOfRangeException(nameof(duration), "Duration must be greater than zero.");
            Duration = duration;
        }

        public double StartTime { get; private set; }

        public Node Next { get; private set; }

        public Node Prev { get; private set; }

        public double Duration { get; }

        public double EndTime => StartTime + Duration;

        public void SetPrev(Node node)
        {
            if (Prev != null || node.Next != null) return;
            Prev = node;
            node.Next = this;
            StartTime = Prev?.StartTime + Prev?.Duration ?? 0.0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double GetNormalizedRelativePos(Synthesizer synth)
        {
            return (synth.TimePosition - StartTime) / Duration;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double GetRelativeSeconds(Synthesizer synth)
        {
            return synth.TimePosition - StartTime;
        }

        /// <summary>
        /// Invoked when the node is first entered.
        /// </summary>
        /// <param name="synth">The synthesizer to affect.</param>
        /// <returns></returns>
        public abstract void OnEnter(Synthesizer synth);

        /// <summary>
        /// Invoked when the node is exited.
        /// </summary>
        /// <param name="synth">The synthesizer to affect.</param>
        /// <returns></returns>
        public abstract void OnExit(Synthesizer synth);

        /// <summary>
        /// Invoked for each sample on which the synthesizer uses this node.
        /// </summary>
        /// <param name="synth">The synthesizer to affect.</param>
        /// <returns></returns>
        public abstract void OnUpdate(Synthesizer synth);
    }
}