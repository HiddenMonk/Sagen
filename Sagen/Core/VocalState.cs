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
	/// <summary>
	/// The vocal state maintains dynamic state information about the synthesizer's signal generators and corresponding filters.
	/// </summary>
    internal sealed class VocalState
    {
        /// <summary>
        /// The current glottis level.
        /// This defines how "open" the glottis is. When completely closed (= 0.0), no sound is produced.
        /// </summary>
        public double GlottisLevel { get; set; } = 0.0;

        /// <summary>
        /// The current aspiration level.
        /// Aspiration is the amount of turbulence produced by the glottis.
        /// </summary>
        public double AspirationLevel { get; set; } = 1.0;

        /// <summary>
        /// The current frication level.
        /// Frication is turbulence produced by air passing through a narrow gap between an active and passive articulator.
        /// </summary>
        public double FricationLevel { get; set; } = 0.0;

        /// <summary>
        /// The glottis level on the previous node's exit.
        /// </summary>
        public double LastGlottisLevel { get; set; } = 0.0;

		/// <summary>
		/// Vowel height.
		/// </summary>
        public double Height { get; set; } = 0.0;

		/// <summary>
		/// Vowel backness.
		/// </summary>
        public double Backness { get; set; } = 0.0;

		/// <summary>
		/// Vowel roundedness.
		/// </summary>
        public double Roundedness { get; set; } = 0.0;
    }
}