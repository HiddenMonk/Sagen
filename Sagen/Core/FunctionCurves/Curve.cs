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
using System.Linq;

namespace Sagen.Core.FunctionCurves
{
    internal abstract class Curve
    {
        private bool empty;
        private Keyframe firstKey, lastKey;
        private Keyframe[] keys;

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
                            start += size;
                    }
                    else
                    {
                        if (time >= keys[mid].Time)
                            start = mid;
                        size = size / 2 + 1;
                    }
                }
            }
        }

        protected abstract float Interpolate(Keyframe left, Keyframe right, float delta);
    }
}