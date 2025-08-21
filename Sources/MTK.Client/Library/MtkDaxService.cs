using mtkclient.library.xflash;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace mtkclient.library
{
    internal class MtkDaxService
    {
        public static async Task<byte[]> ReadAsync(
            IMtkDevice device,
            CancellationToken cancellationToken
        )
        {
            uint magic = await device.ReadDwordAsync(false, cancellationToken);
            await device.ReadDwordAsync(false, cancellationToken);
            uint num = await device.ReadDwordAsync(false, cancellationToken);

            byte[] result = new byte[((int)(num - 1)) + 1];
            await device.ReadExactAsync(result, 0, (int)num, cancellationToken);
            return result;
        }

        public static async Task<uint> ReadStatusAsync(
            IMtkDevice device,
            CancellationToken cancellationToken
        )
        {
            byte[] array = await ReadAsync(device, cancellationToken);
            if (array.Length == 2)
            {
                return BitConverter.ToUInt16(array, 0);
            }
            if (array.Length < 4)
            {
                Console.WriteLine("Invalid DAX status buffer length: " + array.Length);
            }
            uint num = BitConverter.ToUInt32(array, 0);
            if (num == 4277071599U)
            {
                return 0U;
            }
            return num;
        }

        public static async Task<uint> ReadAckAsync(
            IMtkDevice device,
            CancellationToken cancellationToken
        )
        {
            await SendAsync(device, 0U, cancellationToken);
            return await ReadStatusAsync(device, cancellationToken);
        }

        public static async Task SendAsync(
            IMtkDevice device,
            byte[] data,
            int bufferSize,
            CancellationToken cancellationToken
        )
        {
            using (MemoryStream requestStream = new MemoryStream())
            {
                requestStream.Write(BitConverter.GetBytes(4277071599U));
                requestStream.Write(BitConverter.GetBytes(1));
                requestStream.Write(BitConverter.GetBytes(data.Length));
                byte[] array = requestStream.ToArray();
                await device.WriteAsync(array, 0, array.Length, cancellationToken);
                int sent = 0;
                byte[] sendBuff = new byte[bufferSize];
                int toSend = 0;
                while (sent < data.Length)
                {
                    toSend = Math.Min(sendBuff.Length, data.Length - sent);
                    System.Array.Copy(data, sent, sendBuff, 0, toSend);
                    await device.WriteAsync(sendBuff, 0, toSend, cancellationToken);
                    sent += toSend;
                }
            }
        }

        public static Task SendAsync(
            IMtkDevice device,
            byte[] data,
            CancellationToken cancellationToken
        )
        {
            return SendAsync(device, data, 512, cancellationToken);
        }

        public static Task SendAsync(
            IMtkDevice device,
            uint data,
            CancellationToken cancellationToken
        )
        {
            return SendAsync(device, BitConverter.GetBytes(data), cancellationToken);
        }
    }
}
