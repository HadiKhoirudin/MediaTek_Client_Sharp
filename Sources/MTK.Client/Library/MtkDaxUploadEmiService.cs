using System;
using System.Threading;
using System.Threading.Tasks;

namespace mtkclient.library
{
    internal class MtkDaxUploadEmiService
    {
        public static async Task UploadEmiAsync(
            IMtkDevice device,
            byte[] emi,
            CancellationToken cancellationToken
        )
        {
            Console.WriteLine("Sending upload emi command: 0x01000A");
            await MtkDaxService.SendAsync(device, 65546U, cancellationToken);
            Console.WriteLine("Reading upload emi command status");
            uint num = await MtkDaxService.ReadStatusAsync(device, cancellationToken);
            if (num == 0)
            {
                Console.WriteLine("Sending emi length: {0}", emi.Length);
                await MtkDaxService.SendAsync(device, (uint)emi.Length, cancellationToken);
                Console.WriteLine("Uploading emi");
                await MtkDaxService.SendAsync(device, emi, cancellationToken);
                Console.WriteLine("Reading upload emi status");
                num = await MtkDaxService.ReadStatusAsync(device, cancellationToken);
                if (num != 0)
                {
                    Console.WriteLine($"Invalid emi upload status: 0x{num:X8}");
                }
                return;
            }
            Console.WriteLine($"Invalid emi command status: 0x{num:X8}");
        }
    }
}
