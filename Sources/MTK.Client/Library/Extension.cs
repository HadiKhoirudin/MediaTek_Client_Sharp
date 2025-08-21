using System.Collections.Generic;
using System.IO;

namespace mtkclient.library
{
    internal static class Extension
    {
        public static void Write(this MemoryStream ms, byte[] buff)
        {
            ms.Write(buff, 0, buff.Length);
        }

        public static int Find(this byte[] haystack, byte[] needle, int start = 0)
        {
            int num = needle.Length;
            int num2 = haystack.Length - num;
            for (int i = start; i <= num2; i++)
            {
                int j;
                for (j = 0; j < num && needle[j] == haystack[i + j]; j++) { }
                if (j == num)
                {
                    return i;
                }
            }
            return -1;
        }

        public static byte[][] Split(this byte[] input, byte[] separator)
        {
            List<byte[]> list = new List<byte[]>();
            using (MemoryStream memoryStream = new MemoryStream(input))
            {
                while (memoryStream.Position + 1L < input.Length)
                {
                    int num = input.Find(separator, (int)memoryStream.Position);
                    byte[] array = (
                        (num != -1)
                            ? new byte[(int)(num - memoryStream.Position)]
                            : new byte[(int)(input.Length - memoryStream.Position)]
                    );
                    if (array.Length != 0)
                    {
                        memoryStream.Read(array, 0, array.Length);
                        list.Add(array);
                    }
                    memoryStream.Seek(separator.Length, SeekOrigin.Current);
                }
            }
            return list.ToArray();
        }
    }
}
