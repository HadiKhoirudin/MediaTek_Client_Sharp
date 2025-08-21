using System;
using System.Collections.Generic;
using System.Linq;

namespace mtkclient.library.xflash
{
    internal class MtkDaWriteDataService
    {
        public static void PrepareData(
            byte[] da,
            int signatureLength,
            out ushort checksum,
            out byte[] buffer
        )
        {
            checksum = 0;
            buffer = da;
            if (buffer.Length % 2 != 0)
            {
                buffer = ((IEnumerable<byte>)buffer).Append((byte)0).ToArray();
            }
            int i = 0;
            while (i < buffer.Length)
            {
                checksum = (ushort)(checksum ^ BitConverter.ToUInt16(buffer, i));
                i += 2;
            }
            if (((uint)buffer.Length & (true ? 1U : 0U)) != 0)
            {
                checksum = (ushort)(checksum ^ buffer.Last());
            }
        }
    }
}
