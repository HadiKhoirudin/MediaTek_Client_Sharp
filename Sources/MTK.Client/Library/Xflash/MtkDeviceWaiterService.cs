using mtkclient.devicehandler;
using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static mtkclient.gui;

namespace mtkclient.library.xflash
{
    internal class MtkDeviceWaiterService
    {
        public static bool reconnect = false;

        public static async Task<MtkDeviceWaitResult> WaitSerialAsync(
            bool doHandshake,
            CancellationToken cancellationToken
        )
        {
            int current = -1;
            do
            {
                cancellationToken.ThrowIfCancellationRequested();

                int num = current + 1;
                current = num;
                if (current != 180)
                {
                    IMtkSerialDevice[] devices = await MtkSerialDeviceFinderService.FindAsync();
                    if (devices.Length != 0)
                    {
                        if (devices.Length <= 1)
                        {
                            if (!reconnect)
                            {
                                Richlog(
                                    "Connecting to mtk serial device : "
                                        + ((object)devices[0]).ToString(),
                                    Color.Black,
                                    false,
                                    true
                                );
                            }

                            try
                            {
                                await devices[0].ConnectAsync();
                            }
                            catch
                            {
                                devices[0].Dispose();
                                Console.WriteLine("Error connecting to mtk serial device");
                                Thread.Sleep(1000);
                                continue;
                            }
                            bool isBootloader = false;
                            MtkDeviceInfo deviceInfo = null;
                            if (doHandshake)
                            {
                                try
                                {
                                    isBootloader = await MtkHandshakeService.DoHandshakeAsync(
                                        devices[0],
                                        cancellationToken
                                    );

                                    if (!reconnect)
                                    {
                                        Richlog(" ", Color.Black, false, true);
                                        Richlog(
                                            "Send Boot Sequence   : ",
                                            Color.Black,
                                            false,
                                            false
                                        );
                                        Richlog("OK", Color.Lime, false, true);
                                    }
                                    deviceInfo = await MtkHandshakeService.GetDeviceInfoAsync(
                                        devices[0],
                                        cancellationToken
                                    );

                                    reconnect = true;
                                }
                                catch
                                {
                                    devices[0].Dispose();
                                    Richlog("Error doing mtk handshake", Color.Black, false, true);
                                    Thread.Sleep(1000);

                                    cancellationToken.ThrowIfCancellationRequested();
                                    continue;
                                }
                            }
                            else
                            {
                                isBootloader = false;
                                deviceInfo = new MtkDeviceInfo(
                                    0U,
                                    0U,
                                    IsSecure: false,
                                    "",
                                    new MtkChipConfig()
                                );
                            }
                            return new MtkDeviceWaitResult(devices[0], isBootloader, deviceInfo);
                        }
                        IMtkSerialDevice[] array = devices;
                        for (num = 0; num < array.Length; num++)
                        {
                            array[num].Dispose();
                        }
                        Richlog(
                            "Multiple mtk serial devices found. Retrying",
                            Color.Black,
                            false,
                            true
                        );
                        //if (
                        //    (
                        //        await m_dialogService.ShowAsync(
                        //            "confirmation",
                        //            "connect_only_one_device",
                        //            new MessageDialogAction[2]
                        //            {
                        //                new MessageDialogAction { Name = "ok" },
                        //                new MessageDialogAction { Name = "cancel" }
                        //            }
                        //        )
                        //    ).Name == "cancel"
                        //)
                        //{
                        //    break;
                        //}
                        Thread.Sleep(1000);
                    }
                    else
                    {
                        Thread.Sleep(1000);
                    }
                    continue;
                }
                MessageBox.Show("Timeout has reached...", "Info!", MessageBoxButtons.OK);
                return null;
            } while (true);
            MessageBox.Show("Task Canceled!", "Info!", MessageBoxButtons.OK);
            return null;
        }

        public static async Task<MtkDeviceWaitResult> WaitUsbAsync(
            bool doHandshake,
            CancellationToken cancellationToken
        )
        {
            int current = -1;
            IMtkUsbDevice[] devices = null;
            bool isBootloader = false;
            MtkDeviceInfo deviceInfo = null;
            do
            {
                cancellationToken.ThrowIfCancellationRequested();
                int num = current + 1;
                current = num;
                if (current != 180)
                {
                    devices = await MtkUsbDeviceFinderService.FindAsync();
                    if (devices.Length == 0)
                    {
                        Thread.Sleep(1000);
                        continue;
                    }
                    if (devices.Length > 1)
                    {
                        IMtkUsbDevice[] array = devices;
                        for (num = 0; num < array.Length; num++)
                        {
                            array[num].Dispose();
                        }
                        Richlog(
                            "Multiple mtk usb devices found. Retrying",
                            Color.Black,
                            false,
                            true
                        );
                        //if (
                        //    (
                        //        await m_dialogService.ShowAsync(
                        //            "confirmation",
                        //            "connect_only_one_device",
                        //            new MessageDialogAction[2]
                        //            {
                        //                new MessageDialogAction { Name = "ok" },
                        //                new MessageDialogAction { Name = "cancel" }
                        //            }
                        //        )
                        //    ).Name == "cancel"
                        //)
                        //{
                        //    throw new TaskCanceledException();
                        //}
                        Thread.Sleep(1000);

                        continue;
                    }
                    if (!reconnect)
                    {
                        Richlog(
                            "Connecting to mtk usb device : " + ((object)devices[0]).ToString(),
                            Color.Black,
                            false,
                            true
                        );
                    }
                    try
                    {
                        await devices[0].ConnectAsync();
                    }
                    catch
                    {
                        devices[0].Dispose();
                        Richlog("Error connecting to mtk usb device", Color.Black, false, true);
                        Thread.Sleep(1000);
                        continue;
                    }
                    if (doHandshake)
                    {
                        try
                        {
                            isBootloader = await MtkHandshakeService.DoHandshakeAsync(
                                devices[0],
                                cancellationToken
                            );

                            if (!reconnect)
                            {
                                Richlog("Gettting mtk device info", Color.Black, false, true);
                            }

                            deviceInfo = await MtkHandshakeService.GetDeviceInfoAsync(
                                devices[0],
                                cancellationToken
                            );
                        }
                        catch
                        {
                            devices[0].Dispose();
                            Richlog("Error doing mtk handshake", Color.Black, false, true);
                            Thread.Sleep(1000);
                            continue;
                        }
                    }
                    else
                    {
                        isBootloader = false;
                        deviceInfo = new MtkDeviceInfo(
                            0U,
                            0U,
                            IsSecure: false,
                            "",
                            new MtkChipConfig()
                        );
                    }
                    break;
                }
                MessageBox.Show("Timeout has reached...", "Info!", MessageBoxButtons.OK);
                return null;
            } while (true);
            return new MtkDeviceWaitResult(devices[0], isBootloader, deviceInfo);
        }
    }
}
