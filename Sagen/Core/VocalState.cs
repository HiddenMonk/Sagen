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

namespace Sagen.Core
{
    internal sealed class VocalState
    {
        /// <summary>
        /// The current glottis level.
        /// </summary>
        public double GlottisLevel { get; set; } = 0.0;

        /// <summary>
        /// The current aspiration level.
        /// </summary>
        public double AspirationLevel { get; set; } = 1.0;

        /// <summary>
        /// The current frication level.
        /// </summary>
        public double FricationLevel { get; set; } = 0.0;

        /// <summary>
        /// The glottis level on the previous node's exit.
        /// </summary>
        public double LastGlottisLevel { get; set; } = 0.0;

        public double Height { get; set; } = 0.0;
        public double Backness { get; set; } = 0.0;
        public double Roundedness { get; set; } = 0.0;
    }
}