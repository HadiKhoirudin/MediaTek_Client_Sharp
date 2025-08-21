using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace mtkclient.library
{
    internal class MtkDaxUploadSetupService
    {
        public static async Task SetupEnvAsync(
            IMtkDevice device,
            CancellationToken cancellationToken
        )
        {
            Console.WriteLine("Sending setup env command: 0x010100");
            await MtkDaxService.SendAsync(device, 65792U, cancellationToken);
            Console.WriteLine("Sending setup env param");
            using (MemoryStream paramStream = new MemoryStream())
            {
                paramStream.Write(BitConverter.GetBytes(2));
                paramStream.Write(BitConverter.GetBytes(1));
                paramStream.Write(BitConverter.GetBytes(1));
                paramStream.Write(BitConverter.GetBytes(0));
                paramStream.Write(BitConverter.GetBytes(0));
                await MtkDaxService.SendAsync(device, paramStream.ToArray(), cancellationToken);
                Console.WriteLine("Reading setup env status");
                uint num = await MtkDaxService.ReadStatusAsync(device, cancellationToken);
                if (num != 0)
                {
                    Console.WriteLine($"Invalid setup env status: 0x{num:X8}");
                }
            }
        }

        public static async Task SetupHardwareInitAsync(
            IMtkDevice device,
            CancellationToken cancellationToken
        )
        {
            Console.WriteLine("Sending setup hardware init command: 0x010101");
            await MtkDaxService.SendAsync(device, 65793U, cancellationToken);
            Console.WriteLine("Sending setup hardware init param");
            await MtkDaxService.SendAsync(device, 0U, cancellationToken);
            Console.WriteLine("Reading setup hardware init status");
            uint num = await MtkDaxService.ReadStatusAsync(device, cancellationToken);
            if (num != 0)
            {
                Console.WriteLine($"Invalid setup hardware init status: 0x{num:X8}");
            }
        }

        public static async Task SwitchUsbSpeedAsync(
            IMtkDevice device,
            CancellationToken cancellationToken
        )
        {
            Console.WriteLine("Sending usb switch command: 0x01000B");
            await MtkDaxService.SendAsync(device, 65547U, cancellationToken);
            Console.WriteLine("Reading usb switch command status");
            uint num = await MtkDaxService.ReadStatusAsync(device, cancellationToken);
            if (num == 0)
            {
                Console.WriteLine("Sending usb switch data: 0x0E8D2001");
                await MtkDaxService.SendAsync(device, 244129793U, cancellationToken);
                Console.WriteLine("Reading usb switch data status");
                num = await MtkDaxService.ReadStatusAsync(device, cancellationToken);
                if (num != 0)
                {
                    Console.WriteLine($"Invalid usb switch data status: 0x{num:X8}");
                }
                return;
            }
            Console.WriteLine($"Invalid usb switch command status: 0x{num:X8}");
        }
    }
}
