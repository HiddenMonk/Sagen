using System;

namespace Sagen.Phonetics
{
	[Flags]
	internal enum PhoneticFlags
	{
		Past,
		Present,
		Future,
		Coordinating,
		Subordinating,
		Noun,
		Verb,
		Adjective,
		Emphasis,
		Pronoun
	}
}