using Microsoft.VisualBasic;
using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace mtkclient.library
{
    class MtkSparse
    {
        private static MTK_CHUNK_HEADER chunkheader;
        private static MTK_SPARSE_HEADER sparseheader;

        public static int sectsize = 512;
        private static int totalchunk { get; set; }

        private const Int64 MTK_SPARSE_MAGIC = unchecked((int)0xEED26FF3A);
        private const Int64 MTK_SPARSE_RAW_CHUNK = 0xECAC1;
        private const Int64 MTK_SPARSE_FILL_CHUNK = 0xECAC2;
        private const Int64 MTK_SPARSE_DONT_CARE = 0xECAC3;

        public struct MTK_CHUNK_HEADER
        {
            public Int16 wChunkType;
            public Int16 wReserved;
            public Int32 dwChunkSize;
            public Int32 dwTotalSize;
        }

        public struct MTK_SPARSE_HEADER
        {
            public Int32 dwMagic; //4
            public Int16 wVerMajor; //2
            public Int16 wVerMinor; //2
            public Int16 wSparseHeaderSize; //2
            public Int16 wChunkHeaderSize; //2
            public Int32 dwBlockSize; //4
            public Int32 dwTotalBlocks; //4
            public Int32 dwTotalChunks;
            public Int32 dwImageChecksum;
        }

        public static bool CekSparse(string files)
        {
            long header_magic;
            Stream stream = File.OpenRead(files);
            stream.Seek(0L, SeekOrigin.Begin);

            byte[] buffer = new byte[1025];
            using (BinaryReader reader = new BinaryReader(stream))
            {
                reader.Read(buffer, 0, 28);
                sparseheader = parsingheader(buffer);
                var magic = sparseheader.dwMagic;
                header_magic = Convert.ToInt64(magic);
                if (header_magic == MTK_SPARSE_MAGIC)
                {
                    totalchunk = sparseheader.dwTotalChunks;
                    stream.Close();
                    reader.Close();
                    return true;
                }
                else
                {
                    stream.Close();
                    reader.Close();
                    return false;
                }
            }
        }

        public static MTK_SPARSE_HEADER parsingheader(byte[] bytes)
        {
            MTK_SPARSE_HEADER stuff = new MTK_SPARSE_HEADER();
            GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            try
            {
                stuff = (MTK_SPARSE_HEADER)
                    Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(MTK_SPARSE_HEADER));
            }
            finally
            {
                handle.Free();
            }
            return stuff;
        }

        public static void dosparse2unsparse(string filesparse, string savefile)
        {
            byte[] dataBytes = new byte[16385];

            if (!File.Exists(filesparse))
            {
                return;
            }

            FileStream Stream = new FileStream(filesparse, FileMode.Open, FileAccess.Read);
            FileStream Streamwrite = new FileStream(savefile, FileMode.Append, FileAccess.Write);
            Stream.Read(dataBytes, 0, dataBytes.Length);

            Stream.Seek(0, SeekOrigin.Begin);

            Int16 type = 0;
            double hexchunk = 0;
            int totalsizechunk = 0;
            long sizechunk = 0;
            long sectorsizeCunk = 0;
            long offsset = 0;
            long offsetdisk = 0;
            int su = 0;
            long kunyukasu = 0;
            long bytesTobeWrite = 0;
            if (totalchunk > 0)
            {
                int i = 0;
                using (BinaryReader reader = new BinaryReader(Stream))
                {
                    byte[] buffer = new byte[1025];
                    chunkheader = new MTK_CHUNK_HEADER();
                    do
                    {
                        try
                        {
                            if (i == 0)
                            {
                                reader.BaseStream.Seek(28, SeekOrigin.Begin);
                                reader.Read(buffer, 0, 12);
                                chunkheader.wChunkType = BitConverter.ToInt16(
                                    buffer.Skip(0).Take(2).ToArray(),
                                    0
                                );
                                chunkheader.dwChunkSize = BitConverter.ToInt32(
                                    buffer.Skip(4).Take(4).ToArray(),
                                    0
                                );
                                chunkheader.dwTotalSize = BitConverter.ToInt32(
                                    buffer.Skip(8).Take(4).ToArray(),
                                    0
                                );
                                type = chunkheader.wChunkType;
                                hexchunk = (
                                    Conversion.Val("&HE" + Convert.ToString(type, 16).ToUpper())
                                );
                                totalsizechunk = chunkheader.dwTotalSize;
                                sizechunk = chunkheader.dwChunkSize;
                                sectorsizeCunk = sizechunk * sparseheader.dwBlockSize;
                                offsset += chunkheader.dwTotalSize;
                                offsetdisk = 0;
                            }
                            else
                            {
                                reader.BaseStream.Seek(offsset + 28, SeekOrigin.Begin);
                                reader.Read(buffer, 0, 12);
                                chunkheader.wChunkType = BitConverter.ToInt16(
                                    buffer.Skip(0).Take(12).ToArray(),
                                    0
                                );
                                chunkheader.dwChunkSize = BitConverter.ToInt32(
                                    buffer.Skip(4).Take(4).ToArray(),
                                    0
                                );
                                chunkheader.dwTotalSize = BitConverter.ToInt32(
                                    buffer.Skip(8).Take(4).ToArray(),
                                    0
                                );
                                type = chunkheader.wChunkType;
                                hexchunk = (
                                    Conversion.Val("&HE" + Convert.ToString(type, 16).ToUpper())
                                );
                                totalsizechunk = chunkheader.dwTotalSize;
                                offsset += chunkheader.dwTotalSize;
                                sizechunk = chunkheader.dwChunkSize;
                                sectorsizeCunk = sizechunk * sparseheader.dwBlockSize;
                                offsetdisk += Convert.ToInt64(sectorsizeCunk / (double)sectsize);
                            }
                            if (hexchunk == MTK_SPARSE_RAW_CHUNK)
                            {
                                su += 1;

                                int PacketSize = 524288;
                                long NumSec = Convert.ToInt64(sectorsizeCunk / (double)sectsize);
                                long bytesWritten = 0;

                                if (sectorsizeCunk <= PacketSize)
                                {
                                    PacketSize = (int)sectorsizeCunk;
                                }
                                int OffsetStreams = 0;

                                kunyukasu += NumSec;

                                do
                                {
                                    if (sectorsizeCunk - bytesWritten < PacketSize)
                                    {
                                        PacketSize = (int)(sectorsizeCunk - bytesWritten);
                                    }
                                    if (bytesWritten == sectorsizeCunk)
                                    {
                                        break;
                                    }

                                    byte[] byt = new byte[PacketSize];
                                    reader.Read(byt, 0, PacketSize);
                                    Streamwrite.Write(byt, 0, byt.Length);
                                    bytesTobeWrite += byt.Length;
                                    bytesWritten += byt.Length;
                                    OffsetStreams += byt.Length;

                                    //Main.ProcessBar(bytesWritten, sectorsizeCunk);
                                } while (true);
                            }
                            else if (hexchunk == MTK_SPARSE_FILL_CHUNK)
                            {
                                su += 1;

                                int PacketSize = 524288;
                                long NumSec = Convert.ToInt64(sectorsizeCunk / (double)sectsize);
                                long bytesWritten = 0;

                                if (sectorsizeCunk <= PacketSize)
                                {
                                    PacketSize = (int)sectorsizeCunk;
                                }
                                int OffsetStreams = 0;

                                kunyukasu += NumSec;

                                do
                                {
                                    if (sectorsizeCunk - bytesWritten < PacketSize)
                                    {
                                        PacketSize = (int)(sectorsizeCunk - bytesWritten);
                                    }
                                    if (bytesWritten == sectorsizeCunk)
                                    {
                                        break;
                                    }

                                    byte[] byt = new byte[PacketSize];
                                    reader.Read(byt, 0, PacketSize);
                                    Streamwrite.Write(byt, 0, byt.Length);
                                    bytesTobeWrite += byt.Length;
                                    bytesWritten += byt.Length;
                                    OffsetStreams += byt.Length;

                                    //Main.ProcessBar(bytesWritten, sectorsizeCunk);
                                } while (true);
                            }
                            else if (hexchunk == MTK_SPARSE_DONT_CARE)
                            {
                                su += 1;

                                int PacketSize = 524288;
                                long NumSec = Convert.ToInt64(sectorsizeCunk / (double)sectsize);
                                long bytesWritten = 0;

                                if (sectorsizeCunk <= PacketSize)
                                {
                                    PacketSize = (int)sectorsizeCunk;
                                }
                                int OffsetStreams = 0;
                                kunyukasu += NumSec;

                                do
                                {
                                    if (sectorsizeCunk - bytesWritten < PacketSize)
                                    {
                                        PacketSize = (int)(sectorsizeCunk - bytesWritten);
                                    }
                                    if (bytesWritten == sectorsizeCunk)
                                    {
                                        break;
                                    }

                                    byte[] byt = new byte[PacketSize];
                                    reader.Read(byt, 0, PacketSize);
                                    Streamwrite.Write(byt, 0, byt.Length);
                                    bytesTobeWrite += byt.Length;
                                    bytesWritten += byt.Length;
                                    OffsetStreams += byt.Length;

                                    //Main.ProcessBar(bytesWritten, sectorsizeCunk);
                                } while (true);
                            }
                            else { }
                            i += 1;
                            if (i == totalchunk)
                            {
                                Main.ProcessBar(i, totalchunk);
                                Stream.Close();
                                reader.Close();
                                break;
                            }
                            Main.ProcessBar(i, totalchunk);
                            continue;
                        }
                        catch
                        {
                            Stream.Close();
                            break;
                        }
                    } while (true);
                }
            }
            Stream.Close();
            Streamwrite.Close();
        }
    }
}
