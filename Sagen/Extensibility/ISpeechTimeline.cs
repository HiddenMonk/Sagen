namespace Sagen.Extensibility
{
	public interface ISpeechTimeline
	{
		void AddSilence(double seconds);
		void AddPhonation(double seconds, double h, double b, double r);
	}
}