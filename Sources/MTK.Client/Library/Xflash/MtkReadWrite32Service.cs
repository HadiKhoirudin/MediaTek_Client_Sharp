using System;
using System.Threading;
using System.Threading.Tasks;

namespace mtkclient.library.xflash
{
    internal class MtkReadWrite32Service
    {
        public static async Task ReadAsync(
            IMtkDevice device,
            uint address,
            int count,
            CancellationToken cancellationToken
        )
        {
            await device.EchoAsync(209, cancellationToken);
            byte[] bytes = BitConverter.GetBytes(address);
            Array.Reverse(bytes);
            await device.EchoAsync(bytes, cancellationToken);
            byte[] bytes2 = BitConverter.GetBytes(count);
            Array.Reverse(bytes2);
            await device.EchoAsync(bytes2, cancellationToken);
            ushort num = await device.ReadWordAsync(little: true, cancellationToken);
            if (num > 255 && num != 7432)
            {
                Console.WriteLine("Invalid read32 status: 0x{0:X4}", num);
                //Console.WriteLine($"Invalid read32 status: 0x{num:X4}");
            }
        }

        public static async Task<uint[]> ReadResultAsync(
            IMtkDevice device,
            uint address,
            int count,
            bool little,
            CancellationToken cancellationToken
        )
        {
            await ReadAsync(device, address, count, cancellationToken);
            uint[] result = new uint[count];
            int i = 0;
            while (i < count)
            {
                uint[] array = result;
                int num = i;
                array[num] = await device.ReadDwordAsync(little, cancellationToken);
                int num2 = i + 1;
                i = num2;
            }
            ushort num3 = await device.ReadWordAsync(little: true, cancellationToken);
            if (num3 > 255 && num3 != 7432)
            {
                Console.WriteLine("Invalid read32 value status: 0x{0:X4}", num3);
                //Console.WriteLine($"Invalid read32 value status: 0x{num3:X4}");
            }
            return result;
        }

        public static async Task WriteAsync(
            IMtkDevice device,
            uint address,
            uint value,
            bool bigEndian,
            CancellationToken cancellationToken
        )
        {
            Console.WriteLine("Sending 0xD4");
            await device.EchoAsync(212, cancellationToken);
            Console.WriteLine("Sending address: 0x{0:X8}", address);
            byte[] bytes = BitConverter.GetBytes(address);
            Array.Reverse(bytes);
            await device.EchoAsync(bytes, cancellationToken);
            Console.WriteLine("Sending length: 1");
            byte[] bytes2 = BitConverter.GetBytes(1);
            Array.Reverse(bytes2);
            await device.EchoAsync(bytes2, cancellationToken);
            Console.WriteLine("Reading status");
            ushort num = await device.ReadWordAsync(little: true, cancellationToken);
            if (num != 1)
            {
                Console.WriteLine($"Invalid write32 status: 0x{num:X4}");
            }
            Console.WriteLine("Sending value: 0x{0:X8}", value);
            byte[] bytes3 = BitConverter.GetBytes(value);
            if (bigEndian)
            {
                Array.Reverse(bytes3);
            }
            await device.EchoAsync(bytes3, cancellationToken);
            Console.WriteLine("Reading status");
            num = await device.ReadWordAsync(little: true, cancellationToken);
            if (num != 1)
            {
                Console.WriteLine($"Invalid write32 value status: 0x{num:X4}");
            }
        }
    }
}
