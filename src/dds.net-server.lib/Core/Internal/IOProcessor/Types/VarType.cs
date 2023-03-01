namespace DDS.Net.Server.Core.Internal.IOProcessor.Types
{
    internal enum VarType
    {
        String = 0,
        Boolean,

        Byte,          // 1-Byte Signed Integer
        Word,          // 2-Byte Signed Integer
        DWord,         // 4-Byte Signed Integer
        QWord,         // 8-Byte Signed Integer

        UnsignedByte,  // 1-Byte Unsigned Integer
        UnsignedWord,  // 2-Byte Unsigned Integer
        UnsignedDWord, // 4-Byte Unsigned Integer
        UnsignedQWord, // 8-Byte Unsigned Integer

        Single,        // 4-Byte Floating-point
        Double,        // 8-Byte Floating-point
    }
}
