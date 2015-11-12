using System.Collections.Generic;

namespace HAARP
{
	internal static class BufferPool
	{
		public const int DefaultInitialCount = 5;
		public const int DefaultInitialBufferSize = 220500;

		private static int _initialCount = DefaultInitialCount;
		private static int _initialBufferSize = DefaultInitialBufferSize;

		private static readonly object syncObj = new object();
		private static List<float[]> _buffers;
		private static readonly HashSet<float[]> _loans = new HashSet<float[]>();

		public static int InitialCount
		{
			get { return _initialCount; }
			set
			{
				_initialCount = value < 1 ? 1 : value;
			}
		}

		public static int InitialBufferSize
		{
			get { return _initialBufferSize; }
			set
			{
				_initialBufferSize = value < 0 ? 0 : value;
			}
		}

		public static void Init()
		{
			if (_buffers != null) return;
			_buffers = new List<float[]>();
			for (int i = 0; i < _initialCount; i++)
			{
				_buffers.Add(new float[_initialBufferSize]);
			}
		}

		public static float[] Loan()
		{
			lock (syncObj)
			{
				Init();
				foreach (var buffer in _buffers)
				{
					if (_loans.Contains(buffer)) continue;
					_loans.Add(buffer);
					return buffer;
				}

				var bufferNew = new float[_initialBufferSize];
				_buffers.Add(bufferNew);
				_loans.Add(bufferNew);
				return bufferNew;
			}
		}

		public static void Reclaim(float[] buffer)
		{
			lock (syncObj)
			{
				Init();
				_loans.Remove(buffer);
			}
		}
	}
}