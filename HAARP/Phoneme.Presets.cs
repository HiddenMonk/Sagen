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
			// Close vowels
			_presets.Add('i', new Phoneme(CLOSE, FRONT, UNROUNDED)); // 'ie' in 'wie' (German)
			_presets.Add('y', new Phoneme(CLOSE, FRONT, ROUNDED)); // 'ü' in 'über' (German)
			_presets.Add('\u0268', new Phoneme(CLOSE, CENTRAL, UNROUNDED));
			_presets.Add('\u0289', new Phoneme(CLOSE, CENTRAL, ROUNDED));
			_presets.Add('\u026f', new Phoneme(CLOSE, BACK, UNROUNDED));
			_presets.Add('u', new Phoneme(CLOSE, BACK, ROUNDED)); // 'u' in 'Fuß' (German)

			// Close-mid vowels
			_presets.Add('e', new Phoneme(CLOSE_MID, FRONT, UNROUNDED)); // 'ee' in 'Seele' (German)
			_presets.Add('\u00f8', new Phoneme(CLOSE_MID, BACK, ROUNDED)); // 'ö' in 'schön' (German)
			_presets.Add('\u0258', new Phoneme(CLOSE_MID, CENTRAL, UNROUNDED)); // 'e' in 'bitte' (German)
			_presets.Add('\u0275', new Phoneme(CLOSE_MID, CENTRAL, ROUNDED)); // 'oo' in 'foot' (UK English)
			_presets.Add('\u0264', new Phoneme(CLOSE_MID, BACK, UNROUNDED));
			_presets.Add('o', new Phoneme(CLOSE_MID, BACK, ROUNDED)); // 'o' in 'oder' (German)

			// Mid vowels
			_presets.Add('\u0259', new Phoneme(MID, CENTRAL, UNROUNDED)); // 'u' in 'under' (US English)

			// Open-mid vowels
			_presets.Add('\u025b', new Phoneme(OPEN_MID, FRONT, UNROUNDED)); // 'e' in 'ten' (US English)
			_presets.Add('\u0153', new Phoneme(OPEN_MID, FRONT, ROUNDED)); // 'ö' in 'löschen' (German)
			_presets.Add('\u025c', new Phoneme(OPEN_MID, CENTRAL, UNROUNDED)); // 'ir' in 'bird' (UK English)
			_presets.Add('\u025e', new Phoneme(OPEN_MID, CENTRAL, ROUNDED)); // 'um' in 'Parfum' (German)
			_presets.Add('\u028c', new Phoneme(OPEN_MID, BACK, UNROUNDED));
			_presets.Add('\u0254', new Phoneme(OPEN_MID, BACK, ROUNDED)); // 'o' in 'voll' (German)

			// Near-open vowels
			_presets.Add('\u00e6', new Phoneme(NEAR_OPEN, FRONT, UNROUNDED)); // 'a' in 'cat' (US English)
			_presets.Add('\u0250', new Phoneme(NEAR_OPEN, CENTRAL, UNROUNDED)); // 'er' in 'oder' (German)

			// Open vowels
			_presets.Add('a', new Phoneme(OPEN, FRONT, UNROUNDED)); // 'a' in 'aber' (German)
			_presets.Add('\u0276', new Phoneme(OPEN, FRONT, ROUNDED));
			_presets.Add('\u0251', new Phoneme(OPEN, BACK, UNROUNDED)); // 'o' in 'hot' (US English)
			_presets.Add('\u0252', new Phoneme(OPEN, BACK, ROUNDED)); // 'o' in 'not' (UK English)
		}
	}
}