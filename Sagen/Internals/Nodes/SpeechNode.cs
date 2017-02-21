using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Sagen.Internals.Nodes
{
	/// <summary>
	/// Represents a node in a speech event timeline used for controlling synthesis parameters over time.
	/// Nodes can be connected together like a linked list, such that the synthesizer will traverse them in series.
	/// </summary>
	internal abstract class SpeechNode
	{
		private SpeechNode _prev, _next;
		private readonly double _duration;
		private readonly double _startTime;

		public SpeechNode(SpeechNode prev, double duration)
		{
			if (duration <= 0) throw new ArgumentOutOfRangeException(nameof(duration), "Duration must be greater than zero.");
			SetPrev(prev);
			_duration = duration;
			_startTime = _prev?._startTime + _prev?._duration ?? 0.0;
		}

		private void SetPrev(SpeechNode node)
		{
			if (_prev != null || node._next != null) return;
			this._prev = node;
			node._next = this;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double GetNormalizedRelativePos(Synthesizer synth)
		{
			return (synth.TimePosition - _startTime) / _duration;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double GetRelativeSeconds(Synthesizer synth)
		{
			return synth.TimePosition - _startTime;
		}
		
		/// <summary>
		/// Invoked when the node is first entered.
		/// </summary>
		/// <param name="synth">The synthesizer to affect.</param>
		/// <returns></returns>
		public abstract double OnEnter(Synthesizer synth);

		/// <summary>
		/// Invoked when the node is exited.
		/// </summary>
		/// <param name="synth">The synthesizer to affect.</param>
		/// <returns></returns>
		public abstract double OnExit(Synthesizer synth);

		/// <summary>
		/// Invoked for each sample on which the synthesizer uses this node.
		/// </summary>
		/// <param name="synth">The synthesizer to affect.</param>
		/// <returns></returns>
		public abstract double OnUpdate(Synthesizer synth);
	}
}
