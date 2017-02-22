namespace Sagen.Extensibility
{
    /// <summary>
    /// Exposes timeline creation functionality to the plugin system.
    /// </summary>
	public interface ISpeechTimeline
	{
		void AddSilence(double seconds);
		void AddPhoneme(double seconds, double h, double b, double r);
        //void AddStress();
        //void AddClauseBoundary();
	}
}