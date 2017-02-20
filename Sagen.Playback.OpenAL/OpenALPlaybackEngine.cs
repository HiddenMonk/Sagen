using Sagen.Extensibility;
using System;
using System.Collections.Generic;
using System.Threading;

using static Sagen.Playback.OpenAL.AL10;
using static Sagen.Playback.OpenAL.ALC10;

namespace Sagen.Playback.OpenAL
{
	/// <summary>
	/// Represents a speech playback engine that implements the OpenAL API.
	/// </summary>
	public sealed class OpenALPlaybackEngine : IPlaybackEngine
	{
		private static readonly IntPtr pDevice, pContext;

		private const int BUFFER_COUNT = 64;

		private readonly uint[] buffers = new uint[BUFFER_COUNT];
		private readonly uint[] removedBuffers = new uint[BUFFER_COUNT];
		private readonly Stack<uint> freeBuffers = new Stack<uint>();
		private uint source;

		static OpenALPlaybackEngine()
		{
			pDevice = alcOpenDevice(null);
			pContext = alcCreateContext(pDevice, null);
			alcMakeContextCurrent(pContext);
			AppDomain.CurrentDomain.ProcessExit += (sender, args) =>
			{
				alcMakeContextCurrent(IntPtr.Zero);
				alcDestroyContext(pContext); // Destroys sources too?
				alcCloseDevice(pDevice);
			};
		}

		public OpenALPlaybackEngine()
		{
			alGenBuffers(new IntPtr(BUFFER_COUNT), buffers);
			alGenSources(new IntPtr(1), out source);
			for (int i = 0; i < BUFFER_COUNT; i++) freeBuffers.Push(buffers[i]);
		}

		public void QueueDataBlock(short[] buffer, int length, int sampleRate)
		{
			// Free processed buffers
			int numEmpty;
			alGetSourcei(source, AL_BUFFERS_PROCESSED, out numEmpty);
			if (numEmpty > 0)
			{
				alSourceUnqueueBuffers(source, new IntPtr(numEmpty), removedBuffers);
				for (int i = 0; i < numEmpty; i++)
				{
					freeBuffers.Push(removedBuffers[i]);
				}
			}

			uint b = freeBuffers.Pop();
			alBufferData(b, AL_FORMAT_MONO16, buffer, new IntPtr(length * 2), new IntPtr(sampleRate));

			alSourceQueueBuffers(source, new IntPtr(1), ref b);

			int state;
			alGetSourcei(source, AL_SOURCE_STATE, out state);
			if (state != AL_PLAYING) alSourcePlay(source);
		}

		private void DisposeOnFinish(object cbObject)
		{
			var removeAsActive = cbObject as RemoveActivePlaybackEngineCallback;
			int state;

			do
			{
				alGetSourcei(source, AL_SOURCE_STATE, out state);
				Thread.Sleep(10);
			} while (state == AL_PLAYING);

			alSourcei(source, AL_BUFFER, 0);
			alDeleteSources(new IntPtr(1), ref source);
			alDeleteBuffers(new IntPtr(BUFFER_COUNT), buffers);
			
			removeAsActive();

			GC.KeepAlive(this);
		}

		public void MarkComplete(RemoveActivePlaybackEngineCallback callback)
		{
			ThreadPool.QueueUserWorkItem(DisposeOnFinish, callback);
		}
	}
}
