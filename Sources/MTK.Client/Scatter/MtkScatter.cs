using mtkclient.library;
using mtkclient.library.xflash;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using static mtkclient.MTK.Client.utils;

namespace mtkclient.MTK.Client.Scatter
{
    internal class MtkScatter
    {
        public static string CPU { get; set; }
        public static string CPUType { get; set; }
        public static string Cache { get; set; }
        public static string Userdata { get; set; }
        public static string Cachepath { get; set; }
        public static string Userpath { get; set; }

        public static string ScatterBuilder()
        {
            string ScatterLine = null;
            ScatterLine = string.Concat(
                "############################################################################################################"
                    + Environment.NewLine
            );
            ScatterLine += string.Concat("#" + Environment.NewLine);
            ScatterLine += string.Concat("#  General Setting" + Environment.NewLine);
            ScatterLine += string.Concat("#" + Environment.NewLine);
            ScatterLine += string.Concat(
                "############################################################################################################"
                    + Environment.NewLine
            );
            ScatterLine += string.Concat("- general: MTK_PLATFORM_CFG" + Environment.NewLine);
            ScatterLine += string.Concat("  info: " + Environment.NewLine);
            ScatterLine += string.Concat("    - config_version: V1.1.2" + Environment.NewLine);
            ScatterLine += string.Concat(
                "      platform: " + Mediatek.Platform + Environment.NewLine
            );
            ScatterLine += string.Concat(
                "      project: iReverse_MTKClient_"
                    + Mediatek.PreloaderName.Replace("preloader_", "").Replace(".bin", "").ToUpper()
                    + Environment.NewLine
            );
            ScatterLine += string.Concat(
                "      storage: " + Mediatek.Storage + Environment.NewLine
            );
            ScatterLine += string.Concat("      boot_channel: MSDC_0" + Environment.NewLine);
            ScatterLine += string.Concat("      block_size: 0x20000" + Environment.NewLine);
            ScatterLine += string.Concat(
                "############################################################################################################"
                    + Environment.NewLine
            );
            ScatterLine += string.Concat("#" + Environment.NewLine);
            ScatterLine += string.Concat(
                "#  " + Mediatek.Storage + " Layout Setting" + Environment.NewLine
            );
            ScatterLine += string.Concat("#" + Environment.NewLine);
            ScatterLine += string.Concat(
                "############################################################################################################"
                    + Environment.NewLine
            );

            int NumPart = 0;

            foreach (DataGridViewRow item in Main.SharedUI.DataViewmtk.Rows)
            {
                ScatterLine += string.Concat(
                    "- partition_index: SYS" + NumPart + Environment.NewLine
                );
                ScatterLine += string.Concat(
                    "  partition_name: " + item.Cells[2].Value.ToString() + Environment.NewLine
                );
                if (item.Cells[2].Value.ToString() == "preloader")
                {
                    ScatterLine += string.Concat(
                        "  file_name: " + Mediatek.PreloaderName + Environment.NewLine
                    );
                }
                else
                {
                    ScatterLine += string.Concat(
                        "  file_name: "
                            + item.Cells[2].Value.ToString()
                            + ".img"
                            + Environment.NewLine
                    );
                }
                ScatterLine += string.Concat("  is_download: true" + Environment.NewLine);
                if (item.Cells[2].Value.ToString() == "preloader")
                {
                    ScatterLine += string.Concat("  type: SV5_BL_BIN" + Environment.NewLine);
                }
                else
                {
                    ScatterLine += string.Concat("  type: NORMAL_ROM" + Environment.NewLine);
                }
                ScatterLine += string.Concat(
                    "  linear_start_addr: " + item.Cells[3].Value.ToString() + Environment.NewLine
                );
                ScatterLine += string.Concat(
                    "  physical_start_addr: " + item.Cells[3].Value.ToString() + Environment.NewLine
                );
                ScatterLine += string.Concat(
                    "  partition_size: " + item.Cells[4].Value.ToString() + Environment.NewLine
                );
                if (item.Cells[2].Value.ToString() == "preloader")
                {
                    ScatterLine += string.Concat(
                        "  region: " + Mediatek.Storage + "_BOOT1_BOOT2" + Environment.NewLine
                    );
                }
                else
                {
                    ScatterLine += string.Concat(
                        "  region: " + Mediatek.Storage + "_USER" + Environment.NewLine
                    );
                }
                ScatterLine += string.Concat(
                    "  storage: HW_STORAGE_" + Mediatek.Storage + Environment.NewLine
                );
                ScatterLine += string.Concat("  boundary_check: true" + Environment.NewLine);
                ScatterLine += string.Concat("  is_reserved: false" + Environment.NewLine);
                ScatterLine += string.Concat(
                    ScatterOpr(item.Cells[2].Value.ToString()) + Environment.NewLine
                );
                ScatterLine += string.Concat("  is_upgradable: true" + Environment.NewLine);
                ScatterLine += string.Concat("  empty_boot_needed: false" + Environment.NewLine);
                ScatterLine += string.Concat("  combo_partsize_check: false" + Environment.NewLine);
                ScatterLine += string.Concat("  reserve: 0x00" + Environment.NewLine);
                ScatterLine += string.Concat(Environment.NewLine);

                NumPart += 1;
            }

            return ScatterLine;
        }

        public static string ScatterBuilderGPT(MtkGpt gpt)
        {
            string ScatterLine = null;
            if (gpt.Partitions.Length > 0)
            {
                ScatterLine = string.Concat(
                    "############################################################################################################"
                        + Environment.NewLine
                );
                ScatterLine += string.Concat("#" + Environment.NewLine);
                ScatterLine += string.Concat("#  General Setting" + Environment.NewLine);
                ScatterLine += string.Concat("#" + Environment.NewLine);
                ScatterLine += string.Concat(
                    "############################################################################################################"
                        + Environment.NewLine
                );
                ScatterLine += string.Concat("- general: MTK_PLATFORM_CFG" + Environment.NewLine);
                ScatterLine += string.Concat("  info: " + Environment.NewLine);
                ScatterLine += string.Concat("    - config_version: V1.1.2" + Environment.NewLine);
                ScatterLine += string.Concat(
                    "      platform: " + Mediatek.Platform + Environment.NewLine
                );
                ScatterLine += string.Concat(
                    "      project: iReverse_MTKClient_"
                        + Mediatek.PreloaderName
                            .Replace("preloader_", "")
                            .Replace(".bin", "")
                            .ToUpper()
                        + Environment.NewLine
                );
                ScatterLine += string.Concat(
                    "      storage: " + Mediatek.Storage + Environment.NewLine
                );
                ScatterLine += string.Concat("      boot_channel: MSDC_0" + Environment.NewLine);
                ScatterLine += string.Concat("      block_size: 0x20000" + Environment.NewLine);
                ScatterLine += string.Concat(
                    "############################################################################################################"
                        + Environment.NewLine
                );
                ScatterLine += string.Concat("#" + Environment.NewLine);
                ScatterLine += string.Concat(
                    "#  " + Mediatek.Storage + " Layout Setting" + Environment.NewLine
                );
                ScatterLine += string.Concat("#" + Environment.NewLine);
                ScatterLine += string.Concat(
                    "############################################################################################################"
                        + Environment.NewLine
                );

                int NumPart = 0;

                ScatterLine += string.Concat(
                    "- partition_index: SYS" + NumPart + Environment.NewLine
                );
                ScatterLine += string.Concat("  partition_name: preloader" + Environment.NewLine);
                ScatterLine += string.Concat(
                    "  file_name: " + Mediatek.PreloaderName + Environment.NewLine
                );
                ScatterLine += string.Concat("  is_download: true" + Environment.NewLine);
                ScatterLine += string.Concat("  type: SV5_BL_BIN" + Environment.NewLine);
                ScatterLine += string.Concat("  linear_start_addr: 0x0" + Environment.NewLine);
                ScatterLine += string.Concat("  physical_start_addr: 0x0" + Environment.NewLine);
                ScatterLine += string.Concat(
                    "  partition_size: " + LongToHex(Mediatek.BootSize) + Environment.NewLine
                );
                ScatterLine += string.Concat(
                    "  region: " + Mediatek.Storage + "_BOOT1_BOOT2" + Environment.NewLine
                );
                ScatterLine += string.Concat(
                    "  storage: HW_STORAGE_" + Mediatek.Storage + Environment.NewLine
                );
                ScatterLine += string.Concat("  boundary_check: true" + Environment.NewLine);
                ScatterLine += string.Concat("  is_reserved: false" + Environment.NewLine);
                ScatterLine += string.Concat(ScatterOpr("preloader") + Environment.NewLine);
                ScatterLine += string.Concat("  is_upgradable: true" + Environment.NewLine);
                ScatterLine += string.Concat("  empty_boot_needed: false" + Environment.NewLine);
                ScatterLine += string.Concat("  combo_partsize_check: false" + Environment.NewLine);
                ScatterLine += string.Concat("  reserve: 0x00" + Environment.NewLine);
                ScatterLine += string.Concat(Environment.NewLine);

                NumPart += 1;

                foreach (var item in gpt.Partitions)
                {
                    ScatterLine += string.Concat(
                        "- partition_index: SYS" + NumPart + Environment.NewLine
                    );
                    ScatterLine += string.Concat(
                        "  partition_name: " + item.Name + Environment.NewLine
                    );
                    ScatterLine += string.Concat(
                        "  file_name: " + item.Name + ".img" + Environment.NewLine
                    );
                    ScatterLine += string.Concat("  is_download: true" + Environment.NewLine);
                    ScatterLine += string.Concat("  type: SV5_BL_BIN" + Environment.NewLine);
                    ScatterLine += string.Concat(
                        "  linear_start_addr: "
                            + LongToHex(item.FirstLba * MtkSparse.sectsize)
                            + Environment.NewLine
                    );
                    ScatterLine += string.Concat(
                        "  physical_start_addr: "
                            + LongToHex(item.FirstLba * MtkSparse.sectsize)
                            + Environment.NewLine
                    );
                    ScatterLine += string.Concat(
                        "  partition_size: "
                            + LongToHex(item.SectorCount * MtkSparse.sectsize)
                            + Environment.NewLine
                    );
                    ScatterLine += string.Concat(
                        "  region: " + Mediatek.Storage + "_USER" + Environment.NewLine
                    );
                    ScatterLine += string.Concat(
                        "  storage: HW_STORAGE_" + Mediatek.Storage + Environment.NewLine
                    );
                    ScatterLine += string.Concat("  boundary_check: true" + Environment.NewLine);
                    ScatterLine += string.Concat("  is_reserved: false" + Environment.NewLine);
                    ScatterLine += string.Concat(ScatterOpr(item.Name) + Environment.NewLine);
                    ScatterLine += string.Concat("  is_upgradable: true" + Environment.NewLine);
                    ScatterLine += string.Concat(
                        "  empty_boot_needed: false" + Environment.NewLine
                    );
                    ScatterLine += string.Concat(
                        "  combo_partsize_check: false" + Environment.NewLine
                    );
                    ScatterLine += string.Concat("  reserve: 0x00" + Environment.NewLine);
                    ScatterLine += string.Concat(Environment.NewLine);

                    NumPart += 1;
                }
            }
            else
            {
                ScatterLine = "";
            }
            return ScatterLine;
        }

        private static string ScatterOpr(string PartName)
        {
            if (PartName == "preloader")
            {
                return "  operation_type: BOOTLOADERS";
            }
            else if (PartName == "proinfo")
            {
                return "  operation_type: PROTECTED";
            }
            else if (PartName == "nvcfg")
            {
                return "  operation_type: PROTECTED";
            }
            else if (PartName == "persist")
            {
                return "  operation_type: PROTECTED";
            }
            else if (PartName == "protect1")
            {
                return "  operation_type: PROTECTED";
            }
            else if (PartName == "protect2")
            {
                return "  operation_type: PROTECTED";
            }
            else if (PartName == "pgpt")
            {
                return "  operation_type: INVISIBLE";
            }
            else if (PartName == "boot_para")
            {
                return "  operation_type: INVISIBLE";
            }
            else if (PartName == "para")
            {
                return "  operation_type: INVISIBLE";
            }
            else if (PartName == "expdb")
            {
                return "  operation_type: INVISIBLE";
            }
            else if (PartName == "frp")
            {
                return "  operation_type: INVISIBLE";
            }
            else if (PartName == "nvdata")
            {
                return "  operation_type: INVISIBLE";
            }
            else if (PartName == "metadata")
            {
                return "  operation_type: INVISIBLE";
            }
            else if (PartName == "md_udc")
            {
                return "  operation_type: INVISIBLE";
            }
            else if (PartName == "seccfg")
            {
                return "  operation_type: INVISIBLE";
            }
            else if (PartName == "persist")
            {
                return "  operation_type: INVISIBLE";
            }
            else if (PartName == "sec1")
            {
                return "  operation_type: INVISIBLE";
            }
            else if (PartName == "efuse")
            {
                return "  operation_type: INVISIBLE";
            }
            else if (PartName == "gz1")
            {
                return "  operation_type: INVISIBLE";
            }
            else if (PartName == "pad")
            {
                return "  operation_type: INVISIBLE";
            }
            else if (PartName == "devinfo")
            {
                return "  operation_type: INVISIBLE";
            }
            else if (PartName == "otp")
            {
                return "  operation_type: RESERVED";
            }
            else if (PartName == "flashinfo")
            {
                return "  operation_type: RESERVED";
            }
            else if (PartName == "sgpt")
            {
                return "  operation_type: RESERVED";
            }
            else
            {
                return "  operation_type: UPDATE";
            }
        }

        public static bool IsSupport(string scatter)
        {
            bool result = false;
            try
            {
                bool flag = false;
                string s = File.ReadAllText(scatter);
                using (StringReader stringReader = new StringReader(s))
                {
                    while (stringReader.Peek() != -1)
                    {
                        string text = stringReader.ReadLine();
                        if (text.Contains("platform:"))
                        {
                            CPU = text.Substring(text.IndexOf(":") + 2);
                            flag = true;
                        }
                        else if (text.Contains("storage: EMMC"))
                        {
                            CPUType = "EMMC";
                        }
                        else if (text.Contains("storage: NAND"))
                        {
                            CPUType = "NAND";
                        }
                        else if (text.Contains("storage: UFS"))
                        {
                            CPUType = "UFS";
                        }
                    }
                }
                result = flag;
            }
            catch
            {
                Console.WriteLine("Scatter cant support !");
            }
            return result;
        }

        public class mtk
        {
            public string Partition_index;
            public string Partition_name;
            public string File_name;
            public string Is_download;
            public string Linear_start_addr;
            public string Partition_size;

            public mtk(
                string Partition_index,
                string Partition_name,
                string File_name,
                string Is_download,
                string Linear_start_addr,
                string Partition_size
            )
            {
                this.Partition_index = Partition_index;
                this.Partition_name = Partition_name;
                this.File_name = File_name;
                this.Is_download = Is_download;
                this.Linear_start_addr = Linear_start_addr;
                this.Partition_size = Partition_size;
            }
        }

        public static List<mtk> ScatterTable(string Scatterfile)
        {
            List<mtk> list = new List<mtk>();
            string text = File.ReadAllText(Scatterfile)
                .Replace("- partition_index:", "+ partition_index:");
            string[] array = text.Split(new char[] { '+' });
            foreach (string text2 in array)
            {
                if (text2.Contains("partition_name"))
                {
                    string partition_index = "";
                    string partition_name = "";
                    string file_name = "";
                    string is_download = "";
                    string linear_start_addr = "";
                    string partition_size = "";
                    using (StringReader stringReader = new StringReader(text2))
                    {
                        while (stringReader.Peek() != -1)
                        {
                            string text3 = stringReader.ReadLine();
                            if (text3.Contains("partition_index"))
                            {
                                partition_index = text3
                                    .Substring(text3.IndexOf(":") + 2)
                                    .Replace("SYS", "");
                            }
                            if (text3.Contains("partition_name"))
                            {
                                partition_name = text3.Substring(text3.IndexOf(":") + 2);
                            }
                            if (text3.Contains("file_name"))
                            {
                                file_name = text3.Substring(text3.IndexOf(":") + 2);
                            }
                            if (text3.Contains("is_download"))
                            {
                                is_download = text3.Substring(text3.IndexOf(":") + 2);
                            }
                            if (text3.Contains("linear_start_addr"))
                            {
                                linear_start_addr = text3.Substring(text3.IndexOf(":") + 2);
                            }
                            if (text3.Contains("partition_size"))
                            {
                                partition_size = text3.Substring(text3.IndexOf(":") + 2);
                            }
                        }
                    }
                    list.Add(
                        new mtk(
                            partition_index,
                            partition_name,
                            file_name,
                            is_download,
                            linear_start_addr,
                            partition_size
                        )
                    );
                }
            }
            return list;
        }

        public class Firmware
        {
            public string Index { get; set; }
            public string Filepath { get; set; }

            public Firmware(string Index, string Filepath)
            {
                this.Index = Index;
                this.Filepath = Filepath;
            }
        }
    }

    public class Mediatek
    {
        public static string DA { get; set; }
        public static string Auth { get; set; }
        public static string Scatterfile { get; set; }
        public static long BootSize { get; set; }
        public static string Platform { get; set; }
        public static string Preloader { get; set; }
        public static string PreloaderName { get; set; }
        public static byte[] PreloaderEmi { get; set; }
        public static string Connection { get; set; }
        public static string Preloaderunlock { get; set; }
        public static string Savepartition { get; set; }
        public static string Storage { get; set; }
    }
}
