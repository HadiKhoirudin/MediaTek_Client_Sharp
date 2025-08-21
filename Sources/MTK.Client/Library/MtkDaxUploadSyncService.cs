using mtkclient.library.xflash;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace mtkclient.library
{
    internal class MtkDaxUploadSyncService
    {
        public static async Task SyncAsync(IMtkDevice device, CancellationToken cancellationToken)
        {
            Console.WriteLine("Reading DA sync");
            byte b = await device.ReadByteAsync(cancellationToken);
            if (b != 192)
            {
                Console.WriteLine($"Invalid DA sync: 0x{b:X2}");
            }
            //Richlog("Sending DA sync: 0x434E5953", Color.Black, false, true);

            await MtkDaxService.SendAsync(device, 1129208147U, cancellationToken);
            //Richlog("Setting up DA environment", Color.Black, false, true);

            await MtkDaxUploadSetupService.SetupEnvAsync(device, cancellationToken);
            //Richlog("Setting up hardware init", Color.Black, false, true);

            await MtkDaxUploadSetupService.SetupHardwareInitAsync(device, cancellationToken);
            Console.WriteLine("Reading status");
            uint num = await MtkDaxService.ReadStatusAsync(device, cancellationToken);
            if (num != 1129208147)
            {
                Console.WriteLine($"Invalid DA sync status: 0x{num:X8}");
            }
        }
    }
}
