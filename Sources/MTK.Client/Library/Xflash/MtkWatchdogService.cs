using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static mtkclient.gui;

namespace mtkclient.library.xflash
{
    internal class MtkWatchdogService
    {
        public static async Task DisableAsync(
            IMtkDevice device,
            MtkChipConfig chipConfig,
            CancellationToken cancellationToken
        )
        {
            if (chipConfig.WdgAddress.HasValue)
            {
                uint num = MtkWatchdogValueCalculatorService.CalculateDisable(
                    chipConfig.WdgAddress.Value,
                    chipConfig.HardwareCode
                );
                Richlog("Disable Watchdog     : ", Color.Black, false, false);
                Console.WriteLine("Writing value to address 0x{0:X8}", chipConfig.WdgAddress.Value);
                await MtkReadWrite32Service.WriteAsync(
                    device,
                    chipConfig.WdgAddress.Value,
                    num,
                    bigEndian: true,
                    cancellationToken
                );
                if (chipConfig.HardwareCode == 26002)
                {
                    Console.WriteLine("Writing 0x22000000 to address 0x10000500");
                    await MtkReadWrite32Service.WriteAsync(
                        device,
                        268436736U,
                        570425344U,
                        bigEndian: true,
                        cancellationToken
                    );
                }
                else if (chipConfig.HardwareCode == 25973 || chipConfig.HardwareCode == 25975)
                {
                    Console.WriteLine("Writing 0xC0000000 to address 0x2200");
                    await MtkReadWrite32Service.WriteAsync(
                        device,
                        8704U,
                        3221225472U,
                        bigEndian: true,
                        cancellationToken
                    );
                }
                Richlog("OK", Color.Lime, false, true);
                return;
            }
            Richlog("WdgAddress is null", Color.Black, false, true);
            MessageBox.Show(
                "Device Security Is Not Supported... Please Use Other Device!",
                "Info!",
                MessageBoxButtons.OK
            );
            return;
        }
    }
}
