
using System.Windows.Forms;

namespace mtkclient
{
	public partial class Main
	{

		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.CkAutoReboot = new System.Windows.Forms.CheckBox();
            this.CkBromReady = new System.Windows.Forms.CheckBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.ComboPort = new System.Windows.Forms.ComboBox();
            this.ButtonSTOP = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label_status = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label_transferrate = new System.Windows.Forms.Label();
            this.lb_transferrate = new System.Windows.Forms.Label();
            this.label_writensize = new System.Windows.Forms.Label();
            this.lb_writensize = new System.Windows.Forms.Label();
            this.label_totalsize = new System.Windows.Forms.Label();
            this.lb_totalsize = new System.Windows.Forms.Label();
            this.log = new System.Windows.Forms.RichTextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.BtnScatter = new System.Windows.Forms.Button();
            this.BtnEMI1 = new System.Windows.Forms.Button();
            this.BtnBrowse = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.TxtScatter = new System.Windows.Forms.TextBox();
            this.TxtEMI = new System.Windows.Forms.TextBox();
            this.TxtIMGBin = new System.Windows.Forms.TextBox();
            this.BtnErasePartition = new System.Windows.Forms.Button();
            this.BtnReadPartition = new System.Windows.Forms.Button();
            this.BtnFlash = new System.Windows.Forms.Button();
            this.BtnIdentify = new System.Windows.Forms.Button();
            this.CkList = new System.Windows.Forms.CheckBox();
            this.DataViewmtk = new System.Windows.Forms.DataGridView();
            this.Column0 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.part = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.BtnEraseFRP = new System.Windows.Forms.Button();
            this.BtnFormatUserdataFRP = new System.Windows.Forms.Button();
            this.BtnEraseNV = new System.Windows.Forms.Button();
            this.BtnBackupNV = new System.Windows.Forms.Button();
            this.BtnFormatFromRecoveryFRP = new System.Windows.Forms.Button();
            this.BtnFormatFromRecovery = new System.Windows.Forms.Button();
            this.BtnFormatUserdata = new System.Windows.Forms.Button();
            this.BtnEMI2 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.TxtEMIOneClick = new System.Windows.Forms.TextBox();
            this.BtnClearLogs = new System.Windows.Forms.Button();
            this.CkAutoUnsparse = new System.Windows.Forms.CheckBox();
            this.CkAutoCrashPreloader = new System.Windows.Forms.CheckBox();
            this.CkBypassSecureBootOnly = new System.Windows.Forms.CheckBox();
            this.ButtonInfo = new System.Windows.Forms.Button();
            this.CkAutoSwitchHighSpeedUSB = new System.Windows.Forms.CheckBox();
            this.CkAutoRepairGPTFromSGPT = new System.Windows.Forms.CheckBox();
            this.CkCreateScatterBackupFile = new System.Windows.Forms.CheckBox();
            this.CkReadAndroidInfo = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataViewmtk)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // CkAutoReboot
            // 
            this.CkAutoReboot.AutoSize = true;
            this.CkAutoReboot.Checked = true;
            this.CkAutoReboot.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CkAutoReboot.Location = new System.Drawing.Point(17, 29);
            this.CkAutoReboot.Name = "CkAutoReboot";
            this.CkAutoReboot.Size = new System.Drawing.Size(92, 17);
            this.CkAutoReboot.TabIndex = 8;
            this.CkAutoReboot.Text = "Auto Reboot";
            this.CkAutoReboot.UseVisualStyleBackColor = true;
            // 
            // CkBromReady
            // 
            this.CkBromReady.AutoSize = true;
            this.CkBromReady.Location = new System.Drawing.Point(301, 53);
            this.CkBromReady.Name = "CkBromReady";
            this.CkBromReady.Size = new System.Drawing.Size(86, 17);
            this.CkBromReady.TabIndex = 8;
            this.CkBromReady.Text = "Brom Ready";
            this.CkBromReady.UseVisualStyleBackColor = true;
            this.CkBromReady.CheckedChanged += new System.EventHandler(this.CkBromReady_CheckedChanged);
            // 
            // progressBar1
            // 
            this.progressBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBar1.Location = new System.Drawing.Point(0, 503);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(1128, 23);
            this.progressBar1.TabIndex = 9;
            this.progressBar1.Value = 100;
            // 
            // ComboPort
            // 
            this.ComboPort.Dock = System.Windows.Forms.DockStyle.Top;
            this.ComboPort.FormattingEnabled = true;
            this.ComboPort.Location = new System.Drawing.Point(0, 0);
            this.ComboPort.Name = "ComboPort";
            this.ComboPort.Size = new System.Drawing.Size(1128, 21);
            this.ComboPort.TabIndex = 11;
            this.ComboPort.SelectedIndexChanged += new System.EventHandler(this.ComboPort_SelectedIndexChanged);
            // 
            // ButtonSTOP
            // 
            this.ButtonSTOP.Location = new System.Drawing.Point(1028, 445);
            this.ButtonSTOP.Name = "ButtonSTOP";
            this.ButtonSTOP.Size = new System.Drawing.Size(73, 32);
            this.ButtonSTOP.TabIndex = 0;
            this.ButtonSTOP.Text = "STOP";
            this.ButtonSTOP.UseVisualStyleBackColor = true;
            this.ButtonSTOP.Click += new System.EventHandler(this.ButtonSTOP_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label_status);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label_transferrate);
            this.panel1.Controls.Add(this.lb_transferrate);
            this.panel1.Controls.Add(this.label_writensize);
            this.panel1.Controls.Add(this.lb_writensize);
            this.panel1.Controls.Add(this.label_totalsize);
            this.panel1.Controls.Add(this.lb_totalsize);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 481);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1128, 22);
            this.panel1.TabIndex = 16;
            // 
            // label_status
            // 
            this.label_status.AutoSize = true;
            this.label_status.BackColor = System.Drawing.Color.White;
            this.label_status.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.label_status.ForeColor = System.Drawing.Color.Black;
            this.label_status.Location = new System.Drawing.Point(655, 5);
            this.label_status.Name = "label_status";
            this.label_status.Size = new System.Drawing.Size(21, 13);
            this.label_status.TabIndex = 7;
            this.label_status.Text = "OK";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(594, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Status :";
            // 
            // label_transferrate
            // 
            this.label_transferrate.AutoSize = true;
            this.label_transferrate.BackColor = System.Drawing.Color.White;
            this.label_transferrate.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_transferrate.ForeColor = System.Drawing.Color.Black;
            this.label_transferrate.Location = new System.Drawing.Point(487, 5);
            this.label_transferrate.Name = "label_transferrate";
            this.label_transferrate.Size = new System.Drawing.Size(90, 13);
            this.label_transferrate.TabIndex = 1;
            this.label_transferrate.Text = "0.00 Bytes /s   ";
            // 
            // lb_transferrate
            // 
            this.lb_transferrate.AutoSize = true;
            this.lb_transferrate.BackColor = System.Drawing.Color.White;
            this.lb_transferrate.Location = new System.Drawing.Point(397, 5);
            this.lb_transferrate.Name = "lb_transferrate";
            this.lb_transferrate.Size = new System.Drawing.Size(103, 13);
            this.lb_transferrate.TabIndex = 2;
            this.lb_transferrate.Text = "Transfer Rate : ";
            // 
            // label_writensize
            // 
            this.label_writensize.AutoSize = true;
            this.label_writensize.BackColor = System.Drawing.Color.White;
            this.label_writensize.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_writensize.ForeColor = System.Drawing.Color.Black;
            this.label_writensize.Location = new System.Drawing.Point(277, 5);
            this.label_writensize.Name = "label_writensize";
            this.label_writensize.Size = new System.Drawing.Size(99, 13);
            this.label_writensize.TabIndex = 3;
            this.label_writensize.Text = "0.00 Bytes           ";
            // 
            // lb_writensize
            // 
            this.lb_writensize.AutoSize = true;
            this.lb_writensize.BackColor = System.Drawing.Color.White;
            this.lb_writensize.Location = new System.Drawing.Point(200, 5);
            this.lb_writensize.Name = "lb_writensize";
            this.lb_writensize.Size = new System.Drawing.Size(91, 13);
            this.lb_writensize.TabIndex = 4;
            this.lb_writensize.Text = "Writen Size : ";
            // 
            // label_totalsize
            // 
            this.label_totalsize.AutoSize = true;
            this.label_totalsize.BackColor = System.Drawing.Color.White;
            this.label_totalsize.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_totalsize.ForeColor = System.Drawing.Color.Black;
            this.label_totalsize.Location = new System.Drawing.Point(82, 5);
            this.label_totalsize.Name = "label_totalsize";
            this.label_totalsize.Size = new System.Drawing.Size(99, 13);
            this.label_totalsize.TabIndex = 5;
            this.label_totalsize.Text = "0.00 Bytes           ";
            // 
            // lb_totalsize
            // 
            this.lb_totalsize.AutoSize = true;
            this.lb_totalsize.BackColor = System.Drawing.Color.White;
            this.lb_totalsize.Location = new System.Drawing.Point(13, 5);
            this.lb_totalsize.Name = "lb_totalsize";
            this.lb_totalsize.Size = new System.Drawing.Size(85, 13);
            this.lb_totalsize.TabIndex = 6;
            this.lb_totalsize.Text = "Total Size : ";
            // 
            // log
            // 
            this.log.Dock = System.Windows.Forms.DockStyle.Right;
            this.log.Location = new System.Drawing.Point(587, 21);
            this.log.Name = "log";
            this.log.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
            this.log.Size = new System.Drawing.Size(541, 460);
            this.log.TabIndex = 18;
            this.log.Text = "";
            this.log.TextChanged += new System.EventHandler(this.log_TextChanged);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(13, 52);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(564, 423);
            this.tabControl1.TabIndex = 19;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.BtnScatter);
            this.tabPage1.Controls.Add(this.BtnEMI1);
            this.tabPage1.Controls.Add(this.BtnBrowse);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.TxtScatter);
            this.tabPage1.Controls.Add(this.TxtEMI);
            this.tabPage1.Controls.Add(this.TxtIMGBin);
            this.tabPage1.Controls.Add(this.BtnErasePartition);
            this.tabPage1.Controls.Add(this.BtnReadPartition);
            this.tabPage1.Controls.Add(this.BtnFlash);
            this.tabPage1.Controls.Add(this.BtnIdentify);
            this.tabPage1.Controls.Add(this.CkList);
            this.tabPage1.Controls.Add(this.DataViewmtk);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(556, 397);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "FLASH";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // BtnScatter
            // 
            this.BtnScatter.Location = new System.Drawing.Point(486, 288);
            this.BtnScatter.Name = "BtnScatter";
            this.BtnScatter.Size = new System.Drawing.Size(64, 20);
            this.BtnScatter.TabIndex = 34;
            this.BtnScatter.Text = "Browse";
            this.BtnScatter.UseVisualStyleBackColor = true;
            this.BtnScatter.Click += new System.EventHandler(this.BtnScatter_Click);
            // 
            // BtnEMI1
            // 
            this.BtnEMI1.Location = new System.Drawing.Point(486, 314);
            this.BtnEMI1.Name = "BtnEMI1";
            this.BtnEMI1.Size = new System.Drawing.Size(64, 20);
            this.BtnEMI1.TabIndex = 34;
            this.BtnEMI1.Text = "Browse";
            this.BtnEMI1.UseVisualStyleBackColor = true;
            this.BtnEMI1.Click += new System.EventHandler(this.BtnEmi_Click);
            // 
            // BtnBrowse
            // 
            this.BtnBrowse.Location = new System.Drawing.Point(486, 341);
            this.BtnBrowse.Name = "BtnBrowse";
            this.BtnBrowse.Size = new System.Drawing.Size(64, 20);
            this.BtnBrowse.TabIndex = 35;
            this.BtnBrowse.Text = "Browse";
            this.BtnBrowse.UseVisualStyleBackColor = true;
            this.BtnBrowse.Click += new System.EventHandler(this.BtnBrowse_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 292);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(79, 13);
            this.label5.TabIndex = 32;
            this.label5.Text = "Scatter File";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 317);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(103, 13);
            this.label3.TabIndex = 32;
            this.label3.Text = "Custom Preloader";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 344);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 13);
            this.label1.TabIndex = 33;
            this.label1.Text = "*IMG *BIN Folder";
            // 
            // TxtScatter
            // 
            this.TxtScatter.Location = new System.Drawing.Point(117, 288);
            this.TxtScatter.Name = "TxtScatter";
            this.TxtScatter.Size = new System.Drawing.Size(363, 20);
            this.TxtScatter.TabIndex = 30;
            // 
            // TxtEMI
            // 
            this.TxtEMI.Location = new System.Drawing.Point(117, 314);
            this.TxtEMI.Name = "TxtEMI";
            this.TxtEMI.Size = new System.Drawing.Size(363, 20);
            this.TxtEMI.TabIndex = 30;
            // 
            // TxtIMGBin
            // 
            this.TxtIMGBin.Location = new System.Drawing.Point(117, 341);
            this.TxtIMGBin.Name = "TxtIMGBin";
            this.TxtIMGBin.Size = new System.Drawing.Size(363, 20);
            this.TxtIMGBin.TabIndex = 31;
            // 
            // BtnErasePartition
            // 
            this.BtnErasePartition.Location = new System.Drawing.Point(6, 367);
            this.BtnErasePartition.Name = "BtnErasePartition";
            this.BtnErasePartition.Size = new System.Drawing.Size(64, 24);
            this.BtnErasePartition.TabIndex = 28;
            this.BtnErasePartition.Text = "Erase";
            this.BtnErasePartition.UseVisualStyleBackColor = true;
            this.BtnErasePartition.Click += new System.EventHandler(this.BtnErasePartition_Click);
            // 
            // BtnReadPartition
            // 
            this.BtnReadPartition.Location = new System.Drawing.Point(147, 367);
            this.BtnReadPartition.Name = "BtnReadPartition";
            this.BtnReadPartition.Size = new System.Drawing.Size(64, 24);
            this.BtnReadPartition.TabIndex = 29;
            this.BtnReadPartition.Text = "Backup";
            this.BtnReadPartition.UseVisualStyleBackColor = true;
            this.BtnReadPartition.Click += new System.EventHandler(this.BtnReadPartition_Click);
            // 
            // BtnFlash
            // 
            this.BtnFlash.Location = new System.Drawing.Point(486, 367);
            this.BtnFlash.Name = "BtnFlash";
            this.BtnFlash.Size = new System.Drawing.Size(64, 24);
            this.BtnFlash.TabIndex = 26;
            this.BtnFlash.Text = "Flash";
            this.BtnFlash.UseVisualStyleBackColor = true;
            this.BtnFlash.Click += new System.EventHandler(this.BtnFlash_Click);
            // 
            // BtnIdentify
            // 
            this.BtnIdentify.Location = new System.Drawing.Point(356, 367);
            this.BtnIdentify.Name = "BtnIdentify";
            this.BtnIdentify.Size = new System.Drawing.Size(64, 24);
            this.BtnIdentify.TabIndex = 27;
            this.BtnIdentify.Text = "Identify";
            this.BtnIdentify.UseVisualStyleBackColor = true;
            this.BtnIdentify.Click += new System.EventHandler(this.BtnIdentify_Click);
            // 
            // CkList
            // 
            this.CkList.AutoSize = true;
            this.CkList.Location = new System.Drawing.Point(4, 11);
            this.CkList.Name = "CkList";
            this.CkList.Size = new System.Drawing.Size(15, 14);
            this.CkList.TabIndex = 10;
            this.CkList.UseVisualStyleBackColor = true;
            this.CkList.CheckedChanged += new System.EventHandler(this.CkList_CheckedChanged);
            // 
            // DataViewmtk
            // 
            this.DataViewmtk.AllowUserToAddRows = false;
            this.DataViewmtk.AllowUserToDeleteRows = false;
            this.DataViewmtk.BackgroundColor = System.Drawing.SystemColors.Window;
            this.DataViewmtk.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataViewmtk.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column0,
            this.Column3,
            this.part,
            this.Column1,
            this.Column2,
            this.Column4});
            this.DataViewmtk.Location = new System.Drawing.Point(-1, 6);
            this.DataViewmtk.Name = "DataViewmtk";
            this.DataViewmtk.RowHeadersVisible = false;
            this.DataViewmtk.RowTemplate.Height = 25;
            this.DataViewmtk.Size = new System.Drawing.Size(557, 271);
            this.DataViewmtk.TabIndex = 9;
            this.DataViewmtk.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataViewmtk_CellContentDoubleClick);
            // 
            // Column0
            // 
            this.Column0.HeaderText = "";
            this.Column0.Name = "Column0";
            this.Column0.Width = 20;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Region";
            this.Column3.Name = "Column3";
            this.Column3.Width = 70;
            // 
            // part
            // 
            this.part.HeaderText = "Partitions";
            this.part.Name = "part";
            this.part.Width = 80;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Start";
            this.Column1.Name = "Column1";
            this.Column1.Width = 80;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Sizes";
            this.Column2.Name = "Column2";
            this.Column2.Width = 80;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "Filename";
            this.Column4.Name = "Column4";
            this.Column4.Width = 280;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.BtnEraseFRP);
            this.tabPage2.Controls.Add(this.BtnFormatUserdataFRP);
            this.tabPage2.Controls.Add(this.BtnEraseNV);
            this.tabPage2.Controls.Add(this.BtnBackupNV);
            this.tabPage2.Controls.Add(this.BtnFormatFromRecoveryFRP);
            this.tabPage2.Controls.Add(this.BtnFormatFromRecovery);
            this.tabPage2.Controls.Add(this.BtnFormatUserdata);
            this.tabPage2.Controls.Add(this.BtnEMI2);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.TxtEMIOneClick);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(556, 397);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "OneClick";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // BtnEraseFRP
            // 
            this.BtnEraseFRP.Location = new System.Drawing.Point(144, 171);
            this.BtnEraseFRP.Name = "BtnEraseFRP";
            this.BtnEraseFRP.Size = new System.Drawing.Size(268, 32);
            this.BtnEraseFRP.TabIndex = 37;
            this.BtnEraseFRP.Text = "Erase FRP";
            this.BtnEraseFRP.UseVisualStyleBackColor = true;
            this.BtnEraseFRP.Click += new System.EventHandler(this.BtnEraseFRP_Click);
            // 
            // BtnFormatUserdataFRP
            // 
            this.BtnFormatUserdataFRP.Location = new System.Drawing.Point(280, 68);
            this.BtnFormatUserdataFRP.Name = "BtnFormatUserdataFRP";
            this.BtnFormatUserdataFRP.Size = new System.Drawing.Size(268, 32);
            this.BtnFormatUserdataFRP.TabIndex = 37;
            this.BtnFormatUserdataFRP.Text = "Format Userdata + Erase FRP";
            this.BtnFormatUserdataFRP.UseVisualStyleBackColor = true;
            this.BtnFormatUserdataFRP.Click += new System.EventHandler(this.BtnFormatUserdataFRP_Click);
            // 
            // BtnEraseNV
            // 
            this.BtnEraseNV.Location = new System.Drawing.Point(280, 244);
            this.BtnEraseNV.Name = "BtnEraseNV";
            this.BtnEraseNV.Size = new System.Drawing.Size(268, 32);
            this.BtnEraseNV.TabIndex = 37;
            this.BtnEraseNV.Text = "Erase NV";
            this.BtnEraseNV.UseVisualStyleBackColor = true;
            this.BtnEraseNV.Click += new System.EventHandler(this.BtnEraseNV_Click);
            // 
            // BtnBackupNV
            // 
            this.BtnBackupNV.Location = new System.Drawing.Point(6, 244);
            this.BtnBackupNV.Name = "BtnBackupNV";
            this.BtnBackupNV.Size = new System.Drawing.Size(268, 32);
            this.BtnBackupNV.TabIndex = 37;
            this.BtnBackupNV.Text = "Backup NV";
            this.BtnBackupNV.UseVisualStyleBackColor = true;
            this.BtnBackupNV.Click += new System.EventHandler(this.BtnBackupNV_Click);
            // 
            // BtnFormatFromRecoveryFRP
            // 
            this.BtnFormatFromRecoveryFRP.Location = new System.Drawing.Point(280, 106);
            this.BtnFormatFromRecoveryFRP.Name = "BtnFormatFromRecoveryFRP";
            this.BtnFormatFromRecoveryFRP.Size = new System.Drawing.Size(268, 32);
            this.BtnFormatFromRecoveryFRP.TabIndex = 37;
            this.BtnFormatFromRecoveryFRP.Text = "Format From Recovery + Erase FRP";
            this.BtnFormatFromRecoveryFRP.UseVisualStyleBackColor = true;
            this.BtnFormatFromRecoveryFRP.Click += new System.EventHandler(this.BtnFormatFromRecoveryFRP_Click);
            // 
            // BtnFormatFromRecovery
            // 
            this.BtnFormatFromRecovery.Location = new System.Drawing.Point(6, 106);
            this.BtnFormatFromRecovery.Name = "BtnFormatFromRecovery";
            this.BtnFormatFromRecovery.Size = new System.Drawing.Size(268, 32);
            this.BtnFormatFromRecovery.TabIndex = 37;
            this.BtnFormatFromRecovery.Text = "Format From Recovery";
            this.BtnFormatFromRecovery.UseVisualStyleBackColor = true;
            this.BtnFormatFromRecovery.Click += new System.EventHandler(this.BtnFormatFromRecovery_Click);
            // 
            // BtnFormatUserdata
            // 
            this.BtnFormatUserdata.Location = new System.Drawing.Point(6, 68);
            this.BtnFormatUserdata.Name = "BtnFormatUserdata";
            this.BtnFormatUserdata.Size = new System.Drawing.Size(268, 32);
            this.BtnFormatUserdata.TabIndex = 37;
            this.BtnFormatUserdata.Text = "Format Userdata";
            this.BtnFormatUserdata.UseVisualStyleBackColor = true;
            this.BtnFormatUserdata.Click += new System.EventHandler(this.BtnFormatUserdata_Click);
            // 
            // BtnEMI2
            // 
            this.BtnEMI2.Location = new System.Drawing.Point(451, 359);
            this.BtnEMI2.Name = "BtnEMI2";
            this.BtnEMI2.Size = new System.Drawing.Size(99, 32);
            this.BtnEMI2.TabIndex = 37;
            this.BtnEMI2.Text = "Browse";
            this.BtnEMI2.UseVisualStyleBackColor = true;
            this.BtnEMI2.Click += new System.EventHandler(this.BtnEMI2_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 369);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 13);
            this.label4.TabIndex = 36;
            this.label4.Text = "Custom EMI";
            // 
            // TxtEMIOneClick
            // 
            this.TxtEMIOneClick.Location = new System.Drawing.Point(117, 366);
            this.TxtEMIOneClick.Name = "TxtEMIOneClick";
            this.TxtEMIOneClick.Size = new System.Drawing.Size(328, 20);
            this.TxtEMIOneClick.TabIndex = 35;
            // 
            // BtnClearLogs
            // 
            this.BtnClearLogs.Location = new System.Drawing.Point(1075, 30);
            this.BtnClearLogs.Name = "BtnClearLogs";
            this.BtnClearLogs.Size = new System.Drawing.Size(26, 26);
            this.BtnClearLogs.TabIndex = 28;
            this.BtnClearLogs.Text = "X";
            this.BtnClearLogs.UseVisualStyleBackColor = true;
            this.BtnClearLogs.Click += new System.EventHandler(this.BtnClearLogs_Click);
            // 
            // CkAutoUnsparse
            // 
            this.CkAutoUnsparse.AutoSize = true;
            this.CkAutoUnsparse.Checked = true;
            this.CkAutoUnsparse.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CkAutoUnsparse.Location = new System.Drawing.Point(301, 29);
            this.CkAutoUnsparse.Name = "CkAutoUnsparse";
            this.CkAutoUnsparse.Size = new System.Drawing.Size(104, 17);
            this.CkAutoUnsparse.TabIndex = 29;
            this.CkAutoUnsparse.Text = "Auto Unsparse";
            this.CkAutoUnsparse.UseVisualStyleBackColor = true;
            // 
            // CkAutoCrashPreloader
            // 
            this.CkAutoCrashPreloader.AutoSize = true;
            this.CkAutoCrashPreloader.Checked = true;
            this.CkAutoCrashPreloader.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CkAutoCrashPreloader.Location = new System.Drawing.Point(131, 29);
            this.CkAutoCrashPreloader.Name = "CkAutoCrashPreloader";
            this.CkAutoCrashPreloader.Size = new System.Drawing.Size(146, 17);
            this.CkAutoCrashPreloader.TabIndex = 29;
            this.CkAutoCrashPreloader.Text = "Auto Crash Preloader";
            this.CkAutoCrashPreloader.UseVisualStyleBackColor = true;
            // 
            // CkBypassSecureBootOnly
            // 
            this.CkBypassSecureBootOnly.AutoSize = true;
            this.CkBypassSecureBootOnly.Location = new System.Drawing.Point(131, 54);
            this.CkBypassSecureBootOnly.Name = "CkBypassSecureBootOnly";
            this.CkBypassSecureBootOnly.Size = new System.Drawing.Size(164, 17);
            this.CkBypassSecureBootOnly.TabIndex = 8;
            this.CkBypassSecureBootOnly.Text = "Bypass Secure Boot Only";
            this.CkBypassSecureBootOnly.UseVisualStyleBackColor = true;
            this.CkBypassSecureBootOnly.CheckedChanged += new System.EventHandler(this.CkBypassSecureBootOnly_CheckedChanged);
            // 
            // ButtonInfo
            // 
            this.ButtonInfo.Font = new System.Drawing.Font("Consolas", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButtonInfo.Location = new System.Drawing.Point(1043, 29);
            this.ButtonInfo.Name = "ButtonInfo";
            this.ButtonInfo.Size = new System.Drawing.Size(26, 26);
            this.ButtonInfo.TabIndex = 28;
            this.ButtonInfo.Text = "i";
            this.ButtonInfo.UseVisualStyleBackColor = true;
            this.ButtonInfo.Click += new System.EventHandler(this.ButtonInfo_Click);
            // 
            // CkAutoSwitchHighSpeedUSB
            // 
            this.CkAutoSwitchHighSpeedUSB.AutoSize = true;
            this.CkAutoSwitchHighSpeedUSB.Checked = true;
            this.CkAutoSwitchHighSpeedUSB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CkAutoSwitchHighSpeedUSB.Location = new System.Drawing.Point(933, 3);
            this.CkAutoSwitchHighSpeedUSB.Name = "CkAutoSwitchHighSpeedUSB";
            this.CkAutoSwitchHighSpeedUSB.Size = new System.Drawing.Size(182, 17);
            this.CkAutoSwitchHighSpeedUSB.TabIndex = 29;
            this.CkAutoSwitchHighSpeedUSB.Text = "Auto Switch High-Speed USB";
            this.CkAutoSwitchHighSpeedUSB.UseVisualStyleBackColor = true;
            // 
            // CkAutoRepairGPTFromSGPT
            // 
            this.CkAutoRepairGPTFromSGPT.AutoSize = true;
            this.CkAutoRepairGPTFromSGPT.Checked = true;
            this.CkAutoRepairGPTFromSGPT.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CkAutoRepairGPTFromSGPT.Location = new System.Drawing.Point(405, 28);
            this.CkAutoRepairGPTFromSGPT.Name = "CkAutoRepairGPTFromSGPT";
            this.CkAutoRepairGPTFromSGPT.Size = new System.Drawing.Size(176, 17);
            this.CkAutoRepairGPTFromSGPT.TabIndex = 29;
            this.CkAutoRepairGPTFromSGPT.Text = "Auto Repair GPT From SGPT";
            this.CkAutoRepairGPTFromSGPT.UseVisualStyleBackColor = true;
            // 
            // CkCreateScatterBackupFile
            // 
            this.CkCreateScatterBackupFile.AutoSize = true;
            this.CkCreateScatterBackupFile.Checked = true;
            this.CkCreateScatterBackupFile.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CkCreateScatterBackupFile.Location = new System.Drawing.Point(405, 53);
            this.CkCreateScatterBackupFile.Name = "CkCreateScatterBackupFile";
            this.CkCreateScatterBackupFile.Size = new System.Drawing.Size(182, 17);
            this.CkCreateScatterBackupFile.TabIndex = 29;
            this.CkCreateScatterBackupFile.Text = "Create Scatter Backup File";
            this.CkCreateScatterBackupFile.UseVisualStyleBackColor = true;
            // 
            // CkReadAndroidInfo
            // 
            this.CkReadAndroidInfo.AutoSize = true;
            this.CkReadAndroidInfo.Checked = true;
            this.CkReadAndroidInfo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CkReadAndroidInfo.Location = new System.Drawing.Point(796, 3);
            this.CkReadAndroidInfo.Name = "CkReadAndroidInfo";
            this.CkReadAndroidInfo.Size = new System.Drawing.Size(128, 17);
            this.CkReadAndroidInfo.TabIndex = 29;
            this.CkReadAndroidInfo.Text = "Read Android Info";
            this.CkReadAndroidInfo.UseVisualStyleBackColor = true;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(1128, 526);
            this.Controls.Add(this.CkBypassSecureBootOnly);
            this.Controls.Add(this.CkBromReady);
            this.Controls.Add(this.CkAutoCrashPreloader);
            this.Controls.Add(this.CkAutoSwitchHighSpeedUSB);
            this.Controls.Add(this.CkCreateScatterBackupFile);
            this.Controls.Add(this.CkReadAndroidInfo);
            this.Controls.Add(this.CkAutoRepairGPTFromSGPT);
            this.Controls.Add(this.CkAutoUnsparse);
            this.Controls.Add(this.ButtonInfo);
            this.Controls.Add(this.BtnClearLogs);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.ButtonSTOP);
            this.Controls.Add(this.log);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.ComboPort);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.CkAutoReboot);
            this.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "iReverse MTK Client Lite - CSharp Version [ Non Python ] - Minimalist";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataViewmtk)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		private ProgressBar progressBar1;
		public ComboBox ComboPort;
		private Button ButtonSTOP;
		public CheckBox CkBromReady;
		private Panel panel1;
		public Label label_status;
		public Label label_transferrate;
		private Label lb_transferrate;
		public Label label_writensize;
		private Label lb_writensize;
		public Label label_totalsize;
		private Label lb_totalsize;
		public RichTextBox log;
		public Label label2;
		public CheckBox CkAutoReboot;
		private TabControl tabControl1;
		private TabPage tabPage1;
		public Button BtnEMI1;
		public Button BtnBrowse;
		private Label label3;
		private Label label1;
		public TextBox TxtEMI;
		private TextBox TxtIMGBin;
		public Button BtnErasePartition;
		public Button BtnReadPartition;
		public Button BtnFlash;
		public Button BtnIdentify;
		private CheckBox CkList;
		public DataGridView DataViewmtk;
		private TabPage tabPage2;
		public Button BtnFormatUserdataFRP;
		public Button BtnEraseNV;
		public Button BtnBackupNV;
		public Button BtnFormatFromRecoveryFRP;
		public Button BtnFormatFromRecovery;
		public Button BtnFormatUserdata;
		public Button BtnEMI2;
		private Label label4;
		public TextBox TxtEMIOneClick;
		public Button BtnEraseFRP;
		public Button BtnClearLogs;
		public Button BtnScatter;
		private Label label5;
		public TextBox TxtScatter;
		public CheckBox CkAutoUnsparse;
		public CheckBox CkAutoCrashPreloader;
		public CheckBox CkBypassSecureBootOnly;
		public Button ButtonInfo;
		public CheckBox CkAutoSwitchHighSpeedUSB;
		public CheckBox CkAutoRepairGPTFromSGPT;
		private DataGridViewCheckBoxColumn Column0;
		private DataGridViewTextBoxColumn Column3;
		private DataGridViewTextBoxColumn part;
		private DataGridViewTextBoxColumn Column1;
		private DataGridViewTextBoxColumn Column2;
		private DataGridViewTextBoxColumn Column4;
		public CheckBox CkCreateScatterBackupFile;
		public CheckBox CkReadAndroidInfo;
    }
}

