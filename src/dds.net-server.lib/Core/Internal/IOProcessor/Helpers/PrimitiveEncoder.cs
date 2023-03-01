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
    }
}
