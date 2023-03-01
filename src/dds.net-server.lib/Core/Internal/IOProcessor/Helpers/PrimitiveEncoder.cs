using System.Text;

namespace DDS.Net.Server.Core.Internal.IOProcessor.Helpers
{
    internal static class PrimitiveEncoder
    {
        //- 
        //- String
        //- 
        public static string ReadString(this byte[] data, ref int offset)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (offset < 0 || offset + 1 >= data.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }

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
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (offset < 0 || offset >= data.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }

            return data[offset++] != 0;
        }
        public static void WriteBoolean(this byte[] data, ref int offset, bool value)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (offset < 0 || offset >= data.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }

            data[offset++] = (byte)(value? 1 : 0);
        }

        //- 
        //- Byte (1-Byte Signed Integer)
        //- 
        public static sbyte ReadByte(this byte[] data, ref int offset)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (offset < 0 || offset >= data.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }

            return (sbyte)data[offset++];
        }
        public static void WriteByte(this byte[] data, ref int offset, sbyte value)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (offset < 0 || offset >= data.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }

            data[offset++] = (byte)value;
        }

        //- 
        //- Word (2-Byte Signed Integer)
        //- 
        public static short ReadWord(this byte[] data, ref int offset)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (offset < 0 || offset + 1 >= data.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }

            int value = data[offset++];
            value = (value << 8) | data[offset++];

            return (short)value;
        }
        public static void WriteWord(this byte[] data, ref int offset, short value)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (offset < 0 || offset + 1 >= data.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }


            data[offset++] = (byte)((value >> 8) & 0x0ff);
            data[offset++] = (byte)(value & 0x0ff);
        }

        //- 
        //- DWord (4-Byte Signed Integer)
        //- 
        public static int ReadDWord(this byte[] data, ref int offset)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (offset < 0 || offset + 3 >= data.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }

            int value = data[offset++];
            value = (value << 8) | data[offset++];
            value = (value << 8) | data[offset++];
            value = (value << 8) | data[offset++];

            return value;
        }
        public static void WriteDWord(this byte[] data, ref int offset, int value)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (offset < 0 || offset + 3 >= data.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }

            data[offset++] = (byte)((value >> 24) & 0x0ff);
            data[offset++] = (byte)((value >> 16) & 0x0ff);
            data[offset++] = (byte)((value >>  8) & 0x0ff);
            data[offset++] = (byte)((value >>  0) & 0x0ff);
        }

        //- 
        //- QWord (8-Byte Signed Integer)
        //- 
        public static long ReadQWord(this byte[] data, ref int offset)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (offset < 0 || offset + 7 >= data.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }

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
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (offset < 0 || offset + 7 >= data.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }

            data[offset++] = (byte)((value >> 56) & 0x0ff);
            data[offset++] = (byte)((value >> 48) & 0x0ff);
            data[offset++] = (byte)((value >> 40) & 0x0ff);
            data[offset++] = (byte)((value >> 32) & 0x0ff);
            data[offset++] = (byte)((value >> 24) & 0x0ff);
            data[offset++] = (byte)((value >> 16) & 0x0ff);
            data[offset++] = (byte)((value >>  8) & 0x0ff);
            data[offset++] = (byte)((value >>  0) & 0x0ff);
        }

        //- 
        //- Unsigned Byte (1-Byte Unsigned Integer)
        //- 
        public static byte ReadUnsignedByte(this byte[] data, ref int offset)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (offset < 0 || offset >= data.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }

            return data[offset++];
        }
        public static void WriteUnsignedByte(this byte[] data, ref int offset, byte value)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (offset < 0 || offset >= data.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }

            data[offset++] = value;
        }

        //- 
        //- Unsigned Word (2-Byte Unsigned Integer)
        //- 
        public static ushort ReadUnsignedWord(this byte[] data, ref int offset)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (offset < 0 || offset + 1 >= data.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }

            int value = data[offset++];
            value = (value << 8) | data[offset++];

            return (ushort)value;
        }
        public static void WriteUnsignedWord(this byte[] data, ref int offset, ushort value)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (offset < 0 || offset + 1 >= data.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }

            data[offset++] = (byte)((value >> 8) & 0x0ff);
            data[offset++] = (byte)((value >> 0) & 0x0ff);
        }

        //- 
        //- Unsigned DWord (4-Byte Unsigned Integer)
        //- 
        public static uint ReadUnsignedDWord(this byte[] data, ref int offset)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (offset < 0 || offset + 3 >= data.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }

            uint value = data[offset++];
            value = (value << 8) | data[offset++];
            value = (value << 8) | data[offset++];
            value = (value << 8) | data[offset++];

            return value;
        }
        public static void WriteUnsignedDWord(this byte[] data, ref int offset, uint value)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (offset < 0 || offset + 3 >= data.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }

            data[offset++] = (byte)((value >> 24) & 0x0ff);
            data[offset++] = (byte)((value >> 16) & 0x0ff);
            data[offset++] = (byte)((value >>  8) & 0x0ff);
            data[offset++] = (byte)((value >>  0) & 0x0ff);
        }

        //- 
        //- Unsigned QWord (8-Byte Unsigned Integer)
        //- 
        public static ulong ReadUnsignedQWord(this byte[] data, ref int offset)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (offset < 0 || offset + 7 >= data.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }

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
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (offset < 0 || offset + 7 >= data.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }

            data[offset++] = (byte)((value >> 56) & 0x0ff);
            data[offset++] = (byte)((value >> 48) & 0x0ff);
            data[offset++] = (byte)((value >> 40) & 0x0ff);
            data[offset++] = (byte)((value >> 32) & 0x0ff);
            data[offset++] = (byte)((value >> 24) & 0x0ff);
            data[offset++] = (byte)((value >> 16) & 0x0ff);
            data[offset++] = (byte)((value >>  8) & 0x0ff);
            data[offset++] = (byte)((value >>  0) & 0x0ff);
        }

        //- 
        //- Single (4-Byte Floating-point)
        //- 
        public static float ReadSingle(this byte[] data, ref int offset)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (offset < 0 || offset + 3 >= data.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }

            float value = BitConverter.ToSingle(data, offset);
            offset += 4;

            return value;
        }
        public static void WriteSingle(this byte[] data, ref int offset, float value)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (offset < 0 || offset + 3 >= data.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }

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
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (offset < 0 || offset + 7 >= data.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }

            double value = BitConverter.ToDouble(data, offset);
            offset += 8;

            return value;
        }
        public static void WriteDouble(this byte[] data, ref int offset, double value)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (offset < 0 || offset + 7 >= data.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }

            byte[] converted = BitConverter.GetBytes(value);
            for (int i = 0; i < 8; i++)
            {
                data[offset++] = converted[i];
            }
        }
    }
}
