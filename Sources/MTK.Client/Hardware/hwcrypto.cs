using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using static mtkclient.MTK.Client.utils;

namespace mtkclient.MTK.Client
{
    internal class hwcrypto
    {
        public static string getSHA1Hash(byte[] bytetohash)
        {
            SHA1CryptoServiceProvider sha1Obj = new SHA1CryptoServiceProvider();
            bytetohash = sha1Obj.ComputeHash(bytetohash);
            string strResult = string.Empty;
            foreach (byte b in bytetohash)
            {
                strResult += b.ToString("x2");
            }
            return strResult;
        }

        public static string gethash256(byte[] bytetohas)
        {
            var hash = new SHA256Managed().ComputeHash(bytetohas);
            return BytesToHextring(hash);
        }

        public static string getsha256(string content)
        {
            UTF8Encoding myEncoder = new UTF8Encoding();
            byte[] Key = HexStringToBytes("62DDE5B241D5EB467B577C04737FBCD4");
            byte[] Text = myEncoder.GetBytes(content);
            HMACSHA256 myHMACSHA1 = new HMACSHA256(Key);
            byte[] HashCode = myHMACSHA1.ComputeHash(
                HexStringToBytes("44ED75BBF9E6D0701022E954F0549D552D7C67AF9F8926F90B2638B4EBAE31D4")
            );
            string hash = BytesToHextring(HashCode);
            byte[] l = HexStringToBytes(hash);
            string ko = Convert.ToBase64String(l);
            return hash;
        }

        public static string hashlibsha256(string data)
        {
            var hash = new SHA256Managed().ComputeHash(HexStringToBytes(data));
            return BytesToHextring(hash);
        }

        public static byte[] DecryptorSec(byte[] cipherText)
        {
            var key = Encoding.UTF8.GetBytes("1A52A367CB12C458965D32CD874B36B2");
            var iv = HexStringToBytes("57325A5A125497661254976657325A5A");
            try
            {
                RijndaelManaged Algo = (RijndaelManaged)Rijndael.Create();
                Algo.BlockSize = 128;
                Algo.FeedbackSize = 128;
                Algo.KeySize = 128;
                Algo.Mode = CipherMode.CBC;
                Algo.IV = iv;
                Algo.Key = key;
                Algo.Padding = PaddingMode.None;
                long lenFile = 0;
                byte[] buffout = new byte[cipherText.Length - 1 + 1];
                var s = new MemoryStream(buffout);
                using (ICryptoTransform Decryptor = Algo.CreateDecryptor())
                {
                    using (MemoryStream StreamInput = new MemoryStream(cipherText))
                    {
                        using (
                            CryptoStream crypto_stream = new CryptoStream(
                                s,
                                Decryptor,
                                CryptoStreamMode.Write
                            )
                        )
                        {
                            const int block_size = 128;
                            byte[] buffer = new byte[block_size + 1];
                            int bytes_read = 0;
                            do
                            {
                                bytes_read = StreamInput.Read(buffer, 0, block_size);
                                crypto_stream.Write(buffer, 0, bytes_read);
                                if (bytes_read == 0)
                                {
                                    break;
                                }
                            } while (true);
                        }
                    }
                }
                s.Close();
                return buffout;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return new byte[0];
        }

        public static byte[] computeHash(string clearText, string key)
        {
            UTF8Encoding encoder = new UTF8Encoding();
            HMACSHA512 sha512hasher = new HMACSHA512(encoder.GetBytes(key));
            return sha512hasher.ComputeHash(HexStringToBytes(clearText));
        }

        public static string EncSeccfg(string data)
        {
            byte[] hash = new SHA256Managed().ComputeHash(HexStringToBytes(data));
            return BytesToHextring(hash);
        }

        public bool tryDecryptBytes(ref byte[] B, string Pass)
        {
            try
            {
                RijndaelManaged Rij = new RijndaelManaged
                {
                    Mode = CipherMode.ECB,
                    Key = Encoding.UTF8.GetBytes(Pass)
                };
                ICryptoTransform Decryptor = Rij.CreateDecryptor();
                B = Decryptor.TransformFinalBlock(B, 0, B.Length);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
