using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Sagen.Extensibility
{
	internal sealed class SagenLexicon
	{
        private readonly Dictionary<string, Dictionary<Heteronym, string>> _entries = new Dictionary<string, Dictionary<Heteronym, string>>();



		public static SagenLexicon FromStream(Stream stream)
		{
			return null;
			using (var reader = new StreamReader(stream, Encoding.Unicode, true, 256, true))
			{
				while(!reader.EndOfStream)
                {
                    
                }
			}
            
		}
	}
}