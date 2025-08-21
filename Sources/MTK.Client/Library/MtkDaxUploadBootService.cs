using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using static mtkclient.gui;

namespace mtkclient.library
{
    internal class MtkDaxUploadBootService
    {
        public static bool rebootto = false;

        public static async Task BootToAsync(
            IMtkDevice device,
            long address,
            byte[] da,
            CancellationToken cancellationToken
        )
        {
            Console.WriteLine("Sending boot command: 0x010008");

            if (rebootto)
            {
                Richlog("OK", Color.Lime, false, true);
            }

            await MtkDaxService.SendAsync(device, 65544U, cancellationToken);
            Console.WriteLine("Reading boot command status");

            if (rebootto)
            {
                Richlog("Sync Da 2            : ", Color.Black, false, false);
            }

            uint num = await MtkDaxService.ReadStatusAsync(device, cancellationToken);
            if (num != 0)
            {
                Console.WriteLine($"Invalid boot command status: 0x{num:X8}");
            }

            if (rebootto)
            {
                Richlog("OK", Color.Lime, false, true);
            }

            Console.WriteLine(
                "Sending boot parameter: address 0x{0:X16} length {1}",
                address,
                da.Length
            );

            if (rebootto)
            {
                Richlog("Sync Da 2 Extension  : ", Color.Black, false, false);
            }

            using (MemoryStream bootParamStream = new MemoryStream())
            {
                bootParamStream.Write(BitConverter.GetBytes(address));
                bootParamStream.Write(BitConverter.GetBytes((long)da.Length));
                await MtkDaxService.SendAsync(device, bootParamStream.ToArray(), cancellationToken);
                Console.WriteLine("Sending boot DA");
                await MtkDaxService.SendAsync(device, da, 64, cancellationToken);
                Console.WriteLine("Reading boot DA status");
                num = await MtkDaxService.ReadStatusAsync(device, cancellationToken);
                if (num == 0)
                {
                    Console.WriteLine("Delay for 500ms");
                    await Task.Delay(TimeSpan.FromMilliseconds(500.0));
                    Console.WriteLine("Reading boot status");
                    num = await MtkDaxService.ReadStatusAsync(device, cancellationToken);
                    Console.WriteLine("Boot status: 0x{0:X8}", num);
                    if (num != 0 && num != 1129208147)
                    {
                        Console.WriteLine($"Invalid boot status: 0x{num:X8}");
                    }
                    if (rebootto)
                    {
                        Richlog("OK", Color.Lime, false, true);
                    }
                    rebootto = true;

                    return;
                }
                Console.WriteLine($"Invalid boot DA status: 0x{num:X8}");
            }
        }

        public static async Task RebootAsync(IMtkDevice device, CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(1000));
            Console.WriteLine("Sending boot command: 0x010007");
            await MtkDaxService.SendAsync(device, 65543U, cancellationToken);
            Console.WriteLine("Reading command status");
            await Task.Delay(TimeSpan.FromMilliseconds(100));
            uint num = await MtkDaxService.ReadStatusAsync(device, cancellationToken);
            if (num == 0)
            {
                Console.WriteLine("Sending boot command param");
                using (MemoryStream payloadStream = new MemoryStream(24))
                {
                    payloadStream.Write(BitConverter.GetBytes(1));
                    payloadStream.Write(BitConverter.GetBytes(29098084));
                    payloadStream.Write(BitConverter.GetBytes(0));
                    payloadStream.Write(BitConverter.GetBytes(0));
                    payloadStream.Write(BitConverter.GetBytes(0));
                    payloadStream.Write(BitConverter.GetBytes(0));
                    await MtkDaxService.SendAsync(
                        device,
                        payloadStream.ToArray(),
                        cancellationToken
                    );
                    Console.WriteLine("Reading param status");
                    num = await MtkDaxService.ReadStatusAsync(device, cancellationToken);
                    if (num != 0)
                    {
                        Console.WriteLine($"Invalid boot command param status: 0x{num:X8}");
                    }
                    await Task.Delay(TimeSpan.FromMilliseconds(1000));
                    device?.Dispose();
                    return;
                }
            }
        }
    }
}
