using mtkclient.devicehandler;
using mtkclient.library.xflash;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static mtkclient.gui;

namespace mtkclient.library
{
    internal class MtkDaxUploadService
    {
        public static async Task<MtkDaxUploadResult> UploadAsync(
            MtkSerialDevice device,
            MtkChipConfig chipConfig,
            byte[] emi,
            CancellationToken cancellationToken
        )
        {
            Richlog("Load Da              : ", Color.Black, false, false);
            Richlog("MTK_AllInOne_DA_5.2228.bin ", Color.Purple, false, false);

            string daPath = Application.StartupPath + "\\loaders\\MTK_AllInOne_DA_5.2228.bin";

            using (Stream daStream = File.OpenRead(daPath))
            {
                daStream.Seek(0L, SeekOrigin.Begin);
                Console.WriteLine("Getting DA entries");
                MtkDaEntry[] source = await MtkDaService.GetEntriesAsync(daStream);
                daStream.Seek(0L, SeekOrigin.Begin);
                MtkDaEntry daEntry = source
                    .Where((MtkDaEntry x) => chipConfig.DaCode == x.HardwareCode)
                    .FirstOrDefault();
                if (daEntry != null)
                {
                    Console.WriteLine("Calculating DA");
                    byte[] stage1 = await MtkDaService.GetStage1Async(daStream, daEntry);
                    daStream.Seek(0L, SeekOrigin.Begin);
                    byte[] da = await MtkDaService.GetStage2Async(daStream, daEntry);

                    MtkDaxUploadCalculationResult daStages =
                        MtkDaxUploadCalculatorService.Calculate(
                            stage1,
                            da,
                            daEntry.Regions[2].StartAddress,
                            daEntry.Regions[2].SignatureLength
                        );
                    Richlog("OK", Color.Lime, false, true);

                    Richlog("Sending stage 1 DA   : ", Color.Black, false, false);
                    await MtkDaWriteService.WriteAsync(
                        device,
                        daEntry.Regions[1].StartAddress,
                        daEntry.Regions[1].SignatureLength,
                        daStages.Da1,
                        validateUploadStatus: true,
                        cancellationToken
                    );
                    Richlog("OK", Color.Lime, false, true);

                    Richlog("Jumping to 0x:200000 : ", Color.Black, false, false);
                    await MtkDaWriteService.JumpAsync(
                        device,
                        daEntry.Regions[1].StartAddress,
                        cancellationToken
                    );

                    Richlog("OK", Color.Lime, false, true);

                    Richlog("Sync Da	             : ", Color.Black, false, false);
                    await MtkDaxUploadSyncService.SyncAsync(device, cancellationToken);

                    Console.WriteLine("Getting expire date");
                    string propertyValue = await MtkDaxDeviceControlSetupService.GetExpireDateAsync(
                        device,
                        cancellationToken
                    );
                    Richlog("OK", Color.Lime, false, true);

                    Console.WriteLine("Expire date: " + propertyValue);
                    Console.WriteLine("Setting reset key: 0x68");
                    await MtkDaxDeviceControlSetupService.SetResetKeyAsync(
                        device,
                        104U,
                        cancellationToken
                    );
                    Console.WriteLine("Setting checksum level: 0");
                    await MtkDaxDeviceControlSetupService.SetChecksumLevelAsync(
                        device,
                        0U,
                        cancellationToken
                    );
                    Console.WriteLine("Getting connection agent");
                    string text = await MtkDaxDeviceControlSetupService.GetConnectionAgentAsync(
                        device,
                        cancellationToken
                    );
                    Richlog("Con MODE             : ", Color.Black, false, false);
                    Richlog(text, Color.Purple, false, true);
                    if (text == "brom")
                    {
                        await MtkDaxUploadEmiService.UploadEmiAsync(device, emi, cancellationToken);
                    }
                    else if (text != "preloader")
                    {
                        Console.WriteLine("Invalid connection agent: " + text);
                    }

                    //Richlog(
                    //    "Booting to stage 2 address: 0x{0:X8}"
                    //        + daEntry.Regions[2].StartAddress.ToString("X8"),
                    //    Color.Black,
                    //    false,
                    //    true
                    //);

                    await MtkDaxUploadBootService.BootToAsync(
                        device,
                        daEntry.Regions[2].StartAddress,
                        daStages.Da2,
                        cancellationToken
                    );
                    Console.WriteLine("Getting usb speed");
                    string text2 = await MtkDaxDeviceControlSetupService.GetUsbSpeedAsync(
                        device,
                        cancellationToken
                    );
                    Richlog("Con USB              : ", Color.Black, false, false);
                    Richlog(text2, Color.Purple, false, true);

                    bool switched = false;
                    //if (text2 == "full-speed")
                    //{

                    try
                    {
                        Richlog(" ", Color.Black, false, true);

                        MtkDaxFlashInfo flashInfo2 =
                            await MtkDaxDeviceStorageInfoService.GetStorageInfoAsync(
                                device,
                                cancellationToken
                            );
                        Richlog(" ", Color.Black, false, true);

                        Richlog("Sending Da 2         : ", Color.Black, false, false);
                        await MtkDaxUploadBootService.BootToAsync(
                            device,
                            1744830464L,
                            daStages.Extension,
                            cancellationToken
                        );

                        Console.WriteLine("Sending custom ack");
                        await MtkDaxDeviceControlSetupService.SendCustomAckAsync(
                            device,
                            cancellationToken
                        );
                        Console.WriteLine("Getting packet length");
                        PartitionPacketLength partitionPacketLength =
                            await MtkDaxPartitionPacketLengthService.GetAsync(
                                device,
                                cancellationToken
                            );
                        MtkDaxFlashInfo mtkDaxFlashInfo = flashInfo2._get();
                        mtkDaxFlashInfo.WriteBufferSize = partitionPacketLength.WriteLen;
                        mtkDaxFlashInfo.ReadBufferSize = partitionPacketLength.ReadLen;
                        flashInfo2 = mtkDaxFlashInfo;
                        if (Main.SharedUI.CkAutoSwitchHighSpeedUSB.Checked)
                        {
                            Richlog(" ", Color.Black, false, true);
                            Richlog("Switch to High Speed : ", Color.Black, false, false);
                            await MtkDaxUploadSetupService.SwitchUsbSpeedAsync(
                                device,
                                cancellationToken
                            );
                            Richlog("OK", Color.Lime, false, true);
                        }
                        Console.WriteLine("Write buffer size : " + mtkDaxFlashInfo.WriteBufferSize);
                        Console.WriteLine("Read buffer size : " + mtkDaxFlashInfo.ReadBufferSize);
                        device.Dispose();
                        Richlog(" ", Color.Black, false, true);
                        await Task.Delay(TimeSpan.FromSeconds(2.0));
                        device = (MtkSerialDevice)
                            (IMtkSerialDevice)(
                                (
                                    await MtkDeviceWaiterService.WaitSerialAsync(
                                        doHandshake: false,
                                        cancellationToken
                                    )
                                ).Device
                            );

                        switched = true;
                        //}

                        Main.SharedUI.CkBromReady.Invoke(
                            (Action)(() => Main.SharedUI.CkBromReady.Checked = true)
                        );

                        return new MtkDaxUploadResult(device, flashInfo2);
                    }
                    catch
                    {
                        if (switched)
                        {
                            //Richlog(
                            //    "Disconnecting from switched serial device",
                            //    Color.Black,
                            //    false,
                            //    true
                            //);
                            device?.Dispose();
                        }
                        throw;
                    }
                }
                Richlog(
                    "DA code not found: 0x" + chipConfig.DaCode.ToString("X8"),
                    Color.Black,
                    false,
                    true
                );
                MessageBox.Show(
                    "Device Security Is Not Supported... Please Use Other Device!",
                    "Info!",
                    MessageBoxButtons.OK
                );
                return null;
            }
        }
    }
}
