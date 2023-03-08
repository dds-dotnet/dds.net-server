using DDS.Net.Server.Core.Internal.IOProcessor.Helpers;
using System.Text;

namespace DDS.Net.Server.Core.Internal.IOProcessor.EncodersAndDecoders
{
    internal static class EncDecPrimitives
    {
        //- 
        //- String
        //- 

        /// <summary>
        /// Reads an encoded string from given data buffer
        /// </summary>
        /// <param name="data">The buffer containing data</param>
        /// <param name="offset">Offset in the data buffer - updated afterwards to point
        /// to the next element in the buffer</param>
        /// <returns>string read from data buffer</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="Exception"></exception>
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
        /// <summary>
        /// Writes given string to the given buffer encoded with size (total bytes)
        /// </summary>
        /// <param name="data">The buffer containing data</param>
        /// <param name="offset">Offset in the data buffer - updated afterwards to point
        /// to the next element in the buffer</param>
        /// <param name="value">String to be written to the given buffer</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="Exception"></exception>
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

        /// <summary>
        /// Reads Boolean [True / False] from the given data buffer
        /// </summary>
        /// <param name="data">The data buffer</param>
        /// <param name="offset">Offset in the data buffer - updated afterwards to point
        /// to the next element in the buffer</param>
        /// <returns>Boolean value [True / False]</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static bool ReadBoolean(this byte[] data, ref int offset)
        {
            data.ThrowIfNotHavingRequiredBytes(ref offset, 1);

            return data[offset++] != 0;
        }
        /// <summary>
        /// Writes Boolean [True / False] into the given data buffer
        /// </summary>
        /// <param name="data">The data buffer</param>
        /// <param name="offset">Offset in the data buffer - updated afterwards to point
        /// to the next element in the buffer</param>
        /// <param name="value">The Boolean value</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static void WriteBoolean(this byte[] data, ref int offset, bool value)
        {
            data.ThrowIfNotHavingRequiredBytes(ref offset, 1);

            data[offset++] = (byte)(value ? 1 : 0);
        }

        //- 
        //- Byte (1-Byte Signed Integer)
        //- 

        /// <summary>
        /// Reads Signed 1-byte Integer from the given data buffer
        /// </summary>
        /// <param name="data">The data buffer</param>
        /// <param name="offset">Offset in the data buffer - updated afterwards to point
        /// to the next element in the buffer</param>
        /// <returns>Byte (1-byte Signed Integer)</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static sbyte ReadByte(this byte[] data, ref int offset)
        {
            data.ThrowIfNotHavingRequiredBytes(ref offset, 1);

            return (sbyte)data[offset++];
        }
        /// <summary>
        /// Writes Signed 1-byte Integer into the given data buffer
        /// </summary>
        /// <param name="data">The data buffer</param>
        /// <param name="offset">Offset in the data buffer - updated afterwards to point
        /// to the next element in the buffer</param>
        /// <param name="value">1-byte Signed Integer to be written to the data buffer</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static void WriteByte(this byte[] data, ref int offset, sbyte value)
        {
            data.ThrowIfNotHavingRequiredBytes(ref offset, 1);

            data[offset++] = (byte)value;
        }

        //- 
        //- Word (2-Byte Signed Integer)
        //- 

        /// <summary>
        /// Reads Signed 2-byte Integer from the given data buffer
        /// </summary>
        /// <param name="data">The data buffer</param>
        /// <param name="offset">Offset in the data buffer - updated afterwards to point
        /// to the next element in the buffer</param>
        /// <returns>Word (2-byte Signed Integer)</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static short ReadWord(this byte[] data, ref int offset)
        {
            data.ThrowIfNotHavingRequiredBytes(ref offset, 2);

            int value = data[offset++];
            value = (value << 8) | data[offset++];

            return (short)value;
        }
        /// <summary>
        /// Writes Signed 2-byte Integer into the given data buffer
        /// </summary>
        /// <param name="data">The data buffer</param>
        /// <param name="offset">Offset in the data buffer - updated afterwards to point
        /// to the next element in the buffer</param>
        /// <param name="value">2-byte Signed Integer to be written to the data buffer</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static void WriteWord(this byte[] data, ref int offset, short value)
        {
            data.ThrowIfNotHavingRequiredBytes(ref offset, 2);

            data[offset++] = (byte)((value >> 8) & 0x0ff);
            data[offset++] = (byte)(value & 0x0ff);
        }

        //- 
        //- DWord (4-Byte Signed Integer)
        //- 

        /// <summary>
        /// Reads Signed 4-byte Integer from the given data buffer
        /// </summary>
        /// <param name="data">The data buffer</param>
        /// <param name="offset">Offset in the data buffer - updated afterwards to point
        /// to the next element in the buffer</param>
        /// <returns>DWord (4-byte Signed Integer)</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static int ReadDWord(this byte[] data, ref int offset)
        {
            data.ThrowIfNotHavingRequiredBytes(ref offset, 4);

            int value = data[offset++];
            value = (value << 8) | data[offset++];
            value = (value << 8) | data[offset++];
            value = (value << 8) | data[offset++];

            return value;
        }
        /// <summary>
        /// Writes Signed 4-byte Integer into the given data buffer
        /// </summary>
        /// <param name="data">The data buffer</param>
        /// <param name="offset">Offset in the data buffer - updated afterwards to point
        /// to the next element in the buffer</param>
        /// <param name="value">4-byte Signed Integer to be written to the data buffer</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
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

        /// <summary>
        /// Reads Signed 8-byte Integer from the given data buffer
        /// </summary>
        /// <param name="data">The data buffer</param>
        /// <param name="offset">Offset in the data buffer - updated afterwards to point
        /// to the next element in the buffer</param>
        /// <returns>QWord (8-byte Signed Integer)</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
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
        /// <summary>
        /// Writes Signed 8-byte Integer into the given data buffer
        /// </summary>
        /// <param name="data">The data buffer</param>
        /// <param name="offset">Offset in the data buffer - updated afterwards to point
        /// to the next element in the buffer</param>
        /// <param name="value">8-byte Signed Integer to be written to the data buffer</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
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

        /// <summary>
        /// Reads Unsigned 1-byte Integer from the given data buffer
        /// </summary>
        /// <param name="data">The data buffer</param>
        /// <param name="offset">Offset in the data buffer - updated afterwards to point
        /// to the next element in the buffer</param>
        /// <returns>Unsigned Byte (1-byte Unsigned Integer)</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static byte ReadUnsignedByte(this byte[] data, ref int offset)
        {
            data.ThrowIfNotHavingRequiredBytes(ref offset, 1);

            return data[offset++];
        }
        /// <summary>
        /// Writes Unsigned 1-byte Integer into the given data buffer
        /// </summary>
        /// <param name="data">The data buffer</param>
        /// <param name="offset">Offset in the data buffer - updated afterwards to point
        /// to the next element in the buffer</param>
        /// <param name="value">1-byte Unsigned Integer to be written to the data buffer</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static void WriteUnsignedByte(this byte[] data, ref int offset, byte value)
        {
            data.ThrowIfNotHavingRequiredBytes(ref offset, 1);

            data[offset++] = value;
        }

        //- 
        //- Unsigned Word (2-Byte Unsigned Integer)
        //- 

        /// <summary>
        /// Reads Unsigned 2-byte Integer from the given data buffer
        /// </summary>
        /// <param name="data">The data buffer</param>
        /// <param name="offset">Offset in the data buffer - updated afterwards to point
        /// to the next element in the buffer</param>
        /// <returns>Unsigned Word (2-byte Unsigned Integer)</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static ushort ReadUnsignedWord(this byte[] data, ref int offset)
        {
            data.ThrowIfNotHavingRequiredBytes(ref offset, 2);

            int value = data[offset++];
            value = (value << 8) | data[offset++];

            return (ushort)value;
        }
        /// <summary>
        /// Writes Unsigned 2-byte Integer into the given data buffer
        /// </summary>
        /// <param name="data">The data buffer</param>
        /// <param name="offset">Offset in the data buffer - updated afterwards to point
        /// to the next element in the buffer</param>
        /// <param name="value">2-byte Unsigned Integer to be written to the data buffer</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static void WriteUnsignedWord(this byte[] data, ref int offset, ushort value)
        {
            data.ThrowIfNotHavingRequiredBytes(ref offset, 2);

            data[offset++] = (byte)((value >> 8) & 0x0ff);
            data[offset++] = (byte)((value >> 0) & 0x0ff);
        }

        //- 
        //- Unsigned DWord (4-Byte Unsigned Integer)
        //- 

        /// <summary>
        /// Reads Unsigned 4-byte Integer from the given data buffer
        /// </summary>
        /// <param name="data">The data buffer</param>
        /// <param name="offset">Offset in the data buffer - updated afterwards to point
        /// to the next element in the buffer</param>
        /// <returns>Unsigned DWord (4-byte Unsigned Integer)</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static uint ReadUnsignedDWord(this byte[] data, ref int offset)
        {
            data.ThrowIfNotHavingRequiredBytes(ref offset, 4);

            uint value = data[offset++];
            value = (value << 8) | data[offset++];
            value = (value << 8) | data[offset++];
            value = (value << 8) | data[offset++];

            return value;
        }
        /// <summary>
        /// Writes Unsigned 4-byte Integer into the given data buffer
        /// </summary>
        /// <param name="data">The data buffer</param>
        /// <param name="offset">Offset in the data buffer - updated afterwards to point
        /// to the next element in the buffer</param>
        /// <param name="value">4-byte Unsigned Integer to be written to the data buffer</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
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

        /// <summary>
        /// Reads Unsigned 8-byte Integer from the given data buffer
        /// </summary>
        /// <param name="data">The data buffer</param>
        /// <param name="offset">Offset in the data buffer - updated afterwards to point
        /// to the next element in the buffer</param>
        /// <returns>Unsigned QWord (8-byte Unsigned Integer)</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
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
        /// <summary>
        /// Writes Unsigned 8-byte Integer into the given data buffer
        /// </summary>
        /// <param name="data">The data buffer</param>
        /// <param name="offset">Offset in the data buffer - updated afterwards to point
        /// to the next element in the buffer</param>
        /// <param name="value">8-byte Unsigned Integer to be written to the data buffer</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
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

        /// <summary>
        /// Reads Single-precision (float - 4-byte) floating-point number from the given data buffer
        /// </summary>
        /// <param name="data">The data buffer</param>
        /// <param name="offset">Offset in the data buffer - updated afterwards to point
        /// to the next element in the buffer</param>
        /// <returns>float value</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
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

                float value = BitConverter.ToSingle(bytes.SwapBytes(), 0);
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
        /// <summary>
        /// Writes Single-precision (float - 4-byte) floating-point number into the given data buffer
        /// </summary>
        /// <param name="data">The data buffer</param>
        /// <param name="offset">Offset in the data buffer - updated afterwards to point
        /// to the next element in the buffer</param>
        /// <param name="value">The float value to be written to the data buffer</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static void WriteSingle(this byte[] data, ref int offset, float value)
        {
            data.ThrowIfNotHavingRequiredBytes(ref offset, 4);

            byte[] converted = BitConverter.GetBytes(value);

            if (BitConverter.IsLittleEndian)
            {
                for (int i = 3; i >= 0; i--)
                {
                    data[offset++] = converted[i];
                }
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    data[offset++] = converted[i];
                }
            }
        }

        //- 
        //- Double (8-Byte Floating-point)
        //- 

        /// <summary>
        /// Reads Double-precision (double - 8-byte) floating-point number from the given data buffer
        /// </summary>
        /// <param name="data">The data buffer</param>
        /// <param name="offset">Offset in the data buffer - updated afterwards to point
        /// to the next element in the buffer</param>
        /// <returns>double value</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static double ReadDouble(this byte[] data, ref int offset)
        {
            data.ThrowIfNotHavingRequiredBytes(ref offset, 8);


            if (BitConverter.IsLittleEndian)
            {
                byte[] bytes = new byte[8];
                bytes[0] = data[offset + 0];
                bytes[1] = data[offset + 1];
                bytes[2] = data[offset + 2];
                bytes[3] = data[offset + 3];
                bytes[4] = data[offset + 4];
                bytes[5] = data[offset + 5];
                bytes[6] = data[offset + 6];
                bytes[7] = data[offset + 7];

                double value = BitConverter.ToDouble(bytes.SwapBytes(), 0);
                offset += 8;

                return value;
            }
            else
            {
                double value = BitConverter.ToDouble(data, offset);
                offset += 8;

                return value;
            }
        }
        /// <summary>
        /// Writes Double-precision (double - 8-byte) floating-point number into the given data buffer
        /// </summary>
        /// <param name="data">The data buffer</param>
        /// <param name="offset">Offset in the data buffer - updated afterwards to point
        /// to the next element in the buffer</param>
        /// <param name="value">The double value to be written to the data buffer</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static void WriteDouble(this byte[] data, ref int offset, double value)
        {
            data.ThrowIfNotHavingRequiredBytes(ref offset, 8);

            byte[] converted = BitConverter.GetBytes(value);
            
            if (BitConverter.IsLittleEndian)
            {
                for (int i = 7; i >= 0; i--)
                {
                    data[offset++] = converted[i];
                }
            }
            else
            {
                for (int i = 0; i < 8; i++)
                {
                    data[offset++] = converted[i];
                }
            }
        }
    }
}
