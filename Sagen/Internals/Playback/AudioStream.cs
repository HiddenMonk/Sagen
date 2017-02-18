using System;
using System.IO;

namespace Sagen.Internals.Playback
{
	internal abstract class AudioStream
	{
		public abstract void Dispose();

		public abstract void QueueDataBlock(short[] data, int length);

		public abstract void WaitToFinish();

		public abstract void Cleanup();

		~AudioStream()
		{
			Dispose();
		}
	}
}
