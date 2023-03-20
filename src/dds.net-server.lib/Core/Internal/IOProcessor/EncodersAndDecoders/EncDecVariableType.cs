using DDS.Net.Server.Entities;
using System.Diagnostics;

namespace DDS.Net.Server.Core.Internal.IOProcessor.EncodersAndDecoders
{
    internal static class EncDecVariableType
    {
        //- 
        //- VariableType
        //- 

        /// <summary>
        /// Reads <c cref="VariableType">VariableType</c> from the data buffer
        /// and updates the offset past the <c cref="VariableType">VariableType</c>.
        /// </summary>
        /// <param name="data">The buffer containing data.</param>
        /// <param name="offset">offset in the data buffer - updated afterwards to point
        /// to the next element in the buffer.</param>
        /// <returns><c cref="VariableType">VariableType</c></returns>
        public static VariableType ReadVariableType(this byte[] data, ref int offset)
        {
            Debug.Assert(data != null);
            Debug.Assert(offset + 2 <= data.Length);

            int v = data[offset++];
            v = (v << 8) | data[offset++];

            if (v >= 0 && v < (int)VariableType.UnknownVariableType)
            {
                return (VariableType)v;
            }

            return VariableType.UnknownVariableType;
        }

        /// <summary>
        /// Writes <c cref="VariableType">VariableType</c> to the given data buffer.
        /// </summary>
        /// <param name="data">The buffer containing data.</param>
        /// <param name="offset">Offset in the data buffer - updated afterwards to point
        /// to the next element in the buffer.</param>
        /// <param name="value">The value to be written to the buffer.</param>
        public static void WriteVariableType(this byte[] data, ref int offset, VariableType value)
        {
            Debug.Assert(data != null);
            Debug.Assert(offset + 2 <= data.Length);

            int v = (int)value;

            data[offset++] = (byte)((v >> 8) & 0x0ff);
            data[offset++] = (byte)((v >> 0) & 0x0ff);
        }

        /// <summary>
        /// Size in bytes that <c cref="VariableType">VariableType</c> requires on a buffer.
        /// </summary>
        /// <param name="_"></param>
        /// <returns>Number of bytes required on the buffer</returns>
        public static int GetSizeOnBuffer(this VariableType _)
        {
            return 2;
        }
    }
}
