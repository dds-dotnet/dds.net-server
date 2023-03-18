namespace DDS.Net.Server.Entities
{
    /// <summary>
    /// A child class of <c cref="BaseVariableSettings">BaseVariableSettings</c> to
    /// represent settings for <c cref="VariableType.Primitive">VariableType.Primitive</c> variables.
    /// </summary>
    public class PrimitiveVariableSettings : BaseVariableSettings
    {
        /// <summary>
        /// Type of the primitive variable.
        /// </summary>
        public PrimitiveType PrimitiveType { get; private set; }

        /// <summary>
        /// Initializes the settings object with given parameters.
        /// </summary>
        /// <param name="name">The name of the primitive variable.</param>
        /// <param name="type">The type of the primitive variable.</param>
        public PrimitiveVariableSettings(string name, PrimitiveType type) : base(name)
        {
            PrimitiveType = type;
        }
    }
}
