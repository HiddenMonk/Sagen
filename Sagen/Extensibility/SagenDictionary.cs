using System.IO;
using System.Text;

namespace Sagen.Extensibility
{
	internal sealed class SagenDictionary
	{
		public static SagenDictionary FromStream(Stream stream)
		{
			using (var reader = new StreamReader(stream, Encoding.Unicode, true, 256, true))
			{
				return null;
			}
		}
	}
}