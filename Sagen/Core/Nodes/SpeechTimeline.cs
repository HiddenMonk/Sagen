using System.Collections;
using System.Collections.Generic;

using Sagen.Extensibility;

namespace Sagen.Core.Nodes
{
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
		public void AddPhonation(double seconds) => Add(new SyllableNode(seconds));
	}
}