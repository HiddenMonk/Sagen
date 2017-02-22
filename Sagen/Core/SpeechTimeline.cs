using System.Collections;
using System.Collections.Generic;

using Sagen.Core.Nodes;
using Sagen.Extensibility;

namespace Sagen.Core
{
    /// <summary>
    /// Implements a speech timeline, which uses a chain of event nodes to instruct the synthesizer how to generate audio over time.
    /// </summary>
	internal sealed class SpeechTimeline : IEnumerable<SpeechNode>, ISpeechTimeline
	{
		private SpeechNode _first, _last;
		private int _count;

		public void Add(SpeechNode node)
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

		public double LengthSeconds => _last.StartTime + _last.Duration;

		public int Count => _count;

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public IEnumerator<SpeechNode> GetEnumerator()
		{
			SpeechNode current = _first;
			while (current != null)
			{
				yield return current;
				current = current.Next;
			}
		}

		public void AddSilence(double seconds) => Add(new SilenceNode(seconds));
		public void AddPhoneme(double seconds, double h, double b, double r) => Add(new PhonemeNode(seconds, h, b, r));
	}
}