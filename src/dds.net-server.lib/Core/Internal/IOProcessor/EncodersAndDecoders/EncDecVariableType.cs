using DDS.Net.Server.Entities;

namespace DDS.Net.Server.Core.Internal.IOProcessor.EncodersAndDecoders
{
    internal static class EncDecVariableType
    {
        //- 
        //- VariableType
        //- 
        public static PrimitiveVariableType ReadVariableType(this byte[] data, ref int offset)
        {
            data.ThrowIfNotHavingRequiredBytes(ref offset, 1);

            int v = data[offset++];

            if (v >= 0 && v < (int)PrimitiveVariableType.UNKNOWN)
            {
                return (PrimitiveVariableType)v;
            }

            return PrimitiveVariableType.UNKNOWN;
        }
        public static void WriteVariableType(this byte[] data, ref int offset, PrimitiveVariableType value)
        {
            data.ThrowIfNotHavingRequiredBytes(ref offset, 1);

            data[offset++] = (byte)value;
        }
    }
}
