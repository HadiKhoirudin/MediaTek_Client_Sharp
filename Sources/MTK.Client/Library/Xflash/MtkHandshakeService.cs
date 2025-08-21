using mtkclient.MTK.Client.Scatter;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static mtkclient.gui;

namespace mtkclient.library.xflash
{
    internal class MtkHandshakeService
    {
        public static async Task<bool> DoHandshakeAsync(
            IMtkDevice device,
            CancellationToken cancellationToken
        )
        {
            byte[] SYNC = { 160, 10, 80, 5 };
            bool isBootloader = false;
            int errorCount = 0;
            int syncIndex = 0;
            while (syncIndex < SYNC.Length)
            {
                Console.WriteLine(
                    "Sending handshake: index={0}; char=0x{1:X2}",
                    syncIndex,
                    SYNC[syncIndex]
                );
                await device.WriteAsync(SYNC, syncIndex, 1, cancellationToken);
                byte b = await device.ReadByteAsync(cancellationToken);
                int num = 0;
                if (b == 82)
                {
                    Console.WriteLine("Consuming EADY");
                    byte[] eadyBuff = new byte[4];
                    await device.ReadExactAsync(eadyBuff, 0, eadyBuff.Length, cancellationToken);
                    string @string = Encoding.ASCII.GetString(eadyBuff);
                    if (@string != "EADY")
                    {
                        Console.WriteLine("Invalid sync EADY: " + @string);
                    }
                    Richlog(
                        "Retry handshake from beginning. Bootloader detected",
                        Color.Black,
                        false,
                        true
                    );
                    syncIndex = -1;
                    isBootloader = true;
                }
                else if (b != (byte)(~SYNC[syncIndex]))
                {
                    if (errorCount >= 100)
                    {
                        Console.WriteLine(
                            $"Invalid sync response at {syncIndex.ToString()}: 0x{(byte)(~SYNC[syncIndex]):X2} vs 0x{b:X2}"
                        );
                    }
                    num = errorCount + 1;
                    errorCount = num;
                    num = syncIndex - 1;
                    syncIndex = num;
                    Richlog("Handshake error count: " + errorCount, Color.Black, false, true);
                }
                num = syncIndex + 1;
                syncIndex = num;
            }
            return isBootloader;
        }

        public static async Task<MtkDeviceInfo> GetDeviceInfoAsync(
            IMtkDevice device,
            CancellationToken cancellationToken
        )
        {
            Console.WriteLine("Reading hardware code");
            await device.EchoAsync(253, cancellationToken);
            ushort hwCode = await device.ReadWordAsync(little: true, cancellationToken);

            ushort propertyValue = await device.ReadWordAsync(little: true, cancellationToken);
            Console.WriteLine("Hardware code read status: 0x" + propertyValue.ToString("X4"));
            MtkChipConfig chipConfig = MtkChipConfig.ChipConfigs
                .Where((MtkChipConfig x) => x.HardwareCode == hwCode)
                .FirstOrDefault();

            if (chipConfig != null)
            {
                Console.WriteLine("Reading software version");
                await device.EchoAsync(252, cancellationToken);
                await device.ReadWordAsync(little: false, cancellationToken);
                ushort hwVer = await device.ReadWordAsync(little: true, cancellationToken);
                ushort swVer = await device.ReadWordAsync(little: true, cancellationToken);
                ushort propertyValue2 = await device.ReadWordAsync(little: true, cancellationToken);

                if (!MtkDeviceWaiterService.reconnect)
                {
                    Richlog("CPU                  : ", Color.Black, false, false);
                    Richlog(chipConfig.Name, Color.Purple, false, true);

                    string CPU = chipConfig.Name;

                    if (CPU.Contains("/"))
                    {
                        string[] CPUs = CPU.Split("/".ToCharArray());
                        Mediatek.Platform = CPUs[0];
                    }
                    else
                    {
                        Mediatek.Platform = CPU;
                    }
                }

                if (!MtkDeviceWaiterService.reconnect)
                {
                    Richlog("Hardware code        : ", Color.Black, false, false);
                    Richlog("0x" + hwCode.ToString("X4"), Color.Purple, false, true);
                }

                if (!MtkDeviceWaiterService.reconnect)
                {
                    Richlog("Software version     : ", Color.Black, false, false);
                    Richlog("0x" + propertyValue2.ToString("X4"), Color.Purple, false, true);
                }

                Console.WriteLine("Reading security config");
                await device.EchoAsync(216, cancellationToken);
                uint secConfig = await device.ReadDwordAsync(little: true, cancellationToken);
                ushort propertyValue3 = await device.ReadWordAsync(little: true, cancellationToken);

                if (!MtkDeviceWaiterService.reconnect)
                {
                    Richlog("Security config      : ", Color.Black, false, false);
                    Richlog("0x" + propertyValue3.ToString("X4"), Color.Purple, false, true);
                }

                bool flag = Convert.ToBoolean(secConfig & 1U);
                bool flag2 = Convert.ToBoolean(secConfig & 2U);
                bool flag3 = Convert.ToBoolean(secConfig & 4U);
                bool isSecure = flag2 || flag3;
                string securityLevel = "NON_SECURE";
                if (flag || flag2 || flag3)
                {
                    if (!MtkDeviceWaiterService.reconnect)
                    {
                        Richlog("Secure boot          : ", Color.Black, false, false);
                        Richlog("True", Color.Blue, false, true);
                    }
                    List<string> list = new List<string>();
                    if (flag)
                    {
                        if (!MtkDeviceWaiterService.reconnect)
                        {
                            Richlog("SBC                  : ", Color.Black, false, false);
                            Richlog("True", Color.Blue, false, true);
                        }
                        list.Add("SBC");
                    }
                    if (flag2)
                    {
                        if (!MtkDeviceWaiterService.reconnect)
                        {
                            Richlog("SLA                  : ", Color.Black, false, false);
                            Richlog("True", Color.Blue, false, true);
                        }
                        list.Add("SLA");
                    }
                    if (flag3)
                    {
                        if (!MtkDeviceWaiterService.reconnect)
                        {
                            Richlog("SDA                  : ", Color.Black, false, false);
                            Richlog("True", Color.Blue, false, true);
                        }

                        list.Add("SDA");
                    }
                    securityLevel = string.Join("+", list);
                }
                return new MtkDeviceInfo(hwVer, swVer, isSecure, securityLevel, chipConfig);
            }
            MessageBox.Show(
                "Device Security Is Not Supported... Please Use Other Device!",
                "Info!",
                MessageBoxButtons.OK
            );
            return null;
        }
    }
}
