using DDS.Net.Server.Entities;

namespace DDS.Net.Server.Core.Internal.IOProcessor.EncodersAndDecoders
{
    internal static class EncDecPrimitiveType
    {
        //- 
        //- PrimitiveType
        //- 
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
        public static void WritePrimitiveType(this byte[] data, ref int offset, PrimitiveType value)
        {
            data.ThrowIfNotHavingRequiredBytes(ref offset, 1);

            data[offset++] = (byte)value;
        }
        public static int GetSizeOnBuffer(this PrimitiveType primitiveType)
        {
            return 1;
        }
    }
}
