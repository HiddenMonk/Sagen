#region License

// https://github.com/TheBerkin/Sagen
// 
// Copyright (c) 2017 Nicholas Fleck
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in the
// Software without restriction, including without limitation the rights to use, copy,
// modify, merge, publish, distribute, sublicense, and/or sell copies of the Software,
// and to permit persons to whom the Software is furnished to do so, subject to the
// following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A
// PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE
// OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

#endregion

using System.IO;
using System.Text;

namespace Sagen.Core.Audio
{
    internal static class Wav
    {
        private const short WAV_FORMAT_PCM = 0x0001;
        private const short WAV_FORMAT_IEEE_FLOAT = 0x0003;

        private const int FMT_CHUNK_SIZE = 18;
        private const int FACT_CHUNK_SIZE = 4;
        private const short NUM_CHANNELS = 1;
        private const short EXT_SIZE = 0;
        private static readonly byte[] CHUNK_RIFF = { 0x52, 0x49, 0x46, 0x46 };
        private static readonly byte[] ID_WAVE = { 0x57, 0x41, 0x56, 0x45 };
        private static readonly byte[] CHUNK_FMT = { 0x66, 0x6d, 0x74, 0x20 };
        private static readonly byte[] CHUNK_FACT = { 0x66, 0x61, 0x63, 0x74 };
        private static readonly byte[] CHUNK_DATA = { 0x64, 0x61, 0x74, 0x61 };

        public static void GenerateWavHeader(Synthesizer synth, Stream stream, int sampleCount, SampleFormat sampleFormat)
        {
            using (var headerStream = new MemoryStream(40))
            using (var header = new BinaryWriter(headerStream, Encoding.Default, false))
            {
                header.Write(CHUNK_RIFF);
                int posFileSize = (int)headerStream.Position;
                header.Write(0); // Replace later
                header.Write(ID_WAVE);

                // "fmt " chunk
                header.Write(CHUNK_FMT);
                header.Write(FMT_CHUNK_SIZE);
                header.Write(sampleFormat == SampleFormat.Float32 || sampleFormat == SampleFormat.Float64 ? WAV_FORMAT_IEEE_FLOAT : WAV_FORMAT_PCM);
                header.Write(NUM_CHANNELS);
                header.Write(synth.SampleRate);
                // Data rate (bytes per second), block size, and bits per sample
                int blockSize = 0;
                switch (sampleFormat)
                {
                    case SampleFormat.Float64:
                        header.Write(synth.SampleRate * sizeof(double) * NUM_CHANNELS);
                        header.Write((short)(blockSize = sizeof(double) * NUM_CHANNELS));
                        header.Write((short)(sizeof(double) * 8));
                        break;
                    case SampleFormat.Float32:
                        header.Write(synth.SampleRate * sizeof(float) * NUM_CHANNELS);
                        header.Write((short)(blockSize = sizeof(float) * NUM_CHANNELS));
                        header.Write((short)(sizeof(float) * 8));
                        break;
                    case SampleFormat.Signed16:
                        header.Write(synth.SampleRate * sizeof(short) * NUM_CHANNELS);
                        header.Write((short)(blockSize = sizeof(short) * NUM_CHANNELS));
                        header.Write((short)(sizeof(short) * 8));
                        break;
                    case SampleFormat.Unsigned8:
                        header.Write(synth.SampleRate * sizeof(byte) * NUM_CHANNELS);
                        header.Write((short)(blockSize = sizeof(byte) * NUM_CHANNELS));
                        header.Write((short)(sizeof(byte) * 8));
                        break;
                }

                header.Write(EXT_SIZE); // cbSize, required for non-PCM formats

                // "fact" chunk (required for non-PCM formats)
                if (sampleFormat == SampleFormat.Float32 || sampleFormat == SampleFormat.Float64)
                {
                    header.Write(CHUNK_FACT);
                    header.Write(FACT_CHUNK_SIZE);
                    header.Write(sampleCount);
                }

                // "data" chunk
                header.Write(CHUNK_DATA);
                int dataSize = blockSize * sampleCount;
                header.Write(dataSize);
                int dataPos = (int)headerStream.Position;
                headerStream.Position = posFileSize;
                header.Write((int)headerStream.Length + dataSize);

                // Copy the data over to the audio stream
                headerStream.Flush();
                headerStream.WriteTo(stream);
                stream.Position = dataPos;
            }
        }
    }
}