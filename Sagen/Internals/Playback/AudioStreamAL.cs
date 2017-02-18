using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Sagen.Internals.Playback.AL.AL10;
using static Sagen.Internals.Playback.AL.ALC10;

namespace Sagen.Internals.Playback
{
	internal sealed unsafe class AudioStreamAL : AudioStream
	{
		private const string LibraryName = "openal32.dll";
		private const int BUFFER_COUNT = 64;
		private readonly ManualResetEventSlim _resetEvent = new ManualResetEventSlim(false);

		private readonly Synthesizer s;
		private uint[] buffers = new uint[BUFFER_COUNT];
		private uint[] removedBuffers = new uint[BUFFER_COUNT];
		private readonly Stack<uint> freeBuffers = new Stack<uint>();
		private uint source;

		public AudioStreamAL(SampleFormat format, Synthesizer synth)
		{
			s = synth;
			alGenBuffers(new IntPtr(BUFFER_COUNT), buffers);
			alGenSources(new IntPtr(1), out source);
			for (int i = 0; i < BUFFER_COUNT; i++) freeBuffers.Push(buffers[i]);
		}


		public override void Dispose()
		{

		}

		public override void QueueDataBlock(short[] data, int length)
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
			alBufferData(b, AL_FORMAT_MONO16, data, new IntPtr(length * 2), new IntPtr(s.SampleRate));

			alSourceQueueBuffers(source, new IntPtr(1), ref b);

			int state;
			alGetSourcei(source, AL_SOURCE_STATE, out state);
			if (state != AL_PLAYING) alSourcePlay(source);
		}

		public override void Cleanup()
		{
			ThreadPool.QueueUserWorkItem(DisposeOnFinish);
		}

		private void DisposeOnFinish(object arg)
		{
			int state, free;
			
			do
			{
				alGetSourcei(source, AL_SOURCE_STATE, out state);
				Thread.Sleep(10);
			} while (state == AL_PLAYING);

			alSourcei(source, AL_BUFFER, 0);
			alGetSourcei(source, AL_BUFFERS_PROCESSED, out free);
			alDeleteSources(new IntPtr(1), ref source);
			alDeleteBuffers(new IntPtr(BUFFER_COUNT), buffers);

			s.TTS.RemoveActiveAudio(this);
			_resetEvent.Set();
			_resetEvent.Dispose();
			GC.KeepAlive(this);
		}

		public override void WaitToFinish()
		{
			_resetEvent.Wait();
		}
	}
}
