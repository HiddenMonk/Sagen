using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;

namespace HAARP
{
	internal unsafe class VoiceData
	{
		private static readonly Dictionary<string, VoiceData> voices = new Dictionary<string, VoiceData>();

		private readonly int _numSamples48k;
		private readonly int _numSamples41k;
		private readonly int _numSamples22k;
		private readonly int _numSamples11k;
		private readonly int _numSamples8k;

		private readonly float* _samples48000;
		private readonly float* _samples44100;
		private readonly float* _samples22050;
		private readonly float* _samples11025;
		private readonly float* _samples8000;

		static VoiceData()
		{
			foreach (var name in Enum.GetNames(typeof(Voice)))
			{
				voices[name] = new VoiceData(name);
			}
		}

		private VoiceData(string voiceName)
		{
			VoiceDataUtils.SetBufferPointer(ref _samples48000, ref _numSamples48k, voiceName, VoiceQuality.VeryHigh);
			VoiceDataUtils.SetBufferPointer(ref _samples44100, ref _numSamples41k, voiceName, VoiceQuality.High);
			VoiceDataUtils.SetBufferPointer(ref _samples22050, ref _numSamples22k, voiceName, VoiceQuality.Medium);
			VoiceDataUtils.SetBufferPointer(ref _samples11025, ref _numSamples11k, voiceName, VoiceQuality.Low);
			VoiceDataUtils.SetBufferPointer(ref _samples8000, ref _numSamples8k, voiceName, VoiceQuality.Awful);
#if DEBUG
			Console.WriteLine($"Voice loaded: {voiceName}");
#endif
		}

		public static VoiceData Get(Voice voice) => voices[voice.ToString()];

		public float Sample(int sampleNum, VoiceQuality quality)
		{
			switch (quality)
			{
				case VoiceQuality.VeryHigh:
					return _samples48000[sampleNum % _numSamples48k];
				case VoiceQuality.High:
					return _samples44100[sampleNum % _numSamples41k];
				case VoiceQuality.Medium:
					return _samples22050[sampleNum % _numSamples22k];
				case VoiceQuality.Low:
					return _samples11025[sampleNum % _numSamples11k];
				case VoiceQuality.Awful:
					return _samples8000[sampleNum % _numSamples8k];
				default:
					return 0f;
			}
		}

		private static class VoiceDataUtils
		{
			private static readonly Assembly a = Assembly.GetExecutingAssembly();
			
			public static void SetBufferPointer(ref float* buffer, ref int sampleCount, string voiceName, VoiceQuality quality)
			{
				using (var stream = a.GetManifestResourceStream($"HAARP.Data.{voiceName}_{Convert.ToInt32(quality, CultureInfo.InvariantCulture)}.raw") as UnmanagedMemoryStream)
				{	
					if (stream == null)
					{
#if DEBUG
						Console.WriteLine($"HAARP Warning: Voice '{voiceName}' is missing data for {quality} quality.");
#endif
						return;
					}

					buffer = (float*)stream.PositionPointer;
					sampleCount = (int)stream.Length / sizeof(float);
#if DEBUG
					Console.WriteLine($"Data Loaded: {voiceName}, {quality} ({stream.Length}B)");
#endif
				}
			}
		}
	}
}