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

namespace Sagen
{
    /// <summary>
    /// Defines sample formats for PCM speech data.
    /// </summary>
    public enum SampleFormat : short
    {
        /// <summary>
        /// 64-bit floating-point numbers normalized to [-1, 1].
        /// </summary>
        Float64 = 64,

        /// <summary>
        /// 32-bit floating-point numbers normalized to [-1, 1].
        /// </summary>
        Float32 = 32,

        /// <summary>
        /// 16-bit signed integers normalized to [-32768, 32767].
        /// </summary>
        Signed16 = 16,

        /// <summary>
        /// 8-bit unsigned integers normalized to [0, 255] for that extra-butchered sample quality you love.
        /// </summary>
        Unsigned8 = 8
    }
}