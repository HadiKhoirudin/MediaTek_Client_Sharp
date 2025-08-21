﻿using System;
using System.Security.Cryptography;

namespace Force.Crc32
{
    internal class Crc32Algorithm : HashAlgorithm
    {
        private uint _currentCrc;

        private readonly bool _isBigEndian = true;

        private static readonly SafeProxy _proxy;

        public Crc32Algorithm()
        {
            HashSizeValue = 32;
        }

        public Crc32Algorithm(bool isBigEndian = true)
            : this()
        {
            _isBigEndian = isBigEndian;
        }

        public static uint Append(uint initial, byte[] input, int offset, int length)
        {
            if (input != null)
            {
                if (offset < 0 || length < 0 || offset + length > input.Length)
                {
                    throw new ArgumentOutOfRangeException(
                        "Selected range is outside the bounds of the input array"
                    );
                }
                return AppendInternal(initial, input, offset, length);
            }
            throw new ArgumentNullException();
        }

        public static uint Append(uint initial, byte[] input)
        {
            if (input == null)
            {
                throw new ArgumentNullException();
            }
            return AppendInternal(initial, input, 0, input.Length);
        }

        public static uint Compute(byte[] input, int offset, int length)
        {
            return Append(0U, input, offset, length);
        }

        public static uint Compute(byte[] input)
        {
            return Append(0U, input);
        }

        public static uint ComputeAndWriteToEnd(byte[] input, int offset, int length)
        {
            if (length + 4 > input.Length)
            {
                throw new ArgumentOutOfRangeException(
                    "length",
                    "Length of data should be less than array length - 4 bytes of CRC data"
                );
            }
            uint num = Append(0U, input, offset, length);
            int num2 = offset + length;
            input[num2] = (byte)num;
            input[num2 + 1] = (byte)(num >> 8);
            input[num2 + 2] = (byte)(num >> 16);
            input[num2 + 3] = (byte)(num >> 24);
            return num;
        }

        public static uint ComputeAndWriteToEnd(byte[] input)
        {
            if (input.Length < 4)
            {
                throw new ArgumentOutOfRangeException(
                    "input",
                    "input array should be 4 bytes at least"
                );
            }
            return ComputeAndWriteToEnd(input, 0, input.Length - 4);
        }

        public static bool IsValidWithCrcAtEnd(byte[] input, int offset, int lengthWithCrc)
        {
            return Append(0U, input, offset, lengthWithCrc) == 558161692;
        }

        public static bool IsValidWithCrcAtEnd(byte[] input)
        {
            if (input.Length < 4)
            {
                throw new ArgumentOutOfRangeException(
                    "input",
                    "input array should be 4 bytes at least"
                );
            }
            return Append(0U, input, 0, input.Length) == 558161692;
        }

        public override void Initialize()
        {
            _currentCrc = 0U;
        }

        protected override void HashCore(byte[] input, int offset, int length)
        {
            _currentCrc = AppendInternal(_currentCrc, input, offset, length);
        }

        protected override byte[] HashFinal()
        {
            if (!_isBigEndian)
            {
                return (byte[])
                    (object)(
                        new byte[4]
                        {
                            (byte)_currentCrc,
                            (byte)(_currentCrc >> 8),
                            (byte)(_currentCrc >> 16),
                            (byte)(_currentCrc >> 24)
                        }
                    );
            }
            return (byte[])
                (object)(
                    new byte[4]
                    {
                        (byte)(_currentCrc >> 24),
                        (byte)(_currentCrc >> 16),
                        (byte)(_currentCrc >> 8),
                        (byte)_currentCrc
                    }
                );
        }

        private static uint AppendInternal(uint initial, byte[] input, int offset, int length)
        {
            if (length > 0)
            {
                return _proxy.Append(initial, input, offset, length);
            }
            return initial;
        }

        static Crc32Algorithm()
        {
            _proxy = new SafeProxy();
        }
    }
}
