namespace DDS.Net.Server.Core.Internal.IOProcessor.Types
{
    internal enum VariableType
    {
        String = 0,
        Boolean = 1,

        Byte = 2,          // 1-Byte Signed Integer
        Word = 3,          // 2-Byte Signed Integer
        DWord = 4,         // 4-Byte Signed Integer
        QWord = 5,         // 8-Byte Signed Integer

        UnsignedByte = 6,  // 1-Byte Unsigned Integer
        UnsignedWord = 7,  // 2-Byte Unsigned Integer
        UnsignedDWord = 8, // 4-Byte Unsigned Integer
        UnsignedQWord = 9, // 8-Byte Unsigned Integer

        Single = 10,        // 4-Byte Floating-point
        Double = 11,        // 8-Byte Floating-point

        UNKNOWN
    }
}
