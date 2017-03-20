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

namespace Sagen.Phonetics
{
    public static class TextUtils
    {
        /// <summary>
        /// Converts a Roman numeral string to an integer.
        /// </summary>
        /// <param name="romanNumeral">The Roman numeral to convert.</param>
        /// <returns></returns>
        public static int RomanNumeralToInt(string romanNumeral)
        {
            if (String.IsNullOrEmpty(romanNumeral)) return 0;
            int number = 0;
            int a = GetRomanValue(romanNumeral[0]);
            int b = 0;
            for (int i = 0; i < romanNumeral.Length; i++)
            {
                b = GetRomanValue(romanNumeral[i + 1]);
                if (a > 0)
                {
                    if (i + 1 < romanNumeral.Length && a < b)
                    {
                        number -= a;
                    }
                    else
                    {
                        number += a;
                    }
                }
                a = b;
            }
            return number;
        }

        private static int GetRomanValue(char c)
        {
            switch (c)
            {
                case 'I':
                case 'i':
                    return 1;
                case 'V':
                case 'v':
                    return 5;
                case 'X':
                case 'x':
                    return 10;
                case 'L':
                case 'l':
                    return 50;
                case 'C':
                case 'c':
                    return 100;
                case 'D':
                case 'd':
                    return 500;
                case 'M':
                case 'm':
                    return 1000;
                default:
                    return 0;
            }
        }
    }
}