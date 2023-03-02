namespace DDS.Net.Server.Core.Internal.IOProcessor.EncodersAndDecoders
{
    internal static class EncDecVarType
    {
        //- 
        //- VarType
        //- 
        public static Types.VarType ReadVarType(this byte[] data, ref int offset)
        {
            data.CheckForRequiredSize(ref offset, 1);

            int v = data[offset++];

            if (v >= 0 && v < (int)Types.VarType.UNKNOWN)
            {
                return (Types.VarType)v;
            }

            return Types.VarType.UNKNOWN;
        }
        public static void WriteVarType(this byte[] data, ref int offset, Types.VarType value)
        {
            data.CheckForRequiredSize(ref offset, 1);

            data[offset++] = (byte)value;
        }
    }
}
