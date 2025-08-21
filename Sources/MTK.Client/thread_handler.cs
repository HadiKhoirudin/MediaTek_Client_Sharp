using mtkclient.devicehandler;
using mtkclient.library;
using mtkclient.library.xflash;
using mtkclient.MTK.Client.Scatter;
using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static mtkclient.android;
using static mtkclient.gui;
//using static mtkclient.library.xflash.MtkCustomWrite;

using static mtkclient.MTK.Client.utils;

namespace mtkclient.Tasks
{
    internal class MtkTask
    {
        #region Disable Sleep
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);

        // Constants for EXECUTION_STATE
        public enum EXECUTION_STATE : uint
        {
            ES_SYSTEM_REQUIRED = 0x1,
            ES_DISPLAY_REQUIRED = 0x2,
            ES_CONTINUOUS = 0x80000000U
        }

        // Method to prevent Windows from entering sleep mode
        public static void PreventSleep()
        {
            SetThreadExecutionState(
                EXECUTION_STATE.ES_CONTINUOUS
                    | EXECUTION_STATE.ES_SYSTEM_REQUIRED
                    | EXECUTION_STATE.ES_DISPLAY_REQUIRED
            );
        }

        // Method to allow Windows to enter sleep mode
        public static void AllowSleep()
        {
            SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS);
        }
        #endregion

        private static MtkDaxUploadResult uploadResult;
        private static MtkPreloader preloader;
        private static MtkDeviceWaitResult mtkWaitResult;
        public static MtkGpt gpt { get; set; }
        public static string storagetype;

        public static async Task InitAsync(CancellationToken cancelToken)
        {
            cancelToken.ThrowIfCancellationRequested();
            Richlog("Waiting for mtk serial device", Color.Black, false, true);
            MtkDaxUploadBootService.rebootto = false;
            MtkDeviceWaiterService.reconnect = false;
            mtkWaitResult = await MtkDeviceWaiterService.WaitSerialAsync(true, cancelToken);
            try
            {
                if (Main.SharedUI.CkAutoCrashPreloader.Checked)
                {
                    if (mtkWaitResult.IsBootloader)
                    {
                        Richlog("Crashing mtk bootloader", Color.Black, false, true);
                        await MtkBootloaderCrashService.CrashAsync(
                            mtkWaitResult.Device,
                            cancelToken
                        );
                        Richlog("Disconnecting from mtk serial device", Color.Black, false, true);
                        mtkWaitResult.Device.Dispose();
                        mtkWaitResult = null;
                        Richlog("reconnecting", Color.Black, false, true);
                        Richlog("Waiting for mtk serial device", Color.Black, false, true);
                        mtkWaitResult = await MtkDeviceWaiterService.WaitSerialAsync(
                            true,
                            cancelToken
                        );
                        if (mtkWaitResult.IsBootloader)
                        {
                            Richlog(
                                "Mtk device is still in bootloader mode",
                                Color.Black,
                                false,
                                true
                            );
                            MessageBox.Show(
                                "Device Security Not Supported",
                                "Info!",
                                MessageBoxButtons.OK
                            );
                            return;
                        }
                    }
                }

                Console.WriteLine("Disabling watchdog");
                await MtkWatchdogService.DisableAsync(
                    mtkWaitResult.Device,
                    mtkWaitResult.DeviceInfo.ChipConfig,
                    cancelToken
                );

                if (mtkWaitResult.DeviceInfo.IsSecure)
                {
                    Console.WriteLine("Disconnecting from mtk serial device");
                    mtkWaitResult.Device.Dispose();
                    MtkDeviceInfo serialDeviceInfo = mtkWaitResult.DeviceInfo;
                    mtkWaitResult = null;
                    Richlog(" ", Color.Black, false, true);
                    Richlog("Device is Protected  !", Color.Black, false, true);
                    Richlog("Trying to Bypass     : ", Color.Black, false, false);
                    mtkWaitResult = await MtkDeviceWaiterService.WaitUsbAsync(false, cancelToken);
                    await MtkAuthExploitService.ExploitAsync(
                        (IMtkUsbDevice)mtkWaitResult.Device,
                        serialDeviceInfo.ChipConfig,
                        cancelToken
                    );
                    Richlog("OK", Color.Lime, false, true);
                    Richlog("Re HandShake         : ", Color.Black, false, false);
                    mtkWaitResult.Device.Dispose();

                    if (Main.SharedUI.CkBypassSecureBootOnly.Checked)
                    {
                        Richlog("OK", Color.Lime, false, true);
                        Richlog("Security Bypass      : ", Color.Black, false, false);
                        Richlog("OK", Color.Lime, false, true);
                        Richlog(" ", Color.Lime, false, true);
                        Richlog(
                            "Please Use UART Connection Settings From SP Flash Tool Using Desired Port Number...",
                            Color.Black,
                            false,
                            true
                        );
                        Richlog(" ", Color.Lime, false, true);

                        MtkDaxUploadBootService.rebootto = false;
                        MtkDeviceWaiterService.reconnect = false;
                        return;
                    }

                    mtkWaitResult = null;
                    mtkWaitResult = await MtkDeviceWaiterService.WaitSerialAsync(true, cancelToken);

                    if (mtkWaitResult.DeviceInfo.IsSecure)
                    {
                        Richlog("Mtk device is still secure", Color.Black, false, true);
                        MessageBox.Show(
                            "Device Security Not Supported",
                            "Info!",
                            MessageBoxButtons.OK
                        );
                        return;
                    }

                    Richlog("OK", Color.Lime, false, true);
                }
                if (File.Exists(Mediatek.Preloader))
                {
                    FileStream fr = new FileStream(
                        Mediatek.Preloader,
                        FileMode.Open,
                        FileAccess.Read
                    );

                    preloader = await MtkPreloaderService.LoadAsync(
                        fr,
                        mtkWaitResult.DeviceInfo.ChipConfig,
                        cancelToken
                    );

                    fr.Close();
                }
                else
                {
                    string save_preloader = Application.StartupPath + "\\preloaders";

                    if (!Directory.Exists(save_preloader))
                    {
                        Directory.CreateDirectory(save_preloader);
                    }

                    Richlog("Dumping Preloader    : ", Color.Black, false, false);
                    preloader = await MtkPreloaderService.DumpAsync(
                        mtkWaitResult.Device,
                        mtkWaitResult.DeviceInfo.ChipConfig,
                        cancelToken
                    );
                    Richlog("OK", Color.Lime, false, true);

                    Richlog(
                        "Preloader saved in   : ..\\preloaders\\" + preloader.Name,
                        Color.Black,
                        false,
                        true
                    );

                    if (!File.Exists(save_preloader + "\\" + preloader.Name))
                    {
                        File.WriteAllBytes(save_preloader + "\\" + preloader.Name, preloader.Data);
                    }
                }

                Richlog(" ", Color.Black, false, true);
                Console.WriteLine("Uploading DA");
                uploadResult = await MtkDaxUploadService.UploadAsync(
                    (MtkSerialDevice)(IMtkSerialDevice)mtkWaitResult.Device,
                    mtkWaitResult.DeviceInfo.ChipConfig,
                    preloader.Emi,
                    cancelToken
                );
                Mediatek.PreloaderName = preloader.Name;
                Mediatek.PreloaderEmi = preloader.Data;
                Main.SharedUI.CkBromReady.Invoke(
                    (Action)(() => Main.SharedUI.CkBromReady.Checked = true)
                );
            }
            catch (OperationCanceledException ex)
            {
                Console.WriteLine(ex.Message.ToString());
                Main.SharedUI.CkBromReady.Invoke(
                    (Action)(() => Main.SharedUI.CkBromReady.Checked = false)
                );
            }
            finally
            {
                mtkWaitResult?.Device.Dispose();
            }

            await Task.Delay(TimeSpan.FromSeconds(2.0));
            await ReadGPT(cancelToken, false);

            if (Main.SharedUI.CkReadAndroidInfo.Checked)
            {
                await Prepare_ReadInfoIMG(cancelToken);
                if (gpt.Partitions.Length > 0)
                {
                    Richlog("Reading Android Info : ", Color.Black, false, false);
                    string tmp = Application.StartupPath + "/tmp";

                    foreach (var item in gpt.Partitions)
                    {
                        if (item.Name == "recovery")
                        {
                            await ReadPartition(
                                "recovery",
                                tmp,
                                item.FirstLba * MtkSparse.sectsize,
                                item.SectorCount * MtkSparse.sectsize,
                                cancelToken
                            );
                            Richlog("OK ", Color.Lime, false, false);
                            break;
                        }
                    }

                    if (!File.Exists(tmp + "/recovery.img"))
                    {
                        foreach (var item in gpt.Partitions)
                        {
                            if (item.Name == "boot")
                            {
                                await ReadPartition(
                                    "boot",
                                    tmp,
                                    item.FirstLba * MtkSparse.sectsize,
                                    item.SectorCount * MtkSparse.sectsize,
                                    cancelToken
                                );
                                Richlog("OK ", Color.Lime, false, false);
                                break;
                            }
                        }
                    }
                    else if (!File.Exists(tmp + "/boot.img"))
                    {
                        foreach (var item in gpt.Partitions)
                        {
                            if (item.Name == "boot_a")
                            {
                                await ReadPartition(
                                    "boot_a",
                                    tmp,
                                    item.FirstLba * MtkSparse.sectsize,
                                    item.SectorCount * MtkSparse.sectsize,
                                    cancelToken
                                );
                                Richlog("OK ", Color.Lime, false, false);
                                break;
                            }
                        }
                    }
                    else if (!File.Exists(tmp + "/boot_b.img"))
                    {
                        foreach (var item in gpt.Partitions)
                        {
                            if (item.Name == "boot_b")
                            {
                                await ReadPartition(
                                    "boot_b",
                                    tmp,
                                    item.FirstLba * MtkSparse.sectsize,
                                    item.SectorCount * MtkSparse.sectsize,
                                    cancelToken
                                );
                                Richlog("OK ", Color.Lime, false, false);
                                break;
                            }
                        }
                    }

                    if (File.Exists(tmp + "/recovery.img"))
                    {
                        File.Move(tmp + "/recovery.img", sourcefile.Dumped);
                        await ReadInfoIMG(cancelToken);
                    }
                    else if (File.Exists(tmp + "/boot.img"))
                    {
                        File.Move(tmp + "/boot.img", sourcefile.Dumped);
                        await ReadInfoIMG(cancelToken);
                    }
                    else if (File.Exists(tmp + "/boot_a.img"))
                    {
                        File.Move(tmp + "/boot_a.img", sourcefile.Dumped);
                        await ReadInfoIMG(cancelToken);
                    }
                    else if (File.Exists(tmp + "/boot_b.img"))
                    {
                        File.Move(tmp + "/boot_b.img", sourcefile.Dumped);
                        await ReadInfoIMG(cancelToken);
                    }
                }
            }
        }

        public static async Task ReadGPT(
            CancellationToken cancelToken = default,
            bool showlist = true
        )
        {
            try
            {
                cancelToken.ThrowIfCancellationRequested();
                gpt = new MtkGpt();
                Console.WriteLine("GPT Len before read gpt : " + gpt.Partitions.Length);
                gpt = await MtkDaxGptService.ReadAsync(
                    uploadResult.Device,
                    uploadResult.FlashInfo,
                    cancelToken
                );
                Console.WriteLine("GPT Len after read gpt : " + gpt.Partitions.Length);

                if (uploadResult.FlashInfo.Type == MtkDaxFlashInfoType.EMMC)
                {
                    MtkSparse.sectsize = 512;
                    storagetype = "emmc";
                }
                if (uploadResult.FlashInfo.Type == MtkDaxFlashInfoType.UFS)
                {
                    MtkSparse.sectsize = 4096;
                    storagetype = "ufs";
                }

                if (gpt?.Partitions.Length > 0)
                {
                    if (showlist)
                    {
                        Main.SharedUI.DataViewmtk.Invoke(
                            new Action(
                                () =>
                                    Main.SharedUI.DataViewmtk.Rows.Add(
                                        false,
                                        "boot",
                                        "preloader",
                                        "0x0",
                                        LongToHex(Mediatek.BootSize),
                                        "Double click to add file..."
                                    )
                            )
                        );
                        Main.SharedUI.DataViewmtk.Invoke(
                            new Action(
                                () =>
                                    Main.SharedUI.DataViewmtk.Rows.Add(
                                        false,
                                        "userarea",
                                        "PGPT",
                                        "0x0",
                                        "0x8000",
                                        "Double click to add file..."
                                    )
                            )
                        );
                        foreach (var item in gpt.Partitions)
                        {
                            Main.SharedUI.DataViewmtk.Invoke(
                                new Action(
                                    () =>
                                        Main.SharedUI.DataViewmtk.Rows.Add(
                                            false,
                                            "userarea",
                                            item.Name,
                                            LongToHex(item.FirstLba * MtkSparse.sectsize),
                                            LongToHex(item.SectorCount * MtkSparse.sectsize),
                                            "Double click to add file..."
                                        )
                                )
                            );
                        }

                        Richlog(" ", Color.Black, false, true);
                        if (Main.SharedUI.DataViewmtk.Rows.Count > 2)
                        {
                            Richlog("Reading partitions   : OK", Color.Black, false, true);
                        }
                    }
                }
                else
                {
                    Richlog(
                        "Unable to read device gpt! Trying with sgpt.",
                        Color.Black,
                        false,
                        true
                    );
                    using (MemoryStream sgptStream = new MemoryStream())
                    {
                        await MtkDaxPartitionService.ReadPartitionByNameAsync(
                            uploadResult.Device,
                            "sgpt",
                            sgptStream,
                            cancelToken
                        );
                        sgptStream.Seek(0L, SeekOrigin.Begin);

                        File.WriteAllBytes(
                            Application.StartupPath + "//sgpt.bin",
                            sgptStream.ToArray()
                        );

                        Richlog("Parsing sgpt", Color.Black, false, true);
                        byte[] buffer = MtkGptRepairService.Fix(
                            sgptStream.ToArray(),
                            uploadResult.FlashInfo.PageSize
                        );

                        if (Main.SharedUI.CkAutoRepairGPTFromSGPT.Checked)
                        {
                            Richlog("Repairing PGPT       : ", Color.Black, false, false);
                            if (File.Exists(Application.StartupPath + "//repaired.pgpt.bin"))
                            {
                                File.Delete(Application.StartupPath + "//repaired.pgpt.bin");
                            }

                            File.WriteAllBytes(
                                Application.StartupPath + "//repaired.pgpt.bin",
                                buffer
                            );

                            await MtkDaxPartitionService.WriteAsync(
                                uploadResult.Device,
                                uploadResult.FlashInfo,
                                0,
                                new FileInfo(Application.StartupPath + "//repaired.pgpt.bin").Length,
                                Application.StartupPath + "//repaired.pgpt.bin",
                                cancelToken
                            );

                            Richlog("OK", Color.Black, false, true);
                        }

                        using (MemoryStream fixedPgptStream = new MemoryStream(buffer))
                        {
                            gpt = await MtkDaxGptService.ReadAsync(
                                fixedPgptStream,
                                uploadResult.FlashInfo.PageSize,
                                cancelToken
                            );
                        }

                        if (showlist)
                        {
                            Main.SharedUI.DataViewmtk.Invoke(
                                new Action(
                                    () =>
                                        Main.SharedUI.DataViewmtk.Rows.Add(
                                            false,
                                            "boot",
                                            "preloader",
                                            "0x0",
                                            LongToHex(Mediatek.BootSize),
                                            "Double click to add file..."
                                        )
                                )
                            );
                            Main.SharedUI.DataViewmtk.Invoke(
                                new Action(
                                    () =>
                                        Main.SharedUI.DataViewmtk.Rows.Add(
                                            false,
                                            "userarea",
                                            "PGPT",
                                            "0x0",
                                            "0x8000",
                                            "Double click to add file..."
                                        )
                                )
                            );
                            foreach (var item in gpt.Partitions)
                            {
                                Main.SharedUI.DataViewmtk.Invoke(
                                    new Action(
                                        () =>
                                            Main.SharedUI.DataViewmtk.Rows.Add(
                                                false,
                                                "userarea",
                                                item.Name,
                                                LongToHex(item.FirstLba * MtkSparse.sectsize),
                                                LongToHex(item.SectorCount * MtkSparse.sectsize),
                                                "Double click to add file..."
                                            )
                                    )
                                );
                            }

                            Richlog(" ", Color.Black, false, true);
                            Richlog("Reading partitions   : OK", Color.Black, false, true);
                        }
                    }
                }
            }
            catch
            {
                Richlog(" ", Color.Black, false, true);
            }

            return;
        }

        public static async Task Read(string folder, CancellationToken cancelToken = default)
        {
            try
            {
                cancelToken.ThrowIfCancellationRequested();
                foreach (DataGridViewRow item in Main.SharedUI.DataViewmtk.Rows)
                {
                    if (Convert.ToBoolean(item.Cells[0].Value) == true)
                    {
                        Richlog(
                            "Reading partition    : " + item.Cells[2].Value.ToString() + " ",
                            Color.Black,
                            false,
                            false
                        );
                        await MtkTask.ReadPartition(
                            item.Cells[2].Value.ToString(),
                            folder,
                            HexToLong(
                                item.Cells[3].Value.ToString().Replace(" ", "").Replace("0x", "")
                            ),
                            HexToLong(
                                item.Cells[4].Value.ToString().Replace(" ", "").Replace("0x", "")
                            ),
                            cancelToken
                        );
                        Richlog("OK", Color.Lime, false, true);
                    }
                }
            }
            finally
            {
                if (Main.SharedUI.CkCreateScatterBackupFile.Checked)
                {
                    string Scatter = folder + "/" + Mediatek.Platform + "_Android_Scatter.txt";
                    if (File.Exists(Scatter))
                    {
                        File.Delete(Scatter);
                    }

                    Richlog(" ", Color.Black, false, true);
                    Richlog(
                        "Scatter File Created : " + Mediatek.Platform + "_Android_Scatter.txt",
                        Color.Black,
                        false,
                        true
                    );
                    File.WriteAllText(Scatter, MtkScatter.ScatterBuilder());
                }
            }

            if (Main.SharedUI.CkAutoReboot.Checked)
            {
                Richlog("Rebooting            : ", Color.Black, false, false);
                await MtkDaxUploadBootService.RebootAsync(uploadResult.Device, cancelToken);

                Richlog("OK", Color.Lime, false, true);

                Main.SharedUI.CkBromReady.Invoke(
                    (Action)(() => Main.SharedUI.CkBromReady.Checked = false)
                );
                Main.SharedUI.BtnReadPartition.Invoke(
                    (Action)(() => Main.SharedUI.BtnReadPartition.Enabled = true)
                );
                Main.SharedUI.BtnErasePartition.Invoke(
                    (Action)(() => Main.SharedUI.BtnErasePartition.Enabled = true)
                );
                Main.SharedUI.BtnFlash.Invoke(
                    (Action)(() => Main.SharedUI.BtnFlash.Enabled = true)
                );
                Main.SharedUI.BtnIdentify.Invoke(
                    (Action)(() => Main.SharedUI.BtnIdentify.Enabled = true)
                );

                Richlog(" ", Color.Black, false, true);
                Richlog("Task Completed...", Color.Black, false, true);
            }
            else
            {
                Richlog(" ", Color.Black, false, true);
                Richlog("Task Completed...", Color.Black, false, true);
            }
            return;
        }

        public static async Task ReadPartition(
            string partition,
            string foldersave,
            long address,
            long size,
            CancellationToken cancelToken = default
        )
        {
            cancelToken.ThrowIfCancellationRequested();
            string save = null;
            if (partition == "preloader")
            {
                if (File.Exists(foldersave + "\\" + Mediatek.PreloaderName))
                {
                    File.Delete(foldersave + "\\" + Mediatek.PreloaderName);
                }
                File.WriteAllBytes(
                    foldersave + "\\" + Mediatek.PreloaderName,
                    Mediatek.PreloaderEmi
                );
            }
            else
            {
                save = foldersave + "\\" + partition + ".img";
                if (File.Exists(save))
                    File.Delete(save);

                await MtkDaxPartitionService.ReadSaveAsync(
                    uploadResult.Device,
                    uploadResult.FlashInfo,
                    address,
                    size,
                    save,
                    cancelToken
                );
            }
        }

        public static async Task Flash(CancellationToken cancelToken = default)
        {
            cancelToken.ThrowIfCancellationRequested();
            try
            {
                foreach (DataGridViewRow item in Main.SharedUI.DataViewmtk.Rows)
                {
                    if (Convert.ToBoolean(item.Cells[0].Value) == true)
                    {
                        if (File.Exists(item.Cells[5].Value.ToString()))
                        {
                            if (item.Cells[2].Value.ToString() == "preloader")
                            {
                                Richlog(
                                    "Writing Boot 1       : preloader ",
                                    Color.Black,
                                    false,
                                    false
                                );

                                await MtkDaxPartitionService.WriteBoot(
                                    uploadResult.Device,
                                    uploadResult.FlashInfo,
                                    "preloader",
                                    item.Cells[5].Value.ToString(),
                                    cancelToken
                                );

                                Richlog("OK", Color.Lime, false, true);

                                Richlog(
                                    "Writing Boot 2       : preloader backup ",
                                    Color.Black,
                                    false,
                                    false
                                );

                                await MtkDaxPartitionService.WriteBoot(
                                    uploadResult.Device,
                                    uploadResult.FlashInfo,
                                    "preloader_backup",
                                    item.Cells[5].Value.ToString(),
                                    cancelToken
                                );

                                Richlog("OK", Color.Lime, false, true);
                            }
                            else
                            {
                                if (Main.SharedUI.CkAutoUnsparse.Checked)
                                {
                                    if (MtkSparse.CekSparse(item.Cells[5].Value.ToString()))
                                    {
                                        Main.SharedUI.label_status.Invoke(
                                            (Action)(
                                                () =>
                                                    Main.SharedUI.label_status.Text =
                                                        "Formating "
                                                        + item.Cells[2].Value.ToString()
                                                        + " partition..."
                                            )
                                        );
                                        await MtkTask.FormatPartition(
                                            HexToLong(
                                                item.Cells[3].Value
                                                    .ToString()
                                                    .Replace(" ", "")
                                                    .Replace("0x", "")
                                            ),
                                            HexToLong(
                                                item.Cells[4].Value
                                                    .ToString()
                                                    .Replace(" ", "")
                                                    .Replace("0x", "")
                                            ),
                                            cancelToken
                                        );

                                        Main.SharedUI.label_writensize.Invoke(
                                            (Action)(() => Main.SharedUI.label_status.Text = "OK")
                                        );
                                    }
                                }

                                Richlog(
                                    "Writing partition    : "
                                        + item.Cells[2].Value.ToString()
                                        + " ",
                                    Color.Black,
                                    false,
                                    false
                                );
                                await MtkDaxPartitionService.WriteAsync(
                                    uploadResult.Device,
                                    uploadResult.FlashInfo,
                                    HexToLong(
                                                item.Cells[3].Value
                                                    .ToString()
                                                    .Replace(" ", "")
                                                    .Replace("0x", "")
                                            ),
                                    HexToLong(
                                                item.Cells[4].Value
                                                    .ToString()
                                                    .Replace(" ", "")
                                                    .Replace("0x", "")
                                            ),
                                    item.Cells[5].Value.ToString(),
                                    cancelToken
                                );
                                Richlog("OK", Color.Lime, false, true);
                            }
                        }
                    }
                }
            }
            finally { }
            if (Main.SharedUI.CkAutoReboot.Checked)
            {
                Richlog(" ", Color.Black, false, true);
                Richlog("Rebooting            : ", Color.Black, false, false);
                await MtkDaxUploadBootService.RebootAsync(uploadResult.Device, cancelToken);
                Richlog("OK", Color.Lime, false, true);

                Main.SharedUI.CkBromReady.Invoke(
                    (Action)(() => Main.SharedUI.CkBromReady.Checked = false)
                );
                Main.SharedUI.BtnReadPartition.Invoke(
                    (Action)(() => Main.SharedUI.BtnReadPartition.Enabled = true)
                );
                Main.SharedUI.BtnErasePartition.Invoke(
                    (Action)(() => Main.SharedUI.BtnErasePartition.Enabled = true)
                );
                Main.SharedUI.BtnFlash.Invoke(
                    (Action)(() => Main.SharedUI.BtnFlash.Enabled = true)
                );
                Main.SharedUI.BtnIdentify.Invoke(
                    (Action)(() => Main.SharedUI.BtnIdentify.Enabled = true)
                );

                Richlog(" ", Color.Black, false, true);
                Richlog("Task Completed...", Color.Black, false, true);
            }
            else
            {
                Richlog(" ", Color.Black, false, true);
                Richlog("Task Completed...", Color.Black, false, true);
            }
            return;
        }

        public static async Task WritePartition(
            string files,
            long address,
            long len,
            CancellationToken cancelToken = default
        )
        {
            await MtkDaxPartitionService.WriteAsync(
                uploadResult.Device,
                uploadResult.FlashInfo,
                address,
                len,
                files,
                cancelToken
            );
        }

        public static async Task Erase(CancellationToken cancelToken = default)
        {
            cancelToken.ThrowIfCancellationRequested();
            try
            {
                foreach (DataGridViewRow item in Main.SharedUI.DataViewmtk.Rows)
                {
                    if (Convert.ToBoolean(item.Cells[0].Value) == true)
                    {
                        if (item.Cells[2].Value.ToString() == "preloader")
                        {
                            Richlog("Erasing Preloader    : ", Color.Black, false, false);
                            Richlog("OK", Color.Lime, false, true);
                        }
                        else
                        {
                            Richlog(
                                "Erasing partition    : " + item.Cells[2].Value.ToString() + " ",
                                Color.Black,
                                false,
                                false
                            );
                            await MtkTask.FormatPartition(
                                HexToLong(
                                    item.Cells[3].Value
                                        .ToString()
                                        .Replace(" ", "")
                                        .Replace("0x", "")
                                ),
                                HexToLong(
                                    item.Cells[4].Value
                                        .ToString()
                                        .Replace(" ", "")
                                        .Replace("0x", "")
                                ),
                                cancelToken
                            );
                            Richlog("OK", Color.Lime, false, true);
                        }
                    }
                }
            }
            finally { }
            if (Main.SharedUI.CkAutoReboot.Checked)
            {
                Richlog(" ", Color.Black, false, true);
                Richlog("Rebooting            : ", Color.Black, false, false);
                await MtkDaxUploadBootService.RebootAsync(uploadResult.Device, cancelToken);
                Richlog("OK", Color.Lime, false, true);

                Main.SharedUI.CkBromReady.Invoke(
                    (Action)(() => Main.SharedUI.CkBromReady.Checked = false)
                );
                Main.SharedUI.BtnReadPartition.Invoke(
                    (Action)(() => Main.SharedUI.BtnReadPartition.Enabled = true)
                );
                Main.SharedUI.BtnErasePartition.Invoke(
                    (Action)(() => Main.SharedUI.BtnErasePartition.Enabled = true)
                );
                Main.SharedUI.BtnFlash.Invoke(
                    (Action)(() => Main.SharedUI.BtnFlash.Enabled = true)
                );
                Main.SharedUI.BtnIdentify.Invoke(
                    (Action)(() => Main.SharedUI.BtnIdentify.Enabled = true)
                );

                Richlog(" ", Color.Black, false, true);
                Richlog("Task Completed...", Color.Black, false, true);
            }
            else
            {
                Richlog(" ", Color.Black, false, true);
                Richlog("Task Completed...", Color.Black, false, true);
            }
            return;
        }

        public static async Task FormatPartition(
            long address,
            long size,
            CancellationToken cancelToken = default
        )
        {
            await MtkDaxPartitionService.FormatAsync(
                uploadResult.Device,
                uploadResult.FlashInfo,
                address,
                size,
                cancelToken
            );
        }

        public static async Task FormatUserdata(CancellationToken cancelToken = default)
        {
            cancelToken.ThrowIfCancellationRequested();
            Richlog("Formating Userdata : ", Color.Black, false, false);
            await ReadGPT(cancelToken, false);
            if (gpt.Partitions.Length > 0)
            {
                foreach (var item in gpt.Partitions)
                {
                    if (item.Name == "userdata")
                    {
                        await FormatPartition(
                            item.FirstLba * MtkSparse.sectsize,
                            item.SectorCount * MtkSparse.sectsize,
                            cancelToken
                        );
                        break;
                    }
                }
                bool reboot = false;
                Main.SharedUI.CkAutoReboot.Invoke(
                    (Action)(() => reboot = Main.SharedUI.CkAutoReboot.Checked)
                );
                if (reboot)
                {
                    await MtkDaxUploadBootService.RebootAsync(uploadResult.Device, cancelToken);
                }
                Richlog("OK", Color.Lime, false, true);
            }
            else
            {
                Richlog("Failed partition not found!", Color.Red, false, true);
            }
        }

        public static async Task FormatUserdataFRP(CancellationToken cancelToken = default)
        {
            cancelToken.ThrowIfCancellationRequested();
            Richlog("Formating Userdata + FRP : ", Color.Black, false, false);
            await ReadGPT(cancelToken, false);
            if (gpt.Partitions.Length > 0)
            {
                foreach (var item in gpt.Partitions)
                {
                    if (item.Name == "userdata")
                    {
                        await FormatPartition(
                            item.FirstLba * MtkSparse.sectsize,
                            item.SectorCount * MtkSparse.sectsize,
                            cancelToken
                        );
                    }
                }

                foreach (var item in gpt.Partitions)
                {
                    if (item.Name == "frp")
                    {
                        await FormatPartition(
                            item.FirstLba * MtkSparse.sectsize,
                            item.SectorCount * MtkSparse.sectsize,
                            cancelToken
                        );
                    }
                }

                foreach (var item in gpt.Partitions)
                {
                    if (item.Name == "persistent")
                    {
                        await FormatPartition(
                            item.FirstLba * MtkSparse.sectsize,
                            item.SectorCount * MtkSparse.sectsize,
                            cancelToken
                        );
                    }
                }
                bool reboot = false;
                Main.SharedUI.CkAutoReboot.Invoke(
                    (Action)(() => reboot = Main.SharedUI.CkAutoReboot.Checked)
                );
                if (reboot)
                {
                    await MtkDaxUploadBootService.RebootAsync(uploadResult.Device, cancelToken);
                }
                Richlog("OK", Color.Lime, false, true);
            }
            else
            {
                Richlog("Failed partition not found!", Color.Red, false, true);
            }
        }

        public static async Task FormatFromRecovery(CancellationToken cancelToken = default)
        {
            cancelToken.ThrowIfCancellationRequested();
            Richlog("Format From Recovery : ", Color.Black, false, false);
            await ReadGPT(cancelToken, false);
            if (gpt.Partitions.Length > 0)
            {
                foreach (var item in gpt.Partitions)
                {
                    if (item.Name == "para")
                    {
                        string files = Application.StartupPath + "\\files\\" + storagetype;
                        await WritePartition(
                            files,
                            item.FirstLba * MtkSparse.sectsize,
                            item.SectorCount * MtkSparse.sectsize,
                            cancelToken
                        );
                    }
                }
                bool reboot = false;
                Main.SharedUI.CkAutoReboot.Invoke(
                    (Action)(() => reboot = Main.SharedUI.CkAutoReboot.Checked)
                );
                if (reboot)
                {
                    await MtkDaxUploadBootService.RebootAsync(uploadResult.Device, cancelToken);
                }
                Richlog("OK", Color.Lime, false, true);
            }
            else
            {
                Richlog("Failed partition not found!", Color.Red, false, true);
            }
        }

        public static async Task FormatFromRecoveryFRP(CancellationToken cancelToken = default)
        {
            cancelToken.ThrowIfCancellationRequested();
            Richlog("Format From Recovery & Erase FRP : ", Color.Black, false, false);
            await ReadGPT(cancelToken, false);
            if (gpt.Partitions.Length > 0)
            {
                foreach (var item in gpt.Partitions)
                {
                    if (item.Name == "para")
                    {
                        string files = Application.StartupPath + "\\files\\" + storagetype;
                        await WritePartition(
                            files,
                            item.FirstLba * MtkSparse.sectsize,
                            item.SectorCount * MtkSparse.sectsize,
                            cancelToken
                        );
                    }
                }

                foreach (var item in gpt.Partitions)
                {
                    if (item.Name == "frp")
                    {
                        await FormatPartition(
                            item.FirstLba * MtkSparse.sectsize,
                            item.SectorCount * MtkSparse.sectsize,
                            cancelToken
                        );
                    }
                }

                foreach (var item in gpt.Partitions)
                {
                    if (item.Name == "persistent")
                    {
                        await FormatPartition(
                            item.FirstLba * MtkSparse.sectsize,
                            item.SectorCount * MtkSparse.sectsize,
                            cancelToken
                        );
                    }
                }
                bool reboot = false;
                Main.SharedUI.CkAutoReboot.Invoke(
                    (Action)(() => reboot = Main.SharedUI.CkAutoReboot.Checked)
                );
                if (reboot)
                {
                    await MtkDaxUploadBootService.RebootAsync(uploadResult.Device, cancelToken);
                }
                Richlog("OK", Color.Lime, false, true);
            }
            else
            {
                Richlog("Failed partition not found!", Color.Red, false, true);
            }
        }

        public static async Task EraseFRPMiCloud(CancellationToken cancelToken = default)
        {
            cancelToken.ThrowIfCancellationRequested();
            Richlog("Erasing FRP + MiCloud : ", Color.Black, false, false);
            await ReadGPT(cancelToken, false);
            if (gpt.Partitions.Length > 0)
            {
                foreach (var item in gpt.Partitions)
                {
                    if (item.Name == "frp")
                    {
                        await FormatPartition(
                            item.FirstLba * MtkSparse.sectsize,
                            item.SectorCount * MtkSparse.sectsize,
                            cancelToken
                        );
                    }
                }

                foreach (var item in gpt.Partitions)
                {
                    if (item.Name == "persist")
                    {
                        await FormatPartition(
                            item.FirstLba * MtkSparse.sectsize,
                            item.SectorCount * MtkSparse.sectsize,
                            cancelToken
                        );
                    }
                }

                bool reboot = false;
                Main.SharedUI.CkAutoReboot.Invoke(
                    (Action)(() => reboot = Main.SharedUI.CkAutoReboot.Checked)
                );
                if (reboot)
                {
                    await MtkDaxUploadBootService.RebootAsync(uploadResult.Device, cancelToken);
                }
                Richlog("OK", Color.Lime, false, true);
            }
            else
            {
                Richlog("Failed partition not found!", Color.Red, false, true);
            }
        }

        public static async Task EraseFRP(CancellationToken cancelToken = default)
        {
            cancelToken.ThrowIfCancellationRequested();
            Richlog("Erasing FRP : ", Color.Black, false, false);
            await ReadGPT(cancelToken, false);
            if (gpt.Partitions.Length > 0)
            {
                foreach (var item in gpt.Partitions)
                {
                    if (item.Name == "frp")
                    {
                        await FormatPartition(
                            item.FirstLba * MtkSparse.sectsize,
                            item.SectorCount * MtkSparse.sectsize,
                            cancelToken
                        );
                    }
                }
                foreach (var item in gpt.Partitions)
                {
                    if (item.Name == "persistent")
                    {
                        await FormatPartition(
                            item.FirstLba * MtkSparse.sectsize,
                            item.SectorCount * MtkSparse.sectsize,
                            cancelToken
                        );
                    }
                }
                bool reboot = false;
                Main.SharedUI.CkAutoReboot.Invoke(
                    (Action)(() => reboot = Main.SharedUI.CkAutoReboot.Checked)
                );
                if (reboot)
                {
                    await MtkDaxUploadBootService.RebootAsync(uploadResult.Device, cancelToken);
                }
                Richlog("OK", Color.Lime, false, true);
            }
            else
            {
                Richlog("Failed partition not found!", Color.Red, false, true);
            }
        }

        public static async Task BackupNV(string folder, CancellationToken cancelToken = default)
        {
            cancelToken.ThrowIfCancellationRequested();
            Richlog("Backuping NV : ", Color.Black, false, false);
            await ReadGPT(cancelToken, false);
            if (gpt.Partitions.Length > 0)
            {
                foreach (var item in gpt.Partitions)
                {
                    if (item.Name == "nvram")
                    {
                        await ReadPartition(
                            item.Name,
                            folder,
                            item.FirstLba * MtkSparse.sectsize,
                            item.SectorCount * MtkSparse.sectsize,
                            cancelToken
                        );
                    }
                }
                foreach (var item in gpt.Partitions)
                {
                    if (item.Name == "sec_efs")
                    {
                        await ReadPartition(
                            item.Name,
                            folder,
                            item.FirstLba * MtkSparse.sectsize,
                            item.SectorCount * MtkSparse.sectsize,
                            cancelToken
                        );
                    }
                }
                foreach (var item in gpt.Partitions)
                {
                    if (item.Name == "nvcfg")
                    {
                        await ReadPartition(
                            item.Name,
                            folder,
                            item.FirstLba * MtkSparse.sectsize,
                            item.SectorCount * MtkSparse.sectsize,
                            cancelToken
                        );
                    }
                }
                foreach (var item in gpt.Partitions)
                {
                    if (item.Name == "nvdata")
                    {
                        await ReadPartition(
                            item.Name,
                            folder,
                            item.FirstLba * MtkSparse.sectsize,
                            item.SectorCount * MtkSparse.sectsize,
                            cancelToken
                        );
                    }
                }
                foreach (var item in gpt.Partitions)
                {
                    if (item.Name == "protect1")
                    {
                        await ReadPartition(
                            item.Name,
                            folder,
                            item.FirstLba * MtkSparse.sectsize,
                            item.SectorCount * MtkSparse.sectsize,
                            cancelToken
                        );
                    }
                }

                foreach (var item in gpt.Partitions)
                {
                    if (item.Name == "protect2")
                    {
                        await ReadPartition(
                            item.Name,
                            folder,
                            item.FirstLba * MtkSparse.sectsize,
                            item.SectorCount * MtkSparse.sectsize,
                            cancelToken
                        );
                    }
                }

                Richlog("OK", Color.Lime, false, true);

                if (Main.SharedUI.CkCreateScatterBackupFile.Checked)
                {
                    string Scatter = folder + "/" + Mediatek.Platform + "_Android_Scatter.txt";
                    if (File.Exists(Scatter))
                    {
                        File.Delete(Scatter);
                    }

                    Richlog(" ", Color.Black, false, true);
                    Richlog(
                        "Scatter File Created : " + Mediatek.Platform + "_Android_Scatter.txt",
                        Color.Black,
                        false,
                        false
                    );
                    File.WriteAllText(Scatter, MtkScatter.ScatterBuilderGPT(gpt));
                }

                bool reboot = false;
                Main.SharedUI.CkAutoReboot.Invoke(
                    (Action)(() => reboot = Main.SharedUI.CkAutoReboot.Checked)
                );
                if (reboot)
                {
                    await MtkDaxUploadBootService.RebootAsync(uploadResult.Device, cancelToken);
                }
            }
            else
            {
                Richlog("Failed partition not found!", Color.Red, false, true);
            }
        }

        public static async Task EraseNV(CancellationToken cancelToken = default)
        {
            cancelToken.ThrowIfCancellationRequested();
            Richlog("Erasing NV :", Color.Black, false, false);
            await ReadGPT(cancelToken, false);
            if (gpt.Partitions.Length > 0)
            {
                foreach (var item in gpt.Partitions)
                {
                    if (item.Name == "nvram")
                    {
                        await FormatPartition(
                            item.FirstLba * MtkSparse.sectsize,
                            item.SectorCount * MtkSparse.sectsize,
                            cancelToken
                        );
                    }
                }
                foreach (var item in gpt.Partitions)
                {
                    if (item.Name == "sec_efs")
                    {
                        await FormatPartition(
                            item.FirstLba * MtkSparse.sectsize,
                            item.SectorCount * MtkSparse.sectsize,
                            cancelToken
                        );
                    }
                }
                foreach (var item in gpt.Partitions)
                {
                    if (item.Name == "nvcfg")
                    {
                        await FormatPartition(
                            item.FirstLba * MtkSparse.sectsize,
                            item.SectorCount * MtkSparse.sectsize,
                            cancelToken
                        );
                    }
                }
                foreach (var item in gpt.Partitions)
                {
                    if (item.Name == "nvdata")
                    {
                        await FormatPartition(
                            item.FirstLba * MtkSparse.sectsize,
                            item.SectorCount * MtkSparse.sectsize,
                            cancelToken
                        );
                    }
                }
                foreach (var item in gpt.Partitions)
                {
                    if (item.Name == "protect1")
                    {
                        await FormatPartition(
                            item.FirstLba * MtkSparse.sectsize,
                            item.SectorCount * MtkSparse.sectsize,
                            cancelToken
                        );
                    }
                }
                foreach (var item in gpt.Partitions)
                {
                    if (item.Name == "protect2")
                    {
                        await FormatPartition(
                            item.FirstLba * MtkSparse.sectsize,
                            item.SectorCount * MtkSparse.sectsize,
                            cancelToken
                        );
                    }
                }
                bool reboot = false;
                Main.SharedUI.CkAutoReboot.Invoke(
                    (Action)(() => reboot = Main.SharedUI.CkAutoReboot.Checked)
                );
                if (reboot)
                {
                    await MtkDaxUploadBootService.RebootAsync(uploadResult.Device, cancelToken);
                }
                Richlog("OK", Color.Lime, false, true);
            }
            else
            {
                Richlog("Failed partition not found!", Color.Red, false, true);
            }
        }

        public static void closingport()
        {
            uploadResult?.Device?.Dispose();
            mtkWaitResult?.Device?.Dispose();
        }
    }
}
