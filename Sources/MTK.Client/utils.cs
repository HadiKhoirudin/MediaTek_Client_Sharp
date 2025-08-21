using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static mtkclient.gui;

namespace mtkclient.MTK.Client
{
    internal class utils
    {
        public static async Task<byte[]> readmtk(
            IMtkDevice device,
            CancellationToken cancellationToken,
            string len
        )
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return new byte[0];
            }
            try
            {
                return await device.ReadCustomAsync(cancellationToken, Convert.ToInt32(len));
            }
            catch (Exception ex)
            {
                logs("Error " + ex.ToString(), true);
                return new byte[0];
            }
        }

        public static async Task writemtk(
            IMtkDevice device,
            CancellationToken cancellationToken,
            byte[] data
        )
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }
            try
            {
                await device.WriteAsync(data, 0, data.Length, cancellationToken);
            }
            catch (Exception ex)
            {
                logs("Error " + ex.ToString(), true);
            }
        }

        public static string GetFileSize(long TheSize)
        {
            string str = "0KB";
            try
            {
                long num = TheSize;

                double DoubleBytes = 0;
                if (num >= 1099511627776L)
                {
                    DoubleBytes = TheSize / 1099511627776.0;
                    str = string.Concat(
                        Microsoft.VisualBasic.Strings.FormatNumber(
                            DoubleBytes,
                            2,
                            Microsoft.VisualBasic.TriState.UseDefault,
                            Microsoft.VisualBasic.TriState.UseDefault,
                            Microsoft.VisualBasic.TriState.UseDefault
                        ),
                        " TB"
                    );
                }
                else if (num >= 1073741824L && num <= 1099511627775L)
                {
                    DoubleBytes = TheSize / 1073741824.0;
                    str = string.Concat(
                        Microsoft.VisualBasic.Strings.FormatNumber(
                            DoubleBytes,
                            2,
                            Microsoft.VisualBasic.TriState.UseDefault,
                            Microsoft.VisualBasic.TriState.UseDefault,
                            Microsoft.VisualBasic.TriState.UseDefault
                        ),
                        " GB"
                    );
                }
                else if (num >= 1048576L && num <= 1073741823L)
                {
                    DoubleBytes = TheSize / 1048576.0;
                    str = string.Concat(
                        Microsoft.VisualBasic.Strings.FormatNumber(
                            DoubleBytes,
                            2,
                            Microsoft.VisualBasic.TriState.UseDefault,
                            Microsoft.VisualBasic.TriState.UseDefault,
                            Microsoft.VisualBasic.TriState.UseDefault
                        ),
                        " MB"
                    );
                }
                else if (num >= 1024L && num <= 1048575L)
                {
                    DoubleBytes = TheSize / 1024.0;
                    str = string.Concat(
                        Microsoft.VisualBasic.Strings.FormatNumber(
                            DoubleBytes,
                            2,
                            Microsoft.VisualBasic.TriState.UseDefault,
                            Microsoft.VisualBasic.TriState.UseDefault,
                            Microsoft.VisualBasic.TriState.UseDefault
                        ),
                        " KB"
                    );
                }
                else if (num < 0L || num > 1023L)
                {
                    str = "";
                }
                else
                {
                    DoubleBytes = TheSize;
                    str = string.Concat(
                        Microsoft.VisualBasic.Strings.FormatNumber(
                            DoubleBytes,
                            2,
                            Microsoft.VisualBasic.TriState.UseDefault,
                            Microsoft.VisualBasic.TriState.UseDefault,
                            Microsoft.VisualBasic.TriState.UseDefault
                        ),
                        " bytes"
                    );
                }
            }
            catch { }
            return str;
        }

        public static long HexToLong(string hexstring)
        {
            if (hexstring == "none")
            {
                hexstring = "0";
            }
            else if (string.IsNullOrEmpty(hexstring))
            {
                hexstring = "0";
            }
            return Convert.ToInt64(hexstring, 16);
        }

        public static string LongToHex(long LongLen)
        {
            long val = LongLen;
            string hexString = val.ToString("x");
            return "0x" + hexString;
        }

        public static byte[] HexStringToBytes(string s)
        {
            try
            {
                s = s.Replace(" ", String.Empty).Replace("-", String.Empty).ToUpper();
                int nBytes = s.Length / 2;
                byte[] a = new byte[nBytes];

                for (int i = 0; i < nBytes; i++)
                {
                    a[i] = Convert.ToByte(s.Substring(i * 2, 2), 16);
                }
                return a;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return new byte[0];
        }

        public static string DecimalToHexadecimal(long dec)
        {
            if (dec == 0)
            {
                return "00";
            }
            if (dec < 1)
            {
                return "0";
            }

            long hex = 0;
            string hexStr = string.Empty;

            while (dec > 0)
            {
                hex = dec % 16;

                if (hex < 10)
                {
                    hexStr = hexStr.Insert(0, Convert.ToChar(hex + 48).ToString());
                }
                else
                {
                    hexStr = hexStr.Insert(0, Convert.ToChar(hex + 55).ToString());
                }

                dec = Convert.ToInt64(Math.Floor(dec / 16.0));
            }

            return hexStr;
        }

        public static string shifter(string hexstring)
        {
            string output = "";
            if (!(hexstring.Length % 2 == 0))
            {
                hexstring = "0" + hexstring;
            }
            int countsnya = hexstring.Length - 2;
            int lenoutput = hexstring.Length;

            for (var i = 0; i < hexstring.Length / 2; i++)
            {
                output += hexstring.Substring(countsnya, 2);
                countsnya -= 2;
            }

            output = Regex.Replace(output, "^0+", "");

            if (!(output.Length % 2 == 0))
            {
                output = "0" + output;
            }

            double inputlen = hexstring.Length / 2;

            double kurangnya = inputlen - (output.Length / 2);

            for (var zz = 0; zz < kurangnya; zz++)
            {
                output = "00" + output;
            }

            return (output);
        }
        public static string BytesToHextring(byte[] input)
        {
            return BitConverter.ToString(input).Replace("-", "").ToLower();
        }

        public static string HexToDec(string hexstring)
        {
            if (hexstring == "none")
            {
                hexstring = "0";
            }
            else if (hexstring == "")
            {
                hexstring = "0";
            }
            return Convert.ToInt64(hexstring, 16).ToString();
        }
    }
}
