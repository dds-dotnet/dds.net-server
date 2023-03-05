using DDS.Net.Server.Entities;

namespace DDS.Net.Server.Core.Internal.IOProcessor.EncodersAndDecoders
{
    internal static class EncDecVariableType
    {
        //- 
        //- VariableType
        //- 
        public static PrimitiveType ReadVariableType(this byte[] data, ref int offset)
        {
            data.ThrowIfNotHavingRequiredBytes(ref offset, 1);

            int v = data[offset++];

            if (v >= 0 && v < (int)PrimitiveType.UNKNOWN)
            {
                return (PrimitiveType)v;
            }

            return PrimitiveType.UNKNOWN;
        }
        public static void WriteVariableType(this byte[] data, ref int offset, PrimitiveType value)
        {
            data.ThrowIfNotHavingRequiredBytes(ref offset, 1);

            data[offset++] = (byte)value;
        }
    }
}
