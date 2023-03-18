namespace DDS.Net.Server.Entities
{
    /// <summary>
    /// Enumerating the <c>main</c> variable types.
    /// </summary>
    public enum VariableType
    {
        /// <summary>
        /// Represents the very basic variable type, e.g., string, integer, float, etc.
        /// </summary>
        Primitive,
        /// <summary>
        /// Represents a sequence of bytes (unsigned bytes)
        /// </summary>
        RawBytes,

        /// <summary>
        /// Unknown type
        /// </summary>
        UnknownVariableType
    }
}
