namespace Sagen.Extensibility
{
	public delegate void RemoveActivePlaybackEngineCallback();

	public interface IPlaybackEngine
	{
		void QueueDataBlock(short[] buffer, int length, int sampleRate);
		void MarkComplete(RemoveActivePlaybackEngineCallback callback);
	}
}
