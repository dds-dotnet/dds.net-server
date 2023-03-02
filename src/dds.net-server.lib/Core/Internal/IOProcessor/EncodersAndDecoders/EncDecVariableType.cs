namespace DDS.Net.Server.Core.Internal.IOProcessor.EncodersAndDecoders
{
    internal static class EncDecVariableType
    {
        //- 
        //- VariableType
        //- 
        public static Types.VariableType ReadVariableType(this byte[] data, ref int offset)
        {
            data.ThrowIfNotHavingRequiredBytes(ref offset, 1);

            int v = data[offset++];

            if (v >= 0 && v < (int)Types.VariableType.UNKNOWN)
            {
                return (Types.VariableType)v;
            }

            return Types.VariableType.UNKNOWN;
        }
        public static void WriteVariableType(this byte[] data, ref int offset, Types.VariableType value)
        {
            data.ThrowIfNotHavingRequiredBytes(ref offset, 1);

            data[offset++] = (byte)value;
        }
    }
}
