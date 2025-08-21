using System;

namespace Force.Crc32
{
    internal class SafeProxy
    {
        private readonly uint[] _table = (uint[])(object)(new uint[4096]);

        internal SafeProxy()
        {
            Init(3988292384U);
        }

        protected void Init(uint poly)
        {
            uint[] table = _table;
            for (uint num = 0U; num <= 255; num++)
            {
                uint num2 = num;
                for (int i = 0; i <= 15; i++)
                {
                    for (int j = 0; j <= 7; j++)
                    {
                        num2 = (((num2 & 1) == 1) ? (poly ^ (num2 >> 1)) : (num2 >> 1));
                    }
                    table[(int)(i * 256 + num)] = num2;
                }
            }
        }

        public uint Append(uint crc, byte[] input, int offset, int length)
        {
            uint num = (uint)((0xFFFFFFFFU) ^ crc);
            uint[] table = _table;
            while (length >= 16)
            {
                uint num2 =
                    table[768 + input[offset + 12]]
                    ^ (
                        table[512 + input[offset + 13]]
                        ^ (table[256 + input[offset + 14]] ^ table[input[offset + 15]])
                    );
                uint num3 =
                    table[1792 + input[offset + 8]]
                    ^ (
                        table[1536 + input[offset + 9]]
                        ^ (table[1280 + input[offset + 10]] ^ table[1024 + input[offset + 11]])
                    );
                uint num4 =
                    table[2816 + input[offset + 4]]
                    ^ (
                        table[2560 + input[offset + 5]]
                        ^ (table[2304 + input[offset + 6]] ^ table[2048 + input[offset + 7]])
                    );
                uint num5 =
                    table[Convert.ToInt32(3840 + ((num ^ input[offset]) & 0xFF))]
                    ^ (
                        table[Convert.ToInt32(3584 + (((num >> 8) ^ input[offset + 1]) & 0xFF))]
                        ^ (
                            table[
                                Convert.ToInt32(3328 + (((num >> 16) ^ input[offset + 2]) & 0xFF))
                            ]
                            ^ table[
                                Convert.ToInt32(3072 + (((num >> 24) ^ input[offset + 3]) & 0xFF))
                            ]
                        )
                    );
                num = num5 ^ (num4 ^ (num3 ^ num2));
                offset += 16;
                length -= 16;
            }
            length -= 1;
            while (length >= 0)
            {
                num = table[Convert.ToInt32((num ^ input[offset]) & 0xFF)] ^ (num >> 8);
                offset += 1;
                length -= 1;
            }
            return (uint)(num ^ (0xFFFFFFFFU));
        }
    }
}
