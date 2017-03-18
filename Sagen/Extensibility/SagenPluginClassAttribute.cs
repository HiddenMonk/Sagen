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

namespace Sagen.Extensibility
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public class SagenPluginClassAttribute : Attribute
    {
        public SagenPluginClassAttribute(Type pluginClassType, string pluginName)
        {
            if (pluginClassType == null)
                throw new NullReferenceException("Plugin type is null.");
            if (String.IsNullOrWhiteSpace(pluginName))
                throw new ArgumentException($"Parameter '{nameof(pluginName)}' must contain non-whitespace characters.");
            if (!pluginClassType.IsSubclassOf(typeof(SagenLanguage)))
                throw new ArgumentException($"Plugin type {pluginClassType} must derive from {nameof(SagenLanguage)}.");

            PluginClassType = pluginClassType;
            PluginName = pluginName;
        }

        public Type PluginClassType { get; }
        public string PluginName { get; }
    }
}