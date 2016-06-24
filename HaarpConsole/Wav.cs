using System;
using System.Collections.Generic;

namespace HaarpConsole
{
    static class Wav
    {
        static readonly byte[] RIFFHeader = { 0x52, 0x49, 0x46, 0x46 };
        static readonly byte[] FormatWave = { 0x57, 0x41, 0x56, 0x45 };
        static readonly byte[] FormatTag = { 0x66, 0x6d, 0x74, 0x20 };
        static readonly byte[] AudioFormat = { 0x01, 0x00 };
        static readonly byte[] SubchunkID = { 0x64, 0x61, 0x74, 0x61 };
        const int BytesPerSample = 2;

        public static byte[] PrependHeader(byte[] Data, int SampleRate, int Channels = 1)
        {
            var Ret = new List<byte>();
            int ByteRate = SampleRate * Channels * BytesPerSample;
            int BlockAlign = Channels * BytesPerSample;
            Ret.AddRange(RIFFHeader);
            Ret.AddRange(PackageInt(Data.Length + 42, 4));
            Ret.AddRange(FormatWave);
            Ret.AddRange(FormatTag);
            Ret.AddRange(PackageInt(16, 4));
            Ret.AddRange(AudioFormat);
            Ret.AddRange(PackageInt(Channels, 2));
            Ret.AddRange(PackageInt(SampleRate, 4));
            Ret.AddRange(PackageInt(ByteRate, 4));
            Ret.AddRange(PackageInt(BlockAlign, 2));
            Ret.AddRange(PackageInt(BytesPerSample * 8));
            Ret.AddRange(SubchunkID);
            Ret.AddRange(PackageInt(Data.Length, 4));

            Ret.AddRange(Data);
            return Ret.ToArray();
        }

        static byte[] PackageInt(int Src, int Len = 2)
        {
            if ((Len != 2) && (Len != 4))
                throw new ArgumentException("Length must be either 2 or 4", nameof(Len));
            var RetVal = new byte[Len];
            RetVal[0] = (byte)(Src & 0xFF);
            RetVal[1] = (byte)((Src >> 8) & 0xFF);
            if (Len == 4)
            {
                RetVal[2] = (byte)((Src >> 0x10) & 0xFF);
                RetVal[3] = (byte)((Src >> 0x18) & 0xFF);
            }
            return RetVal;
        }
    }
}