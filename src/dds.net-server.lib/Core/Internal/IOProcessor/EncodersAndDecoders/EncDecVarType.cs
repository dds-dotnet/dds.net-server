namespace DDS.Net.Server.Core.Internal.IOProcessor.EncodersAndDecoders
{
    internal static class EncDecVarType
    {
        //- 
        //- VarType
        //- 
        public static Types.VarType ReadVarType(this byte[] data, ref int offset)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (offset < 0 || offset >= data.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }

            int v = data[offset++];

            if (v >= 0 && v < (int)Types.VarType.UNKNOWN)
            {
                return (Types.VarType)v;
            }

            return Types.VarType.UNKNOWN;
        }
        public static void WriteVarType(this byte[] data, ref int offset, Types.VarType value)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (offset < 0 || offset >= data.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }

            data[offset++] = (byte)value;
        }
    }
}
