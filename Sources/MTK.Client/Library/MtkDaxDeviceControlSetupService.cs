using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace mtkclient.library
{
    internal class MtkDaxDeviceControlSetupService
    {
        public static async Task<string> GetConnectionAgentAsync(
            IMtkDevice device,
            CancellationToken cancellationToken
        )
        {
            byte[] bytes = await MtkDaxDeviceControlService.SendDevCtrlAsync(
                device,
                262154U,
                cancellationToken
            );
            return Encoding.ASCII.GetString(bytes);
        }

        public static async Task<string> GetExpireDateAsync(
            IMtkDevice device,
            CancellationToken cancellationToken
        )
        {
            byte[] bytes = await MtkDaxDeviceControlService.SendDevCtrlAsync(
                device,
                262161U,
                cancellationToken
            );
            return Encoding.ASCII.GetString(bytes);
        }

        public static async Task<string> GetUsbSpeedAsync(
            IMtkDevice device,
            CancellationToken cancellationToken
        )
        {
            byte[] bytes = await MtkDaxDeviceControlService.SendDevCtrlAsync(
                device,
                262155U,
                cancellationToken
            );
            return Encoding.ASCII.GetString(bytes);
        }

        public static async Task SendCustomAckAsync(
            IMtkDevice device,
            CancellationToken cancellationToken
        )
        {
            uint num = BitConverter.ToUInt32(
                await MtkDaxDeviceControlService.SendDevCtrlAsync(
                    device,
                    983040U,
                    cancellationToken
                ),
                0
            );
            if (num != 2711790500U)
            {
                Console.WriteLine($"Invalid custom ack response: 0x{num:X8}");
            }
        }

        public static async Task SetChecksumLevelAsync(
            IMtkDevice device,
            uint level,
            CancellationToken cancellationToken
        )
        {
            await MtkDaxDeviceControlService.SendDevCtrlAsync(
                device,
                131075U,
                BitConverter.GetBytes(level),
                cancellationToken
            );
        }

        public static async Task SetResetKeyAsync(
            IMtkDevice device,
            uint resetKey,
            CancellationToken cancellationToken
        )
        {
            await MtkDaxDeviceControlService.SendDevCtrlAsync(
                device,
                131076U,
                BitConverter.GetBytes(resetKey),
                cancellationToken
            );
        }
    }
}
