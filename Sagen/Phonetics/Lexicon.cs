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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Sagen.Phonetics
{
    internal sealed class Lexicon
    {
        private readonly Dictionary<string, Dictionary<Heteronym, string>> _entries = new Dictionary<string, Dictionary<Heteronym, string>>();  

        public static Lexicon FromStream(Stream stream)
        {
            var lex = new Lexicon();
            using (var reader = new StreamReader(stream, Encoding.Unicode, true, 256, true))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (line == null) continue;
                    var entry = line.Split('=').Select(s => s.Trim()).ToArray();
                    if (entry.Length != 2) continue;
                    var keys = entry[0].Split('|').Select(s => s.Trim()).ToArray();
                    var values = entry[1]
                        .Split(',')
                        .Select(s => s.Trim())
                        .Where(s => s.Length > 2 && s.StartsWith("/"))
                        .Select(s => s.Split('/'))
                        .Where(s => s.Length == 2)
                        .ToArray();

                    foreach (var key in keys)
                    {
                        lex._entries[key] = values.ToDictionary(pair => GetHeteronymFromCode(pair[0]), pair => pair[1]);
                    }
                }
            }
            return lex;
        }

        private static Heteronym GetHeteronymFromCode(string code)
        {
            switch (code)
            {
                case "n":
                    return Heteronym.Noun;
                case "adj":
                    return Heteronym.Adjective;
                case "v":
                case "vb":
                    return Heteronym.Verb;
                case "adv":
                    return Heteronym.Adverb;
                case "anim":
                    return Heteronym.Animate;
                case "inan":
                    return Heteronym.Inanimate;
                case "nps":
                    return Heteronym.NonPrimaryStress;
                case "ps":
                    return Heteronym.PrimaryStress;
                case "rnd":
                    return Heteronym.Rounded;
                case "unr":
                    return Heteronym.Unrounded;
                case "past":
                    return Heteronym.Past;
                case "em":
                    return Heteronym.Emphasis;
                default:
                    return Heteronym.Noun;
            }
        }
    }
}