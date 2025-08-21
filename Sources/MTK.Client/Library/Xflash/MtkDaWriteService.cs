using System;
using System.Threading;
using System.Threading.Tasks;

namespace mtkclient.library.xflash
{
    internal class MtkDaWriteService
    {
        public static async Task WriteAsync(
            IMtkDevice device,
            uint address,
            int signatureLength,
            byte[] da,
            bool validateUploadStatus,
            CancellationToken cancellationToken
        )
        {
            cancellationToken.ThrowIfCancellationRequested();
            Console.WriteLine("Preparing da buffer");
            ushort checksum = 0;
            byte[] buffer = null;
            MtkDaWriteDataService.PrepareData(da, signatureLength, out checksum, out buffer);
            Console.WriteLine(
                "Buffer size: {0}; Signature size: {1} Checksum: 0x{2:X4}",
                buffer.Length,
                signatureLength,
                checksum
            );
            Console.WriteLine("Sending 0xD7");
            await device.EchoAsync(215, cancellationToken);
            Console.WriteLine("Sending address: 0x{0:X8}", address);
            byte[] bytes = BitConverter.GetBytes(address);
            Array.Reverse(bytes);
            await device.EchoAsync(bytes, cancellationToken);
            Console.WriteLine("Sending buffer length");
            byte[] bytes2 = BitConverter.GetBytes(buffer.Length);
            Array.Reverse(bytes2);
            await device.EchoAsync(bytes2, cancellationToken);
            Console.WriteLine("Sending signature length");
            byte[] bytes3 = BitConverter.GetBytes(signatureLength);
            Array.Reverse(bytes3);
            await device.EchoAsync(bytes3, cancellationToken);
            Console.WriteLine("Reading status");
            ushort num = await device.ReadWordAsync(little: true, cancellationToken);
            if (num == 0)
            {
                int sent = 0;
                byte[] writeBuff = new byte[64];
                Console.WriteLine("Sending data with 64 byte buffer");
                int toSend = 0;
                while (sent < buffer.Length)
                {
                    toSend = Math.Min(writeBuff.Length, buffer.Length - sent);
                    Array.Copy(buffer, sent, writeBuff, 0, toSend);
                    await device.WriteAsync(writeBuff, 0, toSend, cancellationToken);
                    sent += toSend;
                }
                Console.WriteLine("Reading checksum response");
                ushort rchecksum = await device.ReadWordAsync(little: true, cancellationToken);
                Console.WriteLine("Reading status");
                num = await device.ReadWordAsync(little: true, cancellationToken);
                if (rchecksum != checksum && rchecksum != 0)
                {
                    Console.WriteLine(
                        $"Checksum of DA upload does not match: 0x{checksum:X4} vs 0x{rchecksum:X4}"
                    );
                }
                if (validateUploadStatus && num != 0)
                {
                    Console.WriteLine($"Invalid DA upload status: 0x{num:X4}");
                }
                return;
            }
            Console.WriteLine($"Invalid status: 0x{num:X4}");
        }

        public static async Task JumpAsync(
            IMtkDevice device,
            uint address,
            CancellationToken cancellationToken
        )
        {
            cancellationToken.ThrowIfCancellationRequested();
            Console.WriteLine("Sending 0xD5");
            await device.EchoAsync(213, cancellationToken);
            //Richlog("Sending address: 0x" + address.ToString("X8"), Color.Black, false, true);
            byte[] bytes = BitConverter.GetBytes(address);
            Array.Reverse(bytes);
            await device.EchoAsync(bytes, cancellationToken);
            Console.WriteLine("Reading status");
            ushort num = await device.ReadWordAsync(little: true, cancellationToken);
            if (num != 0)
            {
                Console.WriteLine($"Invalid DA jump status: 0x{num:X4}");
            }
        }
    }
}
