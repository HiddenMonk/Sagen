namespace Sagen.Extensibility
{
	/// <summary>
	/// Exposes synthesizer functionality to the plugin system.
	/// </summary>
	public interface ISynthesizer
	{
		double TimeStep { get; }
		double TimePosition { get; }
		double Fundamental { get; set; }
		int SampleRate { get; }
	}
}