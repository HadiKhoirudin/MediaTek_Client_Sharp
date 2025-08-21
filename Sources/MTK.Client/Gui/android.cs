using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using static mtkclient.gui;

namespace mtkclient
{
    public class android
    {
        public static Task Prepare_ReadInfoIMG(CancellationToken cancelToken)
        {
            cancelToken.ThrowIfCancellationRequested();
            if (!File.Exists(sourcefile.AndroidPath))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(sourcefile.AndroidPath));
                File.WriteAllBytes(sourcefile.AndroidPath, Properties.Resources.C4);
            }
            if (File.Exists(sourcefile.Dumped))
            {
                File.Delete(sourcefile.Dumped);
            }
            return Task.CompletedTask;
        }

        public static async Task ReadInfoIMG(CancellationToken cancelToken)
        {
            cancelToken.ThrowIfCancellationRequested();
            if (File.Exists(sourcefile.Directorypath + "/boot.img"))
            {
                Richlog("Please wait ...", Color.Black, false, true);

                await AndroidUnpack(
                    Path.GetFileName(sourcefile.Dumped),
                    Path.GetDirectoryName(sourcefile.AndroidPath) + "\\initrd\\",
                    cancelToken
                );

                Richlog(" ", Color.Black, false, true);
            }
            Main.ProcessBar(100);
            return;
        }

        public static async Task AndroidUnpack(
            string path,
            string filepath,
            CancellationToken cancelToken
        )
        {
            await ImageUnpackInfo(
                string.Concat(new string[] { "--unpack-bootimg", " ", path }),
                filepath,
                cancelToken
            );
            return;
        }

        public static Task ImageUnpackInfo(string cmd, string path, CancellationToken cancelToken)
        {
            cancelToken.ThrowIfCancellationRequested();
            bool flag = false;
            string array = "";
            string array1 = "";
            string array2 = "";
            string array3 = "";
            string array4 = "";
            string array5 = "";
            string array6 = "";
            string array7 = "";
            string array8 = "";
            string array9 = "";
            string array10 = "";
            string array11 = "";
            string array12 = "";
            string FilePath = string.Empty;
            Richlog("", Color.Black, false, true);
            ProcessStartInfo startInfo = new ProcessStartInfo(sourcefile.AndroidPath, cmd)
            {
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = false,
                Verb = "runas",
                WorkingDirectory = System.IO.Path.GetDirectoryName(sourcefile.AndroidPath),
                RedirectStandardError = true,
                RedirectStandardOutput = true
            };

            using (Process process = Process.Start(startInfo))
            {
                Console.WriteLine(cmd);
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                cancelToken.ThrowIfCancellationRequested();
                long n = 0;
                long t = 0;
                process.OutputDataReceived += (object sender, DataReceivedEventArgs e) =>
                {
                    cancelToken.ThrowIfCancellationRequested();
                    Console.WriteLine(e.Data);

                    if (File.Exists(path + "\\system\\build.prop"))
                    {
                        FilePath = path + "\\system\\build.prop";
                        string[] str = File.ReadAllLines(FilePath);
                        t = str.Length;
                        for (int i = 0; i < str.Length; i++)
                        {
                            if (str[i].Contains("manufacturer="))
                            {
                                flag = true;
                                break;
                            }
                        }
                    }
                    else if (File.Exists(path + "\\vendor\\build.prop"))
                    {
                        FilePath = path + "\\vendor\\build.prop";
                        string[] str2 = File.ReadAllLines(FilePath);
                        t = str2.Length;
                        for (int j = 0; j < str2.Length; j++)
                        {
                            if (str2[j].Contains("manufacturer="))
                            {
                                flag = true;
                                break;
                            }
                        }
                    }
                    else if (File.Exists(path + "prop.default"))
                    {
                        FilePath = path + "prop.default";
                        string[] str3 = File.ReadAllLines(FilePath);
                        t = str3.Length;
                        for (int k = 0; k < str3.Length; k++)
                        {
                            if (str3[k].Contains("manufacturer="))
                            {
                                flag = true;
                                break;
                            }
                        }
                    }
                    else if (File.Exists(path + "default.prop"))
                    {
                        FilePath = path + "default.prop";
                        string[] str4 = File.ReadAllLines(FilePath);
                        t = str4.Length;
                        for (int l = 0; l < str4.Length; l++)
                        {
                            if (str4[l].Contains("manufacturer="))
                            {
                                flag = true;
                                break;
                            }
                        }
                    }

                    if (flag == true)
                    {
                        using (StreamReader streamReader = new StreamReader(FilePath))
                        {
                            string text = null;

                            //While text IsNot Nothing


                            while (CSharpImpl.Assign(ref text, streamReader.ReadLine()) != null)
                            {
                                Console.WriteLine(text);

                                if (text.Contains("ro.product.manufacturer="))
                                {
                                    array2 = text.Substring(text.IndexOf("=") + 1);
                                }

                                if (text.Contains("ro.dolby.manufacturer="))
                                {
                                    array2 = text.Substring(text.IndexOf("=") + 1);
                                }

                                if (text.Contains("ro.product.vendor.manufacturer="))
                                {
                                    array2 = text.Substring(text.IndexOf("=") + 1);
                                }

                                if (text.Contains("ro.product.brand="))
                                {
                                    array5 = text.Substring(text.IndexOf("=") + 1);
                                }

                                if (text.Contains("ro.dolby.brand="))
                                {
                                    array5 = text.Substring(text.IndexOf("=") + 1);
                                }

                                if (text.Contains("ro.product.vendor.brand="))
                                {
                                    array5 = text.Substring(text.IndexOf("=") + 1);
                                }

                                if (text.Contains("ro.product.name="))
                                {
                                    array4 = text.Substring(text.IndexOf("=") + 1);
                                }

                                if (text.Contains("ro.product.vendor.name="))
                                {
                                    array4 = text.Substring(text.IndexOf("=") + 1);
                                }

                                if (text.Contains("ro.dolby.name="))
                                {
                                    array4 = text.Substring(text.IndexOf("=") + 1);
                                }

                                if (text.Contains("ro.product.model="))
                                {
                                    array6 = text.Substring(text.IndexOf("=") + 1);
                                }

                                if (text.Contains("ro.product.vendor.model="))
                                {
                                    array6 = text.Substring(text.IndexOf("=") + 1);
                                }

                                if (text.Contains("ro.dolby.model="))
                                {
                                    array6 = text.Substring(text.IndexOf("=") + 1)
                                        .Replace("effectmodel", "");
                                }

                                if (
                                    text.Contains("ro.build.version.release=")
                                    | text.Contains("ro.vendor.build.version.release=")
                                )
                                {
                                    array8 = AndroidCommands.AndroidName(
                                        text.Replace("ro.build.version.release=", "")
                                            .Replace("ro.vendor.build.version.release=", "")
                                    );
                                }

                                if (text.Contains("ro.mediatek.version.release="))
                                {
                                    array9 = text.Substring(text.IndexOf("=") + 1);
                                }

                                if (
                                    text.Contains("ro.build.id=")
                                    | text.Contains("ro.vendor.build.id=")
                                )
                                {
                                    array7 = text.Replace("ro.build.id=", "")
                                        .Replace("ro.vendor.build.id=", "");
                                }

                                if (
                                    text.Contains("ro.build.version.security_patch=")
                                    | text.Contains("ro.vendor.build.security_patch=")
                                )
                                {
                                    array11 = text.Replace("ro.build.version.security_patch=", "")
                                        .Replace("ro.vendor.build.security_patch=", "");
                                }

                                if (text.Contains("ro.product.board="))
                                {
                                    array3 = text.Substring(text.IndexOf("=") + 1);
                                }

                                if (text.Contains("ro.build.description="))
                                {
                                    array12 = text.Substring(text.IndexOf("=") + 1)
                                        .Replace("release-keys", "");
                                }

                                if (
                                    text.Contains("ro.bootimage.build.date=")
                                    | text.Contains("ro.build.date=")
                                )
                                {
                                    array10 = text.Substring(text.IndexOf("=") + 1);
                                }

                                if (text.Contains("ro.oppo.market.name=")) { }

                                if (
                                    text.Contains("ro.mediatek.platform=")
                                    | text.Contains("ro.vendor.mediatek.platform=")
                                )
                                {
                                    array = text.Replace("ro.mediatek.platform=", "")
                                        .Replace("release-keys", "")
                                        .Replace("ro.vendor.mediatek.platform=", "");
                                    string text9 = array.ToLower();
                                    array = text9
                                        .Replace("qcom", "Qualcomm SnapDragon( QLM ) ")
                                        .Replace("mt", "MT")
                                        .Replace("sc", "SpreadTrum( SPD ) SP")
                                        .Replace("sp", "SpreadTrum( SPD ) SP")
                                        .Replace("samsungexynos", "Samsung Exynos ")
                                        .Replace("hi", "( HiSilicon Kirin ) ")
                                        .Replace("m7cdug", "Qualcomm SnapDragon( QLM )");
                                }

                                if (text.Contains("ro.product.cpu.abi="))
                                {
                                    array1 = text.Substring(text.IndexOf("=") + 1);
                                }
                                n += 1;
                                Main.ProcessBar(n, t);
                                Thread.Sleep(5);
                                text = streamReader.ReadLine();
                            }

                            if (!string.IsNullOrEmpty(array))
                            {
                                Richlog("platform             : ", Color.Black, false, false);
                                Richlog(array.ToUpper(), Color.Purple, false, true);
                            }

                            if (!string.IsNullOrEmpty(array1))
                            {
                                Richlog("cpu abi              : ", Color.Black, false, false);
                                Richlog(array1, Color.Purple, false, true);
                            }

                            if (!string.IsNullOrEmpty(array2))
                            {
                                Richlog("manufacturer         : ", Color.Black, false, false);
                                Richlog(array2, Color.Purple, false, true);
                            }

                            if (!string.IsNullOrEmpty(array3))
                            {
                                Richlog("board                : ", Color.Black, false, false);
                                Richlog(array3, Color.Purple, false, true);
                            }

                            if (!string.IsNullOrEmpty(array4))
                            {
                                Richlog("name                 : ", Color.Black, false, false);
                                Richlog(array4, Color.Purple, false, true);
                            }

                            if (!string.IsNullOrEmpty(array5))
                            {
                                Richlog("brand                : ", Color.Black, false, false);
                                Richlog(array5, Color.Purple, false, true);
                            }

                            if (!string.IsNullOrEmpty(array6))
                            {
                                Richlog("model                : ", Color.Black, false, false);
                                Richlog(array6, Color.Purple, false, true);
                            }

                            if (!string.IsNullOrEmpty(array7))
                            {
                                Richlog("build id             : ", Color.Black, false, false);
                                Richlog(array7, Color.Purple, false, true);
                            }

                            if (!string.IsNullOrEmpty(array8))
                            {
                                Richlog("version              : ", Color.Black, false, false);
                                Richlog(array8, Color.Purple, false, true);
                            }

                            if (!string.IsNullOrEmpty(array9))
                            {
                                Richlog("build number         : ", Color.Black, false, false);
                                Richlog(array9, Color.Purple, false, true);
                            }

                            if (!string.IsNullOrEmpty(array10))
                            {
                                Richlog("build date           : ", Color.Black, false, false);
                                Richlog(array10, Color.Purple, false, true);
                            }

                            if (!string.IsNullOrEmpty(array11))
                            {
                                Richlog("security patch       : ", Color.Black, false, false);
                                Richlog(array11, Color.Purple, false, true);
                            }

                            if (!string.IsNullOrEmpty(array12))
                            {
                                Richlog("description          : ", Color.Black, false, false);
                                Richlog(array12, Color.Purple, false, true);
                            }
                        }
                    }
                };
                process.WaitForExit();
            }

            DirectoryInfo directory = new DirectoryInfo(
                Path.GetDirectoryName(sourcefile.AndroidPath)
            );

            foreach (FileInfo file in directory.EnumerateFiles())
            {
                file.Delete();
            }

            foreach (DirectoryInfo subDirectory in directory.EnumerateDirectories())
            {
                subDirectory.Delete(true);
            }

            directory.Delete(true);

            return Task.CompletedTask;
        }

        private class CSharpImpl
        {
            [Obsolete("Please refactor calling code to use normal Visual Basic assignment")]
            public static T Assign<T>(ref T target, T value)
            {
                target = value;
                return value;
            }
        }
    }

    public class AndroidCommands
    {
        public static string AndroidName(string os)
        {
            os = os.Trim();
            string result = "";

            if (os.Contains("1.5"))
            {
                result = "Android (" + os + ") Cupcake";
            }

            if (os.Contains("1.6"))
            {
                result = "Android (" + os + ") Donut";
            }

            if (os.Contains("2"))
            {
                result = "Android (" + os + ") Eclair";
            }

            if (os.Contains("2.2") || os.Contains("2.2.3"))
            {
                result = "Android (" + os + ") Froyo";
            }

            if (os.Contains("2.3"))
            {
                result = "Android (" + os + ") Gingerbread";
            }

            if (os.Contains("3.0") || os.Contains("3.1") || os.Contains("3.2"))
            {
                result = "Android (" + os + ") Honeycomb";
            }

            if (os.Contains("4.0"))
            {
                result = "Android (" + os + ") ICE Cream Sandwich";
            }

            if (os.Contains("4.1") || os.Contains("4.2") || os.Contains("4.3"))
            {
                result = "Android (" + os + ") Jelly Bean";
            }

            if (os.Contains("4.4"))
            {
                result = "Android (" + os + ") KitKat";
            }

            if (os.Contains("5.0") || os.Contains("5.1"))
            {
                result = "Android (" + os + ") Lollipop";
            }

            if (os.Contains("6.0"))
            {
                result = "Android (" + os + ") Marshmallow";
            }

            if (os.Contains("7.0") || os.Contains("7.1"))
            {
                result = "Android (" + os + ") Nougat";
            }

            if (os.Contains("8.0") || os.Contains("8.1"))
            {
                result = "Android (" + os + ") Oreo";
            }

            if (os.Contains("9"))
            {
                result = "Android (" + os + ") Pie";
            }

            if (os.Contains("10"))
            {
                result = "Android (" + os + ")";
            }

            if (os.Contains("11"))
            {
                result = "Android (" + os + ")";
            }

            if (os.Contains("12"))
            {
                result = "Android (" + os + ")";
            }

            return result;
        }
    }

    public class sourcefile
    {
        public static string Directorypath = "tmp\\unpack";
        public static string AndroidPath = Directorypath + "\\C4.exe";
        public static string Dumped = Directorypath + "\\" + "boot.img";
    }
}
