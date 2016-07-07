using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace Sagen
{
	internal sealed unsafe class AudioStream : IDisposable
	{
		private static readonly HashSet<AudioStream> _activeStreams = new HashSet<AudioStream>();

		private bool _disposed, _fullyQueued;
		private int _queueSize = 0;

		private readonly IntPtr waveOutDevicePtr;
		private static readonly IntPtr WAVE_MAPPER = new IntPtr(-1);
		private const short WAVE_FORMAT_PCM = 0x0001;

		private delegate void WaveOutProcCallback(IntPtr hWaveOut, WaveOutMessage message, IntPtr dwInstance, IntPtr dwParam1, IntPtr dwParam2);

		[DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Auto)]
		private static extern MMRESULT waveOutOpen(ref IntPtr hWaveOut, IntPtr uDeviceID, ref WAVEFORMATEX lpFormat,
			[MarshalAs(UnmanagedType.FunctionPtr)] WaveOutProcCallback dwOutProcCallback, IntPtr dwInstance, WaveOutOpenFlags dwFlags);

		[DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Auto)]
		private static extern MMRESULT waveOutClose(IntPtr hwo);

		[DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Auto)]
		private static extern MMRESULT waveOutPrepareHeader(IntPtr hWaveOut, IntPtr pwh, int uSize);

		[DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Auto)]
		private static extern MMRESULT waveOutUnprepareHeader(IntPtr hwo, IntPtr pwh, int cbwh);

		[DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Auto)]
		private static extern MMRESULT waveOutWrite(IntPtr hwo, IntPtr pwh, int cbwh);

		public AudioStream(SampleFormat format, Synthesizer synth)
		{
			MMRESULT result;
			var fmt = CreateFormatSpec(format, synth.SampleRate);

			// Create output device
			if ((result = waveOutOpen(ref waveOutDevicePtr, WAVE_MAPPER, ref fmt, WaveOutProc, IntPtr.Zero, WaveOutOpenFlags.CALLBACK_FUNCTION)) != MMRESULT.MMSYSERR_NOERROR)
				throw new ExternalException($"Function 'waveOutOpen' returned error code {result}");

			_activeStreams.Add(this);
		}

		public void MarkFullyQueued()
		{
			_fullyQueued = true;
			if (_queueSize == 0) _activeStreams.Remove(this);
		}

		public void QueueDataBlock(Stream stream)
		{
			// Reset the stream position and keep track of the previous position.
			// The former position tells how much data we need to copy.
			stream.Flush();
			int l = (int)stream.Position;
			stream.Position = 0;

			// Allocate unmanaged memory to store samples from stream
			var ptrData = Marshal.AllocHGlobal(l);

			// Populate a WAVEHDR with samples
			WAVEHDR hdr;
			using (var ums = new UnmanagedMemoryStream((byte*)ptrData.ToPointer(), l, l, FileAccess.Write))
			{
				stream.CopyTo(ums, l);
				stream.Position = 0;
				ums.Position = 0;
				hdr = new WAVEHDR
				{
					BufferLength = (uint)l,
					Data = new IntPtr(ums.PositionPointer)
				};
			}

			// Copy WAVEHDR instance to unmanaged space
			var ptrHdr = Marshal.AllocHGlobal(sizeof(WAVEHDR));
			Marshal.StructureToPtr(hdr, ptrHdr, false);

			// Prepare the header and queue it
			MMRESULT result;
			if ((result = waveOutPrepareHeader(waveOutDevicePtr, ptrHdr, sizeof(WAVEHDR))) != MMRESULT.MMSYSERR_NOERROR)
				throw new ExternalException($"Function 'waveOutPrepareHeader' returned error code {result}");
			if ((result = waveOutWrite(waveOutDevicePtr, ptrHdr, sizeof(WAVEHDR))) != MMRESULT.MMSYSERR_NOERROR)
				throw new ExternalException($"Function 'waveOutWrite' returned error code {result}");

			// Increment queue size
			_queueSize++;
		}

		private void WaveOutProc(IntPtr hWaveOut, WaveOutMessage message, IntPtr dwInstance, IntPtr dwParam1, IntPtr dwParam2)
		{
			MMRESULT result;
			switch (message)
			{
				case WaveOutMessage.WOM_DONE:
					{
						// Remove data block from device so it can be freed
						var hdr = (WAVEHDR)Marshal.PtrToStructure(dwParam1, typeof(WAVEHDR));
						if ((result = waveOutUnprepareHeader(hWaveOut, dwParam1, sizeof(WAVEHDR))) != MMRESULT.MMSYSERR_NOERROR)
							throw new ExternalException($"Function 'waveOutUnprepareHeader' returned error code {result}");

						// Free memory used by WAVEHDR and samples
						Marshal.FreeHGlobal(hdr.Data);
						Marshal.FreeHGlobal(dwParam1);

						// Decrease queue size and check if audio is no longer being streamed
						_queueSize--;
						if (_fullyQueued && _queueSize <= 0)
						{
							Dispose();
							_activeStreams.Remove(this);
						}
						break;
					}
			}
		}

		private static WAVEFORMATEX CreateFormatSpec(SampleFormat format, int sampleRate)
		{
			return new WAVEFORMATEX
			{
				FormatTag = WAVE_FORMAT_PCM,
				Channels = 1,
				SamplesPerSec = (uint)sampleRate,
				BitsPerSample = (short)format,
				AvgBytesPerSec = (uint)(((int)format / 8) * sampleRate),
				BlockAlign = (short)((int)format / 8),
				cbSize = 0
			};
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		private struct WAVEHDR
		{
			public IntPtr Data;
			public uint BufferLength;
			public uint BytesRecorded;
			public IntPtr User;
			public WaveHdrFlags Flags;
			public uint Loops;
			private IntPtr Next;
			private IntPtr Reserved;
		}

		[Flags]
		private enum WaveHdrFlags : uint
		{
			WHDR_DONE = 1,
			WHDR_PREPARED = 2,
			WHDR_BEGINLOOP = 4,
			WHDR_ENDLOOP = 8,
			WHDR_INQUEUE = 16
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct WAVEFORMATEX
		{
			public short FormatTag;
			public short Channels;
			public uint SamplesPerSec;
			public uint AvgBytesPerSec;
			public short BlockAlign;
			public short BitsPerSample;
			public short cbSize;
		}

		private enum MMRESULT : uint
		{
			MMSYSERR_NOERROR = 0,
			MMSYSERR_ERROR = 1,
			MMSYSERR_BADDEVICEID = 2,
			MMSYSERR_NOTENABLED = 3,
			MMSYSERR_ALLOCATED = 4,
			MMSYSERR_INVALHANDLE = 5,
			MMSYSERR_NODRIVER = 6,
			MMSYSERR_NOMEM = 7,
			MMSYSERR_NOTSUPPORTED = 8,
			MMSYSERR_BADERRNUM = 9,
			MMSYSERR_INVALFLAG = 10,
			MMSYSERR_INVALPARAM = 11,
			MMSYSERR_HANDLEBUSY = 12,
			MMSYSERR_INVALIDALIAS = 13,
			MMSYSERR_BADDB = 14,
			MMSYSERR_KEYNOTFOUND = 15,
			MMSYSERR_READERROR = 16,
			MMSYSERR_WRITEERROR = 17,
			MMSYSERR_DELETEERROR = 18,
			MMSYSERR_VALNOTFOUND = 19,
			MMSYSERR_NODRIVERCB = 20,
			WAVERR_BADFORMAT = 32,
			WAVERR_STILLPLAYING = 33,
			WAVERR_UNPREPARED = 34
		}

		private enum WaveOutMessage : uint
		{
			WOM_OPEN = 0x3bc,
			WOM_DONE = 0x3bd,
			WOM_CLOSE = 0x3bb
		}

		[Flags]
		private enum WaveOutOpenFlags : uint
		{
			CALLBACK_NULL = 0,
			CALLBACK_FUNCTION = 0x30000,
			CALLBACK_EVENT = 0x50000,
			CALLBACK_WINDOW = 0x10000,
			CALLBACK_THREAD = 0x20000,
			WAVE_FORMAT_QUERY = 1,
			WAVE_MAPPED = 4,
			WAVE_FORMAT_DIRECT = 8
		}

		~AudioStream()
		{
			Dispose();
		}

		public void Dispose()
		{
			if (_disposed) return;
			MMRESULT result;
			if ((result = waveOutClose(waveOutDevicePtr)) != MMRESULT.MMSYSERR_NOERROR)
				throw new ExternalException($"Function 'waveOutClose' returned error code {result}");
			_disposed = true;
		}
	}
}