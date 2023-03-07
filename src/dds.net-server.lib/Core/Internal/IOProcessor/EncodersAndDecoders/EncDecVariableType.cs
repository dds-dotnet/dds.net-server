using DDS.Net.Server.Entities;

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

            if (v >= 0 && v < (int)VariableType.UnknownVariableType)
            {
                return (VariableType)v;
            }

            return VariableType.UnknownVariableType;
        }
        public static void WriteVariableType(this byte[] data, ref int offset, VariableType value)
        {
            data.ThrowIfNotHavingRequiredBytes(ref offset, 1);

            data[offset++] = (byte)value;
        }
        public static int GetSizeOnBuffer(this VariableType variableType)
        {
            return 1;
        }
    }
}
