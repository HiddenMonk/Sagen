namespace Sagen.Core.Voc
{
	internal sealed class TransientPool
	{
		private int _size, _nextFree;
		private Transient _root;

		public readonly Transient[] Pool;
		public Transient Root => _root;
		public int Size => _size;
		public int NextFree => _nextFree;

		public TransientPool()
		{
			_size = 0;
			_nextFree = 0;
			Pool = new Transient[VocSynth.MaxTransients];
			for(int i = 0; i < VocSynth.MaxTransients; i++)
			{
				Pool[i] = new Transient((uint)i);
			}
		}

		public bool AppendTransient(int position)
		{
			int freeID = NextFree;

			if (_size == VocSynth.MaxTransients) return false;

			if (freeID == -1)
			{
				for (int i = 0; i < VocSynth.MaxTransients; i++)
				{
					if (Pool[i].IsFree)
					{
						freeID = i;
						break;
					}
				}
			}

			if (freeID == -1) return false;

			var t = Pool[freeID];
			t.Next = _root;
			_root = t;
			_size++;
			t.IsFree = false;
			t.TimeAlive = 0;
			t.Lifetime = 0.2;
			t.Strength = 0.3;
			t.Exponent = 200;
			t.Position = position;
			_nextFree = -1;
			return true;
		}

		public void RemoveTransient(uint id)
		{
			_nextFree = (int)id;
			var n = _root;

			if (id == n.ID)
			{
				_root = n.Next;
				_size--;
				return;
			}

			for (int i = 0; i < _size; i++)
			{
				if (n.Next.ID == id)
				{
					_size--;
					n.Next.IsFree = true;
					n.Next = n.Next.Next;
				}
				n = n.Next;
			}
		}
	}
}
