using System;
using System.Runtime.CompilerServices;

namespace Sagen.Core.Nodes
{
	/// <summary>
	/// Represents a node in a speech event timeline used for controlling synthesis parameters over time.
	/// Nodes can be connected together like a linked list, such that the synthesizer will traverse them in series.
	/// </summary>
	internal abstract class SpeechNode
	{
		private SpeechNode _prev, _next;
		private readonly double _duration;
		private double _startTime;

		public SpeechNode(double duration)
		{
			if (duration <= 0) throw new ArgumentOutOfRangeException(nameof(duration), "Duration must be greater than zero.");
			_duration = duration;
		}

		public void SetPrev(SpeechNode node)
		{
			if (_prev != null || node._next != null) return;
			this._prev = node;
			node._next = this;
			_startTime = _prev?._startTime + _prev?._duration ?? 0.0;
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

		public double StartTime => _startTime;

		public SpeechNode Next => _next;

		public SpeechNode Prev => _prev;

		public double Duration => _duration;

		public double EndTime => _startTime + _duration;
		
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
