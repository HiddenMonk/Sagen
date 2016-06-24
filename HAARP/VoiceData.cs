using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;

namespace HAARP
{
	internal class VoiceData
	{
		private static readonly Dictionary<string, VoiceData> voices = new Dictionary<string, VoiceData>();

		private readonly float[] _samples48000;
		private readonly float[] _samples44100;
		private readonly float[] _samples22050;
		private readonly float[] _samples11025;
		private readonly float[] _samples8000;

		static VoiceData()
		{
			foreach (var name in Enum.GetNames(typeof(Voice)))
			{
				voices[name] = new VoiceData(name);
			}
		}

		private VoiceData(string voiceName)
		{
			Utils.FillBuffer(ref _samples48000, voiceName, VoiceQuality.VeryHigh);
			Utils.FillBuffer(ref _samples44100, voiceName, VoiceQuality.High);
			Utils.FillBuffer(ref _samples22050, voiceName, VoiceQuality.Medium);
			Utils.FillBuffer(ref _samples11025, voiceName, VoiceQuality.Low);
			Utils.FillBuffer(ref _samples8000, voiceName, VoiceQuality.Awful);
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
					return _samples48000[sampleNum % _samples48000.Length];
				case VoiceQuality.High:
					return _samples44100[sampleNum % _samples44100.Length];
				case VoiceQuality.Medium:
					return _samples22050[sampleNum % _samples22050.Length];
				case VoiceQuality.Low:
					return _samples11025[sampleNum % _samples11025.Length];
				case VoiceQuality.Awful:
					return _samples8000[sampleNum % _samples8000.Length];
				default:
					return 0f;
			}
		}

		private static class Utils
		{
			private static readonly Assembly a = Assembly.GetExecutingAssembly();

			public static void FillBuffer(ref float[] buffer, string voiceName, VoiceQuality quality)
			{
				using (var stream = a.GetManifestResourceStream($"HAARP.Data.{voiceName}_{Convert.ToInt32(quality, CultureInfo.InvariantCulture)}.raw"))
				{
					if (stream == null)
					{
#if DEBUG
						Console.WriteLine($"HAARP Warning: Voice '{voiceName}' is missing data for {quality} quality.");
#endif
						return;
					}

					using (var reader = new BinaryReader(stream, Encoding.Default, true))
					{
						buffer = new float[stream.Length / 4];
						int i = 0;
						while (stream.Position < stream.Length)
						{
							buffer[i++] = reader.ReadSingle();
						}
					}
#if DEBUG
					Console.WriteLine($"Data Loaded: {voiceName}, {quality} ({stream.Length}B)");
#endif
				}
			}
		}
	}
}