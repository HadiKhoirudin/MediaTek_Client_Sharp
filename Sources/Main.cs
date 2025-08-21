using mtkclient.library;
using mtkclient.library.xflash;
using mtkclient.MTK.Client.Scatter;
using mtkclient.Tasks;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static mtkclient.devicehandler.Native;
using static mtkclient.gui;
using static mtkclient.MTK.Client.utils;
using static mtkclient.USBFastConnect;

namespace mtkclient
{
    public partial class Main : Form
    {
        public static Main SharedUI;
        private bool isMTKClientRunning = false;

        private CancellationTokenSource cts = new CancellationTokenSource();

        public Main()
        {
            InitializeComponent();
            SharedUI = this;
            getcomInfo();
            logInfo();
            LibUsb1.LoadLibrary();
        }

        private void log_TextChanged(object sender, EventArgs e)
        {
            log.Invoke(
                new Action(() =>
                {
                    log.SelectionStart = log.TextLength;
                    log.ScrollToCaret();
                })
            );
        }

        private void logInfo()
        {
            log.Clear();
            Richlog("iReverse MTK Client Lite.", Color.Black, false, true);
            Richlog(" ", Color.Black, false, true);
            Richlog("Supported Mediatek CPU Config : ", Color.Black, false, true);
            Richlog(" - MediaTek Protocol V5", Color.Purple, false, true);
        }

        private void ComboPort_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ComboPort.Text))
            {
                MtkTask.AllowSleep();
                CkBromReady.Checked = false;
            }
            else
            {
                MtkTask.PreventSleep();
            }
        }

        private void CkBromReady_CheckedChanged(object sender, EventArgs e)
        {
            if (CkBromReady.Checked)
            {
                MtkDaxUploadBootService.rebootto = false;
                MtkDeviceWaiterService.reconnect = false;
            }
            else
            {
                MtkTask.closingport();
            }
        }

        private void CkBypassSecureBootOnly_CheckedChanged(object sender, EventArgs e)
        {
            if (CkBypassSecureBootOnly.Checked)
            {
                BtnFlash.Text = "Bypass";
                BtnFlash.Enabled = true;
                CkBromReady.Checked = false;
                BtnIdentify.Enabled = false;
                BtnScatter.Enabled = false;
                BtnEMI1.Enabled = false;
                BtnEMI2.Enabled = false;
            }
            else
            {
                BtnFlash.Text = "Flash";
                BtnIdentify.Enabled = true;
                BtnScatter.Enabled = true;
                BtnEMI1.Enabled = true;
                BtnEMI2.Enabled = true;
            }
        }

        private void CkList_CheckedChanged(object sender, EventArgs e)
        {
            if (DataViewmtk.Rows.Count > 0)
            {
                if (CkList.Checked)
                {
                    foreach (DataGridViewRow item in DataViewmtk.Rows)
                    {
                        item.Cells[0].Value = true;
                    }
                }
                else
                {
                    foreach (DataGridViewRow item in DataViewmtk.Rows)
                    {
                        item.Cells[0].Value = false;
                    }
                }
            }
        }

        private void DataViewmtk_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (DataViewmtk.Rows.Count > 0)
            {
                if (e.ColumnIndex == 5)
                {
                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    openFileDialog.Title =
                        "Select File Partition " + DataViewmtk.CurrentRow.Cells[2].Value.ToString();
                    openFileDialog.InitialDirectory = Environment.GetFolderPath(
                        Environment.SpecialFolder.MyComputer
                    );
                    openFileDialog.FileName = "*.*";
                    openFileDialog.Filter = "ALL FILE  (*.*)|*.*";
                    openFileDialog.FilterIndex = 2;
                    openFileDialog.RestoreDirectory = true;
                    if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        FileInfo fileInfo = new FileInfo(openFileDialog.FileName);
                        long lenfile = fileInfo.Length;
                        long length = Convert.ToInt64(
                            HexToLong(
                                DataViewmtk.CurrentRow.Cells[4].Value.ToString().Replace("0x", "")
                            )
                        );
                        if (lenfile > length)
                        {
                            MessageBox.Show(
                                "File size is too large than partition size!",
                                "iReverse MTK CLIENT - C# Minimalis Version",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information
                            );
                            DataViewmtk.CurrentRow.Cells[5].Value = "Double click to input file...";
                            DataViewmtk.CurrentRow.Cells[0].Value = false;
                        }
                        else
                        {
                            DataViewmtk.CurrentRow.Cells[5].Value = openFileDialog.FileName;
                            DataViewmtk.CurrentRow.Cells[0].Value = true;
                        }
                    }
                }
            }
        }

        public static void ProcessBar(long Process, long total)
        {
            int res = Convert.ToInt32(
                Math.Truncate(Math.Round(Math.Round(Process * 100L / (double)total)))
            );
            if (res > 100)
            {
                res = 100;
            }
            Main.SharedUI.progressBar1.Invoke(
                (Action)(() => Main.SharedUI.progressBar1.Value = res)
            );
        }

        public static void ProcessBar(int total)
        {
            int res = total;
            if (res > 100)
            {
                res = 100;
            }
            Main.SharedUI.progressBar1.Invoke(
                (Action)(() => Main.SharedUI.progressBar1.Value = res)
            );
        }

        private async void BtnIdentify_Click(object sender, EventArgs e)
        {
            if (!isMTKClientRunning)
            {
                try
                {
                    isMTKClientRunning = true;
                    var token = cts.Token;
                    if (DataViewmtk.Rows.Count > 0)
                    {
                        DataViewmtk.Rows.Clear();
                    }

                    if (CkBromReady.Checked)
                    {
                        await Task.Run(() => MtkTask.ReadGPT(token));
                    }
                    else
                    {
                        log.Clear();
                        await Task.Run(() => MtkTask.InitAsync(token));
                        if (CkBromReady.Checked)
                        {
                            await Task.Run(() => MtkTask.ReadGPT(token));
                        }
                    }
                }
                catch (OperationCanceledException ex)
                {
                    Richlog("Your task was canceled.", Color.Black, false, true);
                    CkBromReady.Invoke((Action)(() => CkBromReady.Checked = false));
                    isMTKClientRunning = false;
                }
                finally
                {
                    BtnReadPartition.Invoke((Action)(() => BtnReadPartition.Enabled = true));
                    BtnErasePartition.Invoke((Action)(() => BtnErasePartition.Enabled = true));
                    BtnFlash.Invoke((Action)(() => BtnFlash.Enabled = true));
                    BtnBrowse.Invoke((Action)(() => BtnBrowse.Enabled = true));
                    Richlog(" ", Color.Black, false, true);
                    Richlog("Task Completed...", Color.Black, false, true);
                }
                isMTKClientRunning = false;
            }
        }

        private async void BtnReadPartition_Click(object sender, EventArgs e)
        {
            if (!isMTKClientRunning)
            {
                bool Flag = false;
                foreach (DataGridViewRow row in DataViewmtk.Rows)
                {
                    if (Convert.ToBoolean(row.Cells[0].Value) == true)
                    {
                        Flag = true;
                        break;
                    }
                }
                if (Flag)
                {
                    string folder = null;
                    FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog()
                    {
                        ShowNewFolderButton = true
                    };
                    if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        log.Clear();
                        folder = folderBrowserDialog.SelectedPath;
                        try
                        {
                            isMTKClientRunning = true;
                            var token = cts.Token;
                            if (CkBromReady.Checked)
                            {
                                await Task.Run(() => MtkTask.Read(folder, token));
                            }
                            else
                            {
                                await Task.Run(() => MtkTask.InitAsync(token));
                                await Task.Run(() => MtkTask.Read(folder, token));
                            }
                        }
                        catch (OperationCanceledException ex)
                        {
                            Richlog("Your task was canceled.", Color.Black, false, true);
                            CkBromReady.Invoke((Action)(() => CkBromReady.Checked = false));
                            isMTKClientRunning = false;
                        }
                        isMTKClientRunning = false;
                    }
                }
            }
        }

        private async void BtnErasePartition_Click(object sender, EventArgs e)
        {
            if (!isMTKClientRunning)
            {
                bool Flag = false;
                foreach (DataGridViewRow row in DataViewmtk.Rows)
                {
                    if (Convert.ToBoolean(row.Cells[0].Value) == true)
                    {
                        Flag = true;
                        break;
                    }
                }
                if (Flag)
                {
                    log.Clear();
                    try
                    {
                        isMTKClientRunning = true;
                        var token = cts.Token;
                        if (CkBromReady.Checked)
                        {
                            await Task.Run(() => MtkTask.Erase(token));
                        }
                        else
                        {
                            await Task.Run(() => MtkTask.InitAsync(token));
                            await Task.Run(() => MtkTask.Erase(token));
                        }
                    }
                    catch (OperationCanceledException ex)
                    {
                        Richlog("Your task was canceled.", Color.Black, false, true);
                        CkBromReady.Invoke((Action)(() => CkBromReady.Checked = false));

                        isMTKClientRunning = false;
                    }
                }
                isMTKClientRunning = false;
            }
        }

        private async void BtnFlash_Click(object sender, EventArgs e)
        {
            if (!isMTKClientRunning)
            {
                if (BtnFlash.Text == "Flash")
                {
                    bool Flag = false;
                    foreach (DataGridViewRow row in DataViewmtk.Rows)
                    {
                        if (Convert.ToBoolean(row.Cells[0].Value) == true)
                        {
                            Flag = true;
                            break;
                        }
                    }
                    if (Flag)
                    {
                        log.Clear();
                        try
                        {
                            isMTKClientRunning = true;
                            var token = cts.Token;
                            if (CkBromReady.Checked)
                            {
                                await Task.Run(() => MtkTask.Flash(token));
                            }
                            else
                            {
                                await Task.Run(() => MtkTask.InitAsync(token));
                                await Task.Run(() => MtkTask.Flash(token));
                            }
                        }
                        catch (OperationCanceledException ex)
                        {
                            Richlog("Your task was canceled.", Color.Black, false, true);
                            CkBromReady.Invoke((Action)(() => CkBromReady.Checked = false));

                            isMTKClientRunning = false;
                        }
                        finally
                        {
                            string tmp = Application.StartupPath + "\\tmp";
                            if (Directory.Exists(tmp))
                            {
                                DirectoryInfo directory = new DirectoryInfo(tmp);
                                foreach (FileInfo File in directory.EnumerateFiles())
                                {
                                    File.Delete();
                                }
                                foreach (
                                    DirectoryInfo subDirectory in directory.EnumerateDirectories()
                                )
                                {
                                    subDirectory.Delete(true);
                                }
                                directory.Delete(true);
                            }
                        }
                    }
                }
                else
                {
                    log.Clear();
                    try
                    {
                        isMTKClientRunning = true;
                        var token = cts.Token;
                        await Task.Run(() => MtkTask.InitAsync(token));
                    }
                    catch (OperationCanceledException ex)
                    {
                        Richlog("Your task was canceled.", Color.Black, false, true);
                        CkBromReady.Invoke((Action)(() => CkBromReady.Checked = false));
                        isMTKClientRunning = false;
                    }
                }
                isMTKClientRunning = false;
            }
        }

        private void BtnEmi_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Title = "Select EMI | Preloader File";
            fd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
            fd.FileName = "*.*";
            fd.Filter = "Preloader file |*.bin*;";
            fd.FilterIndex = 1;
            fd.RestoreDirectory = true;
            if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                TxtEMI.Text = fd.SafeFileName;
                TxtEMIOneClick.Text = fd.SafeFileName;
                Mediatek.Preloader = fd.FileName;
            }
        }

        private void BtnEMI2_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Title = "Select EMI | Preloader File";
            fd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
            fd.FileName = "*.*";
            fd.Filter = "Preloader file |*.bin*;";
            fd.FilterIndex = 1;
            fd.RestoreDirectory = true;
            if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                TxtEMI.Text = fd.SafeFileName;
                TxtEMIOneClick.Text = fd.SafeFileName;
                Mediatek.Preloader = fd.FileName;
            }
        }

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            if (DataViewmtk.Rows.Count > 0)
            {
                FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog()
                {
                    ShowNewFolderButton = true
                };
                if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string selectedFolderPath = folderBrowserDialog.SelectedPath;
                    TxtIMGBin.Text = folderBrowserDialog.SelectedPath;
                    foreach (DataGridViewRow row in DataViewmtk.Rows)
                    {
                        string fileName = row.Cells[2].Value.ToString();
                        if (fileName == "preloader")
                        {
                            string[] filenames = Directory.GetFiles(
                                selectedFolderPath,
                                "*.*",
                                SearchOption.AllDirectories
                            );
                            foreach (string filename_Conflict in filenames)
                            {
                                if (filename_Conflict.Contains("preloader_"))
                                {
                                    fileName = filename_Conflict;
                                    break;
                                }
                            }
                            Console.WriteLine("File Preloader : " + fileName);
                        }
                        string filePath = Path.Combine(selectedFolderPath, fileName);
                        if (File.Exists(filePath))
                        {
                            row.Cells[0].Value = true;
                            row.Cells[5].Value = filePath;
                        }
                        else
                        {
                            filePath = Path.Combine(selectedFolderPath, fileName + ".img");
                            if (File.Exists(filePath))
                            {
                                row.Cells[0].Value = true;
                                row.Cells[5].Value = filePath;
                            }
                            else
                            {
                                filePath = Path.Combine(selectedFolderPath, fileName + ".bin");
                                if (File.Exists(filePath))
                                {
                                    row.Cells[0].Value = true;
                                    row.Cells[5].Value = filePath;
                                }
                            }
                        }
                    }
                }
                else
                {
                    TxtIMGBin.Text = "";
                }
            }
        }

        private void BtnScatter_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Title = "Choice Scatter File !",
                    Filter = "Scatter file  |*.txt"
                };
                if (openFileDialog.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                {
                    return;
                }
                Mediatek.Scatterfile = openFileDialog.FileName;
                DirectoryInfo directory = new DirectoryInfo(
                    openFileDialog.FileName.Replace("\\" + openFileDialog.SafeFileName, "")
                );
                foreach (FileInfo file in directory.EnumerateFiles())
                {
                    if (file.Name.Contains("preloader"))
                    {
                        Mediatek.Preloader = file.FullName;
                    }
                }
                string regions = "userarea";
                if (MtkScatter.IsSupport(Mediatek.Scatterfile))
                {
                    string path = openFileDialog.FileName.Replace(
                        "\\" + openFileDialog.SafeFileName,
                        ""
                    );

                    if (MtkScatter.CPUType.ToLower() == "emmc")
                    {
                        MtkSparse.sectsize = 512;
                    }
                    else if (MtkScatter.CPUType.ToLower() == "ufs")
                    {
                        MtkSparse.sectsize = 4096;
                    }
                    List<MtkScatter.mtk> list = MtkScatter.ScatterTable(Mediatek.Scatterfile);
                    if (list.Count > 0)
                    {
                        TxtScatter.Text = openFileDialog.SafeFileName;
                        DataViewmtk.Rows.Clear();
                        foreach (MtkScatter.mtk item in list)
                        {
                            if (item.Partition_name == "preloader")
                            {
                                regions = "boot";
                            }
                            else
                            {
                                regions = "userarea";
                            }
                            string text_Conflict = System.IO.Path.Combine(path, item.File_name);
                            if (File.Exists(text_Conflict))
                            {
                                if (item.Partition_name == "preloader")
                                {
                                    if (File.Exists(text_Conflict))
                                    {
                                        Mediatek.Preloader = text_Conflict;

                                        string s = text_Conflict;

                                        int position = s.LastIndexOf("\\");

                                        if (position > -1)
                                        {
                                            s = s.Substring(position + 1);

                                            TxtEMI.Text = s;
                                            TxtEMIOneClick.Text = s;
                                        }
                                    }
                                }
                                DataViewmtk.Rows.Add(
                                    true,
                                    regions,
                                    item.Partition_name,
                                    item.Linear_start_addr,
                                    item.Partition_size,
                                    text_Conflict
                                );
                                regions = MtkSparse.sectsize.ToString();
                            }
                            else
                            {
                                DataViewmtk.Rows.Add(
                                    false,
                                    regions,
                                    item.Partition_name,
                                    item.Linear_start_addr,
                                    item.Partition_size,
                                    "Double click to add file..."
                                );
                            }
                            //}
                        }
                    }
                }
            }
            catch { }
        }

        private async void BtnFormatUserdata_Click(object sender, EventArgs e)
        {
            if (!isMTKClientRunning)
            {
                log.Clear();
                try
                {
                    isMTKClientRunning = true;
                    var token = cts.Token;
                    if (CkBromReady.Checked)
                    {
                        await Task.Run(() => MtkTask.FormatUserdata(token));
                    }
                    else
                    {
                        await Task.Run(() => MtkTask.InitAsync(token));
                        await Task.Run(() => MtkTask.FormatUserdata(token));
                    }
                }
                catch (OperationCanceledException ex)
                {
                    Richlog("Your task was canceled.", Color.Black, false, true);
                    CkBromReady.Invoke((Action)(() => CkBromReady.Checked = false));

                    isMTKClientRunning = false;
                }
                isMTKClientRunning = false;
                Richlog(" ", Color.Black, false, true);
                Richlog("Task Completed...", Color.Black, false, true);
            }
        }

        private async void BtnFormatUserdataFRP_Click(object sender, EventArgs e)
        {
            if (!isMTKClientRunning)
            {
                log.Clear();
                try
                {
                    isMTKClientRunning = true;
                    var token = cts.Token;
                    if (CkBromReady.Checked)
                    {
                        await Task.Run(() => MtkTask.FormatUserdataFRP(token));
                    }
                    else
                    {
                        await Task.Run(() => MtkTask.InitAsync(token));
                        await Task.Run(() => MtkTask.FormatUserdataFRP(token));
                    }
                }
                catch (OperationCanceledException ex)
                {
                    Richlog("Your task was canceled.", Color.Black, false, true);
                    CkBromReady.Invoke((Action)(() => CkBromReady.Checked = false));

                    isMTKClientRunning = false;
                }
                isMTKClientRunning = false;
                Richlog(" ", Color.Black, false, true);
                Richlog("Task Completed...", Color.Black, false, true);
            }
        }

        private async void BtnFormatFromRecovery_Click(object sender, EventArgs e)
        {
            if (!isMTKClientRunning)
            {
                log.Clear();
                try
                {
                    isMTKClientRunning = true;
                    var token = cts.Token;
                    if (CkBromReady.Checked)
                    {
                        await Task.Run(() => MtkTask.FormatFromRecovery(token));
                    }
                    else
                    {
                        await Task.Run(() => MtkTask.InitAsync(token));
                        await Task.Run(() => MtkTask.FormatFromRecovery(token));
                    }
                }
                catch (OperationCanceledException ex)
                {
                    Richlog("Your task was canceled.", Color.Black, false, true);
                    CkBromReady.Invoke((Action)(() => CkBromReady.Checked = false));
                    isMTKClientRunning = false;
                }
                isMTKClientRunning = false;
                Richlog(" ", Color.Black, false, true);
                Richlog("Task Completed...", Color.Black, false, true);
            }
        }

        private async void BtnFormatFromRecoveryFRP_Click(object sender, EventArgs e)
        {
            if (!isMTKClientRunning)
            {
                log.Clear();
                try
                {
                    isMTKClientRunning = true;
                    var token = cts.Token;
                    if (CkBromReady.Checked)
                    {
                        await Task.Run(() => MtkTask.FormatFromRecoveryFRP(token));
                    }
                    else
                    {
                        await Task.Run(() => MtkTask.InitAsync(token));
                        await Task.Run(() => MtkTask.FormatFromRecoveryFRP(token));
                    }
                }
                catch (OperationCanceledException ex)
                {
                    Richlog("Your task was canceled.", Color.Black, false, true);
                    CkBromReady.Invoke((Action)(() => CkBromReady.Checked = false));
                    isMTKClientRunning = false;
                }
                isMTKClientRunning = false;
                Richlog(" ", Color.Black, false, true);
                Richlog("Task Completed...", Color.Black, false, true);
            }
        }

        private async void BtnEraseFRPMiCloud_Click(object sender, EventArgs e)
        {
            if (!isMTKClientRunning)
            {
                log.Clear();
                try
                {
                    isMTKClientRunning = true;
                    var token = cts.Token;
                    if (CkBromReady.Checked)
                    {
                        await Task.Run(() => MtkTask.EraseFRPMiCloud(token));
                    }
                    else
                    {
                        await Task.Run(() => MtkTask.InitAsync(token));
                        await Task.Run(() => MtkTask.EraseFRPMiCloud(token));
                    }
                }
                catch (OperationCanceledException ex)
                {
                    Richlog("Your task was canceled.", Color.Black, false, true);
                    CkBromReady.Invoke((Action)(() => CkBromReady.Checked = false));
                    isMTKClientRunning = false;
                }
                isMTKClientRunning = false;
                Richlog(" ", Color.Black, false, true);
                Richlog("Task Completed...", Color.Black, false, true);
            }
        }

        private async void BtnEraseFRP_Click(object sender, EventArgs e)
        {
            if (!isMTKClientRunning)
            {
                log.Clear();
                try
                {
                    isMTKClientRunning = true;
                    var token = cts.Token;
                    if (CkBromReady.Checked)
                    {
                        await Task.Run(() => MtkTask.EraseFRP(token));
                    }
                    else
                    {
                        await Task.Run(() => MtkTask.InitAsync(token));
                        await Task.Run(() => MtkTask.EraseFRP(token));
                    }
                    Richlog(" ", Color.Black, false, true);
                    Richlog("Task Completed...", Color.Black, false, true);
                }
                catch (OperationCanceledException ex)
                {
                    Richlog("Your task was canceled.", Color.Black, false, true);
                    CkBromReady.Invoke((Action)(() => CkBromReady.Checked = false));
                    isMTKClientRunning = false;
                }
                isMTKClientRunning = false;
                Richlog(" ", Color.Black, false, true);
                Richlog("Task Completed...", Color.Black, false, true);
            }
        }

        private async void BtnBackupNV_Click(object sender, EventArgs e)
        {
            if (!isMTKClientRunning)
            {
                try
                {
                    string folder = null;
                    FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog()
                    {
                        ShowNewFolderButton = true
                    };
                    if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        log.Clear();
                        folder = folderBrowserDialog.SelectedPath;
                        isMTKClientRunning = true;
                        var token = cts.Token;
                        if (CkBromReady.Checked)
                        {
                            await Task.Run(() => MtkTask.BackupNV(folder, token));
                        }
                        else
                        {
                            await Task.Run(() => MtkTask.InitAsync(token));
                            await Task.Run(() => MtkTask.BackupNV(folder, token));
                        }
                        Richlog(" ", Color.Black, false, true);
                        Richlog("Task Completed...", Color.Black, false, true);
                    }
                }
                catch (OperationCanceledException ex)
                {
                    Richlog("Your task was canceled.", Color.Black, false, true);
                    CkBromReady.Invoke((Action)(() => CkBromReady.Checked = false));
                    isMTKClientRunning = false;
                }
                isMTKClientRunning = false;
            }
        }

        private async void BtnEraseNV_Click(object sender, EventArgs e)
        {
            if (!isMTKClientRunning)
            {
                log.Clear();
                try
                {
                    isMTKClientRunning = true;
                    var token = cts.Token;
                    if (CkBromReady.Checked)
                    {
                        await Task.Run(() => MtkTask.EraseNV(token));
                    }
                    else
                    {
                        await Task.Run(() => MtkTask.InitAsync(token));
                        await Task.Run(() => MtkTask.EraseNV(token));
                    }
                    Richlog(" ", Color.Black, false, true);
                    Richlog("Task Completed...", Color.Black, false, true);
                }
                catch (OperationCanceledException ex)
                {
                    Richlog("Your task was canceled.", Color.Black, false, true);
                    CkBromReady.Invoke((Action)(() => CkBromReady.Checked = false));
                    isMTKClientRunning = false;
                }
                isMTKClientRunning = false;
            }
        }

        private void BtnClearLogs_Click(object sender, EventArgs e)
        {
            log.Clear();
        }

        private void ButtonInfo_Click(object sender, EventArgs e)
        {
            logInfo();
        }

        private void ButtonSTOP_Click(object sender, EventArgs e)
        {
            try
            {
                cts.Cancel();
                Thread.Sleep(3000);
                MtkTask.closingport();
                CkBromReady.Checked = false;
                cts.Token.ThrowIfCancellationRequested();
            }
            catch (OperationCanceledException ex)
            {
                Richlog("Task Stopped", Color.Black, false, true);
                cts = new CancellationTokenSource();
                isMTKClientRunning = false;
            }
        }
    }
}
