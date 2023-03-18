namespace DDS.Net.Server.Entities
{
    /// <summary>
    /// Enumerates the sub-type of <c cref="VariableType.Primitive">VariableType.Primitive</c> variable.
    /// </summary>
    public enum PrimitiveType
    {
        /// <summary>
        /// Represents a string of characters
        /// </summary>
        String = 0,
        /// <summary>
        /// Represents a boolean (True or False)
        /// </summary>
        Boolean = 1,
        /// <summary>
        /// Represents a 1-byte Signed Integer
        /// </summary>
        Byte = 2,
        /// <summary>
        /// Represents a 2-byte Signed Integer
        /// </summary>
        Word = 3,
        /// <summary>
        /// Represents a 4-byte Signed Integer
        /// </summary>
        DWord = 4,
        /// <summary>
        /// Represents a 8-byte Signed Integer
        /// </summary>
        QWord = 5,
        /// <summary>
        /// Represents a 1-byte Unsigned Integer
        /// </summary>
        UnsignedByte = 6,
        /// <summary>
        /// Represents a 2-byte Unsigned Integer
        /// </summary>
        UnsignedWord = 7,
        /// <summary>
        /// Represents a 4-byte Unsigned Integer
        /// </summary>
        UnsignedDWord = 8,
        /// <summary>
        /// Represents a 8-byte Unsigned Integer
        /// </summary>
        UnsignedQWord = 9,
        /// <summary>
        /// Represents a single precision 4-byte Floating-point number
        /// </summary>
        Single = 10,
        /// <summary>
        /// Represents a double precision 8-byte Floating-point number
        /// </summary>
        Double = 11,

        /// <summary>
        /// Unknown type
        /// </summary>
        UnknownPrimitiveType
    }
}
