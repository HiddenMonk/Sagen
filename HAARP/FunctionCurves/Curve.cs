using System;
using System.Collections.Generic;
using System.Linq;

namespace HAARP.FunctionCurves
{
    internal abstract class Curve
    {
        private readonly Dictionary<int, HashSet<int>> index = new Dictionary<int, HashSet<int>>();
        private Keyframe[] keys;
        private bool empty;
        private Keyframe firstKey, lastKey;

        public Curve(Keyframe[] keyframes)
        {
            if (keyframes == null) throw new ArgumentNullException(nameof(keyframes));
            keys = keyframes.OrderBy(k => k.Time).ToArray();
            empty = keys.Length == 0;
            if (!empty)
            {
                firstKey = keys[0];
                lastKey = keys[keys.Length - 1];
            }
        }

        public Keyframe[] Keyframes
        {
            get { return keys.ToArray(); }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                keys = value.OrderBy(k => k.Time).ToArray();
                empty = keys.Length == 0;
                if (!empty)
                {
                    firstKey = keys[0];
                    lastKey = keys[keys.Length - 1];
                }
            }
        }

        public bool IsEmpty => empty;

        public float this[float time]
        {
            get
            {
                if (empty) return 0.0f;
                if (keys.Length == 1) return keys[0].Value;
                if (time >= lastKey.Time) return lastKey.Value;
                if (time <= firstKey.Time) return firstKey.Value;

                // Perform a binary search. Gotta go fast
                int start = 0;
                int size = keys.Length;
                int mid;
                while (true)
                {
                    if (size == 2)
                    {
                        return Interpolate(keys[start], keys[start + 1],
                            (time - keys[start].Time) / (keys[start + 1].Time - keys[start].Time));
                    }

                    mid = start + size / 2;
                    if (size % 2 == 0)
                    {
                        size /= 2;
                        if (time >= keys[mid].Time)
                        {
                            start += size;
                        }
                    }
                    else
                    {
                        if (time >= keys[mid].Time)
                        {
                            start = mid;
                        }
                        size = size / 2 + 1;
                    }
                }
            }
        }

        protected abstract float Interpolate(Keyframe left, Keyframe right, float delta);
    }
}