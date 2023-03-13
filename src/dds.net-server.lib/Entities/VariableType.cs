namespace DDS.Net.Server.Entities
{
    public enum VariableType
    {
        /// <summary>
        /// Represents the very basic variable type, e.g., string, integer, float, etc.
        /// </summary>
        Primitive,
        /// <summary>
        /// Represents a sequence of bytes
        /// </summary>
        RawBytes,
        /// <summary>
        /// Unknown type
        /// </summary>
        UnknownVariableType
    }
}
