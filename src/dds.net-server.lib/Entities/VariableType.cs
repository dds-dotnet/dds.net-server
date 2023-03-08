namespace DDS.Net.Server.Entities
{
    public enum VariableType
    {
        /// <summary>
        /// Represents the very basic variable type, e.g., string, integer, float, etc.
        /// </summary>
        Primitive,
        /// <summary>
        /// Represents a combination of basic variable types
        /// </summary>
        Compound,
        /// <summary>
        /// Unknown type
        /// </summary>
        UnknownVariableType
    }
}
