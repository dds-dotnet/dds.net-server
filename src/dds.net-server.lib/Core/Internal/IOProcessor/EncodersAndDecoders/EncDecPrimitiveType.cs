using DDS.Net.Server.Entities;

namespace DDS.Net.Server.Core.Internal.IOProcessor.EncodersAndDecoders
{
    internal static class EncDecPrimitiveType
    {
        //- 
        //- PrimitiveType
        //- 

        /// <summary>
        /// Reads PrimitiveType from the data buffer and updates the offset past the PrimitiveType
        /// </summary>
        /// <param name="data">The buffer containing data</param>
        /// <param name="offset">offset in the data buffer - updated afterwards to point
        /// to the next element in the buffer</param>
        /// <returns>PrimitiveType</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static PrimitiveType ReadPrimitiveType(this byte[] data, ref int offset)
        {
            data.ThrowIfNotHavingRequiredBytes(ref offset, 1);

            int v = data[offset++];

            if (v >= 0 && v < (int)PrimitiveType.UnknownPrimitiveType)
            {
                return (PrimitiveType)v;
            }

            return PrimitiveType.UnknownPrimitiveType;
        }
        /// <summary>
        /// Writes PrimitiveType to the given data buffer
        /// </summary>
        /// <param name="data">The buffer containing data</param>
        /// <param name="offset">Offset in the data buffer - updated afterwards to point
        /// to the next element in the buffer</param>
        /// <param name="value">value to be written to the buffer</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static void WritePrimitiveType(this byte[] data, ref int offset, PrimitiveType value)
        {
            data.ThrowIfNotHavingRequiredBytes(ref offset, 1);

            data[offset++] = (byte)value;
        }
        /// <summary>
        /// Size in bytes PrimitiveType requires on a buffer
        /// </summary>
        /// <param name="_"></param>
        /// <returns>Number of bytes required on the buffer</returns>
        public static int GetSizeOnBuffer(this PrimitiveType _)
        {
            return 1;
        }
    }
}
