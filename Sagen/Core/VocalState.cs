namespace Sagen.Core
{
	internal sealed class VocalState
	{
		/// <summary>
		/// The current glottis level.
		/// </summary>
		public double GlottisLevel { get; set; } = 0.0;

		/// <summary>
		/// The current aspiration level.
		/// </summary>
		public double AspirationLevel { get; set; } = 1.0;

		/// <summary>
		/// The current frication level.
		/// </summary>
		public double FricationLevel { get; set; } = 0.0;

		/// <summary>
		/// The glottis level on the previous node's exit.
		/// </summary>
		public double LastGlottisLevel { get; set; } = 0.0;

		public double Height { get; set; } = 0.0;
		public double Backness { get; set; } = 0.0;
		public double Roundedness { get; set; } = 0.0;
	}
}