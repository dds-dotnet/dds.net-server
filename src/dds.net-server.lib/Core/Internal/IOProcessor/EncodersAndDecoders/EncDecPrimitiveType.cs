using DDS.Net.Server.Entities;
using System.Diagnostics;

namespace DDS.Net.Server.Core.Internal.IOProcessor.EncodersAndDecoders
{
    internal static class EncDecPrimitiveType
    {
        //- 
        //- PrimitiveType
        //- 

        /// <summary>
        /// Reads <c cref="PrimitiveType">PrimitiveType</c> from the data buffer
        /// and updates the offset past the <c cref="PrimitiveType">PrimitiveType</c>.
        /// </summary>
        /// <param name="data">The buffer containing data.</param>
        /// <param name="offset">Offset in the data buffer - updated afterwards to point
        /// to the next element in the buffer.</param>
        /// <returns><c cref="PrimitiveType">PrimitiveType</c></returns>
        public static PrimitiveType ReadPrimitiveType(this byte[] data, ref int offset)
        {
            Debug.Assert(data != null);
            Debug.Assert(offset + 1 <= data.Length);

            int v = data[offset++];

            if (v >= 0 && v < (int)PrimitiveType.UnknownPrimitiveType)
            {
                return (PrimitiveType)v;
            }

            return PrimitiveType.UnknownPrimitiveType;
        }

        /// <summary>
        /// Writes <c cref="PrimitiveType">PrimitiveType</c> to the given data buffer.
        /// </summary>
        /// <param name="data">The buffer containing data.</param>
        /// <param name="offset">Offset in the data buffer - updated afterwards to point
        /// to the next element in the buffer.</param>
        /// <param name="value">The value to be written on the buffer.</param>
        public static void WritePrimitiveType(this byte[] data, ref int offset, PrimitiveType value)
        {
            Debug.Assert(data != null);
            Debug.Assert(offset + 1 <= data.Length);

            data[offset++] = (byte)value;
        }

        /// <summary>
        /// Size in bytes <c cref="PrimitiveType">PrimitiveType</c> requires on a buffer
        /// </summary>
        /// <param name="_"></param>
        /// <returns>Number of bytes required on the buffer</returns>
        public static int GetSizeOnBuffer(this PrimitiveType _)
        {
            return 1;
        }
    }
}
