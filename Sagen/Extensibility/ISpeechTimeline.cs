namespace Sagen.Extensibility
{
	public interface ISpeechTimeline
	{
		void AddSilence(double seconds);
		void AddPhonation(double seconds);
	}
}