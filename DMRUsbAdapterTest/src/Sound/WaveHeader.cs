using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMRUsbAdapterTest.src.Sound
{
    class WaveHeader
    {


        private Int32 chunkId;
        private Int32 chunkSize;
        private Int32 format;
        private Int32 subChunk1Id;
        private Int32 subChunk1Size;
        private Int16 audioFormat;
        private Int16 numChannels;
        private Int32 sampleRate;
        private Int32 byteRate;
        private Int16 blockAlign;
        private Int16 bitsPerSample;
        private Int32 subChunk2Id;
        private Int32 subChunk2Size;




        public WaveHeader()
        {
           chunkId = 0;
           chunkSize = 0;
           format = 0;
           subChunk1Id = 0;
           subChunk1Size = 0;
           audioFormat = 0;
           numChannels = 0;
           sampleRate = 0;
           byteRate = 0;
           blockAlign = 0;
           bitsPerSample = 0;
           subChunk2Id = 0;
           subChunk2Size = 0;
        }



        public WaveHeader(byte[] header)
        {

            if (header == null)
                header = new Byte[44]; // init empty header
            if (header.Length < 44)
                header = new Byte[44];

           /* big endian */

           ChunkId |= header[3];
           ChunkId |= header[2] << 8;
           ChunkId |= header[1] << 16;
           ChunkId |= header[0] << 24;

           /* little endian */

           ChunkSize |= header[4];
           ChunkSize |= header[5] << 8;
           ChunkSize |= header[6] << 16;
           ChunkSize |= header[7] << 24;

           /* big endian */

           Format |= header[11];
           Format |= header[10] << 8;
           Format |= header[9] << 16;
           Format |= header[8] << 24;

           /* big endian */

           SubChunk1Id |= header[15];
           SubChunk1Id |= header[14] << 8;
           SubChunk1Id |= header[13] << 16;
           SubChunk1Id |= header[12] << 24;

           /* little endian */

           SubChunk1Size |= header[16];
           SubChunk1Size |= header[17] << 8;
           SubChunk1Size |= header[18] << 16;
           SubChunk1Size |= header[19] << 24;

           /* little endian */

           AudioFormat |= header[20];
           AudioFormat |= (Int16)(header[21] << 8);

           /* little endian */

           NumChannels |= (Int16)header[22];
           NumChannels |= (Int16)(header[23] << 8);


           /* little endian */

           SampleRate |= header[24];
           SampleRate |= header[25] << 8;
           SampleRate |= header[26] << 16;
           SampleRate |= header[27] << 24;

           /* little endian */

           ByteRate |= header[28];
           ByteRate |= header[29] << 8;
           ByteRate |= header[30] << 16;
           ByteRate |= header[31] << 24;

           /* little endian */

           BlockAlign |= header[32];
           BlockAlign |= (Int16)(header[33] << 8);

           /* little endian */

           BitsPerSample |= header[34];
           BitsPerSample |= (Int16) (header[35] << 8);

           /* big endian */

           SubChunk2Id |= header[39];
           SubChunk2Id |= header[38] << 8;
           SubChunk2Id |= header[37] << 16;
           SubChunk2Id |= header[36] << 24;

           /* little endian */

           SubChunk2Size |= header[40];
           SubChunk2Size |= header[41] << 8;
           SubChunk2Size |= header[42] << 16;
           SubChunk2Size |= header[43] << 24;

        }

        public Int32 ChunkId {get;set;}
        public Int32 ChunkSize { get; set; }
        public Int32 Format { get; set; }
        public Int32 SubChunk1Id { get; set; }
        public Int32 SubChunk1Size { get; set; }
        public Int16 AudioFormat {get; set; }
        public Int16 NumChannels {get{return numChannels;} set{numChannels = value;} }
        public Int32 SampleRate {get { return sampleRate; } set {sampleRate = value;} }
        public Int32 ByteRate {get { return byteRate; } set{byteRate = value; } }
        public Int16 BlockAlign { get; set; }
        public Int16 BitsPerSample { get; set; }
        public Int32 SubChunk2Id { get; set; }
        public Int32 SubChunk2Size { get; set; }
    }
}
