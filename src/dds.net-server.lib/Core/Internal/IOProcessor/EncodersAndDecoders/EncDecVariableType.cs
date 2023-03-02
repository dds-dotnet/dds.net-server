using DDS.Net.Server.Core.Internal.IOProcessor.Types;

namespace DDS.Net.Server.Core.Internal.IOProcessor.EncodersAndDecoders
{
    internal static class EncDecVariableType
    {
        //- 
        //- VariableType
        //- 
        public static VariableType ReadVariableType(this byte[] data, ref int offset)
        {
            data.ThrowIfNotHavingRequiredBytes(ref offset, 1);

            int v = data[offset++];

            if (v >= 0 && v < (int)VariableType.UNKNOWN)
            {
                return (VariableType)v;
            }

            return VariableType.UNKNOWN;
        }
        public static void WriteVariableType(this byte[] data, ref int offset, VariableType value)
        {
            data.ThrowIfNotHavingRequiredBytes(ref offset, 1);

            data[offset++] = (byte)value;
        }
    }
}
