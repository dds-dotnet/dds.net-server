﻿using DDS.Net.Server.Core.Internal.IOProcessor.Helpers;
using System.Text;

namespace DDS.Net.Server.Core.Internal.IOProcessor.EncodersAndDecoders
{
    internal static class EncDecPrimitives
    {
        //- 
        //- String
        //- 
        public static string ReadString(this byte[] data, ref int offset)
        {
            data.ThrowIfNotHavingRequiredBytes(ref offset, 2);

            int length = data[offset++];
            length = (length << 8) | data[offset++];

            if (offset + length > data.Length)
            {
                throw new Exception($"String should be {length} bytes long " +
                                    $"but {offset + length - data.Length} bytes are available");
            }

            string retval = Encoding.Unicode.GetString(data, offset, length);
            offset += length;

            return retval;
        }
        public static void WriteString(this byte[] data, ref int offset, string value)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            int size = 0;
            byte[] bytes = null!;

            if (!string.IsNullOrEmpty(value))
            {
                bytes = Encoding.Unicode.GetBytes(value);
                size = bytes.Length;

                if (offset < 0 || offset + 1 + bytes.Length >= data.Length)
                {
                    throw new ArgumentOutOfRangeException(nameof(offset));
                }
                if (size > 65535)
                {
                    throw new Exception($"String too long - having {size} bytes");
                }
            }
            else
            {
                if (offset < 0 || offset + 1 >= data.Length)
                {
                    throw new ArgumentOutOfRangeException(nameof(offset));
                }
            }

            data[offset + 1] = (byte)(size & 0x0ff);
            data[offset + 0] = (byte)((size >> 8) & 0x0ff);
            offset += 2;

            if (size > 0)
            {
                for (int i = 0; i < size; i++)
                {
                    data[offset++] = bytes[i];
                }
            }
        }

        //- 
        //- Boolean
        //- 
        public static bool ReadBoolean(this byte[] data, ref int offset)
        {
            data.ThrowIfNotHavingRequiredBytes(ref offset, 1);

            return data[offset++] != 0;
        }
        public static void WriteBoolean(this byte[] data, ref int offset, bool value)
        {
            data.ThrowIfNotHavingRequiredBytes(ref offset, 1);

            data[offset++] = (byte)(value ? 1 : 0);
        }

        //- 
        //- Byte (1-Byte Signed Integer)
        //- 
        public static sbyte ReadByte(this byte[] data, ref int offset)
        {
            data.ThrowIfNotHavingRequiredBytes(ref offset, 1);

            return (sbyte)data[offset++];
        }
        public static void WriteByte(this byte[] data, ref int offset, sbyte value)
        {
            data.ThrowIfNotHavingRequiredBytes(ref offset, 1);

            data[offset++] = (byte)value;
        }

        //- 
        //- Word (2-Byte Signed Integer)
        //- 
        public static short ReadWord(this byte[] data, ref int offset)
        {
            data.ThrowIfNotHavingRequiredBytes(ref offset, 2);

            int value = data[offset++];
            value = (value << 8) | data[offset++];

            return (short)value;
        }
        public static void WriteWord(this byte[] data, ref int offset, short value)
        {
            data.ThrowIfNotHavingRequiredBytes(ref offset, 2);

            data[offset++] = (byte)((value >> 8) & 0x0ff);
            data[offset++] = (byte)(value & 0x0ff);
        }

        //- 
        //- DWord (4-Byte Signed Integer)
        //- 
        public static int ReadDWord(this byte[] data, ref int offset)
        {
            data.ThrowIfNotHavingRequiredBytes(ref offset, 4);

            int value = data[offset++];
            value = (value << 8) | data[offset++];
            value = (value << 8) | data[offset++];
            value = (value << 8) | data[offset++];

            return value;
        }
        public static void WriteDWord(this byte[] data, ref int offset, int value)
        {
            data.ThrowIfNotHavingRequiredBytes(ref offset, 4);

            data[offset++] = (byte)((value >> 24) & 0x0ff);
            data[offset++] = (byte)((value >> 16) & 0x0ff);
            data[offset++] = (byte)((value >> 8) & 0x0ff);
            data[offset++] = (byte)((value >> 0) & 0x0ff);
        }

        //- 
        //- QWord (8-Byte Signed Integer)
        //- 
        public static long ReadQWord(this byte[] data, ref int offset)
        {
            data.ThrowIfNotHavingRequiredBytes(ref offset, 8);

            long value = data[offset++];
            value = (value << 8) | data[offset++];
            value = (value << 8) | data[offset++];
            value = (value << 8) | data[offset++];
            value = (value << 8) | data[offset++];
            value = (value << 8) | data[offset++];
            value = (value << 8) | data[offset++];
            value = (value << 8) | data[offset++];

            return value;
        }
        public static void WriteQWord(this byte[] data, ref int offset, long value)
        {
            data.ThrowIfNotHavingRequiredBytes(ref offset, 8);

            data[offset++] = (byte)((value >> 56) & 0x0ff);
            data[offset++] = (byte)((value >> 48) & 0x0ff);
            data[offset++] = (byte)((value >> 40) & 0x0ff);
            data[offset++] = (byte)((value >> 32) & 0x0ff);
            data[offset++] = (byte)((value >> 24) & 0x0ff);
            data[offset++] = (byte)((value >> 16) & 0x0ff);
            data[offset++] = (byte)((value >> 8) & 0x0ff);
            data[offset++] = (byte)((value >> 0) & 0x0ff);
        }

        //- 
        //- Unsigned Byte (1-Byte Unsigned Integer)
        //- 
        public static byte ReadUnsignedByte(this byte[] data, ref int offset)
        {
            data.ThrowIfNotHavingRequiredBytes(ref offset, 1);

            return data[offset++];
        }
        public static void WriteUnsignedByte(this byte[] data, ref int offset, byte value)
        {
            data.ThrowIfNotHavingRequiredBytes(ref offset, 1);

            data[offset++] = value;
        }

        //- 
        //- Unsigned Word (2-Byte Unsigned Integer)
        //- 
        public static ushort ReadUnsignedWord(this byte[] data, ref int offset)
        {
            data.ThrowIfNotHavingRequiredBytes(ref offset, 2);

            int value = data[offset++];
            value = (value << 8) | data[offset++];

            return (ushort)value;
        }
        public static void WriteUnsignedWord(this byte[] data, ref int offset, ushort value)
        {
            data.ThrowIfNotHavingRequiredBytes(ref offset, 2);

            data[offset++] = (byte)((value >> 8) & 0x0ff);
            data[offset++] = (byte)((value >> 0) & 0x0ff);
        }

        //- 
        //- Unsigned DWord (4-Byte Unsigned Integer)
        //- 
        public static uint ReadUnsignedDWord(this byte[] data, ref int offset)
        {
            data.ThrowIfNotHavingRequiredBytes(ref offset, 4);

            uint value = data[offset++];
            value = (value << 8) | data[offset++];
            value = (value << 8) | data[offset++];
            value = (value << 8) | data[offset++];

            return value;
        }
        public static void WriteUnsignedDWord(this byte[] data, ref int offset, uint value)
        {
            data.ThrowIfNotHavingRequiredBytes(ref offset, 4);

            data[offset++] = (byte)((value >> 24) & 0x0ff);
            data[offset++] = (byte)((value >> 16) & 0x0ff);
            data[offset++] = (byte)((value >> 8) & 0x0ff);
            data[offset++] = (byte)((value >> 0) & 0x0ff);
        }

        //- 
        //- Unsigned QWord (8-Byte Unsigned Integer)
        //- 
        public static ulong ReadUnsignedQWord(this byte[] data, ref int offset)
        {
            data.ThrowIfNotHavingRequiredBytes(ref offset, 8);

            ulong value = data[offset++];
            value = (value << 8) | data[offset++];
            value = (value << 8) | data[offset++];
            value = (value << 8) | data[offset++];
            value = (value << 8) | data[offset++];
            value = (value << 8) | data[offset++];
            value = (value << 8) | data[offset++];
            value = (value << 8) | data[offset++];

            return value;
        }

        public static void WriteUnsignedQWord(this byte[] data, ref int offset, ulong value)
        {
            data.ThrowIfNotHavingRequiredBytes(ref offset, 8);

            data[offset++] = (byte)((value >> 56) & 0x0ff);
            data[offset++] = (byte)((value >> 48) & 0x0ff);
            data[offset++] = (byte)((value >> 40) & 0x0ff);
            data[offset++] = (byte)((value >> 32) & 0x0ff);
            data[offset++] = (byte)((value >> 24) & 0x0ff);
            data[offset++] = (byte)((value >> 16) & 0x0ff);
            data[offset++] = (byte)((value >> 8) & 0x0ff);
            data[offset++] = (byte)((value >> 0) & 0x0ff);
        }

        //- 
        //- Single (4-Byte Floating-point)
        //- 
        public static float ReadSingle(this byte[] data, ref int offset)
        {
            data.ThrowIfNotHavingRequiredBytes(ref offset, 4);

            if (BitConverter.IsLittleEndian)
            {
                byte[] bytes = new byte[4];
                bytes[0] = data[offset + 0];
                bytes[1] = data[offset + 1];
                bytes[2] = data[offset + 2];
                bytes[3] = data[offset + 3];

                float value = BitConverter.ToSingle(bytes.ReverseArray(), offset);
                offset += 4;
                return value;
            }
            else
            {
                float value = BitConverter.ToSingle(data, offset);
                offset += 4;
                return value;
            }
        }
        public static void WriteSingle(this byte[] data, ref int offset, float value)
        {
            data.ThrowIfNotHavingRequiredBytes(ref offset, 4);

            byte[] converted = BitConverter.GetBytes(value);
            for (int i = 0; i < 4; i++)
            {
                data[offset++] = converted[i];
            }
        }

        //- 
        //- Double (8-Byte Floating-point)
        //- 
        public static double ReadDouble(this byte[] data, ref int offset)
        {
            data.ThrowIfNotHavingRequiredBytes(ref offset, 8);

            double value = BitConverter.ToDouble(data, offset);
            offset += 8;

            return value;
        }
        public static void WriteDouble(this byte[] data, ref int offset, double value)
        {
            data.ThrowIfNotHavingRequiredBytes(ref offset, 8);

            byte[] converted = BitConverter.GetBytes(value);
            for (int i = 0; i < 8; i++)
            {
                data[offset++] = converted[i];
            }
        }
    }
}