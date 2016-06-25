using System.Collections.Generic;

namespace HAARP
{
	internal partial class Phoneme
	{
		private static readonly Dictionary<char, Phoneme> _presets = new Dictionary<char, Phoneme>();

		// Height
		private const float CLOSE = 1.0f;
		private const float NEAR_CLOSE = 0.85f;
		private const float CLOSE_MID = 0.65f;
		private const float MID = 0.5f;
		private const float OPEN_MID = 0.35f;
		private const float NEAR_OPEN = 0.15f;
		private const float OPEN = 0.0f;

		// Backness
		private const float FRONT = 0.0f;
		private const float NEAR_FRONT = 0.25f;
		private const float CENTRAL = 0.5f;
		private const float NEAR_BACK = 0.75f;
		private const float BACK = 1.0f;

		// Roundedness
		private const float ROUNDED = 1.0f;
		private const float UNROUNDED = 0.0f;

		static Phoneme()
		{
			_presets.Add('\u00e6', new Phoneme(true, NEAR_OPEN, FRONT, UNROUNDED)); // 'a' in 'cat' (English)
			_presets.Add('\u025b', new Phoneme(true, OPEN_MID, FRONT, UNROUNDED)); // 'e' in 'ten' (English)
			_presets.Add('\u0153', new Phoneme(true, OPEN_MID, FRONT, ROUNDED)); // 'ö' in 'löschen' (German)
			_presets.Add('a', new Phoneme(true, OPEN, FRONT, UNROUNDED)); // 'a' in 'aber' (German)

		}
	}
}