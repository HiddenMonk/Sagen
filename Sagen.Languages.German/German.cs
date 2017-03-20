using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sagen.Extensibility;

namespace Sagen.Languages.German
{
    public sealed class German : SagenLanguage
    {
        protected override void ReadUnknownWord(string word, ISpeechTimeline writer)
        {
            throw new NotImplementedException();
        }
    }
}
