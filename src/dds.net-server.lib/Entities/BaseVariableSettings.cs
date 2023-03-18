namespace DDS.Net.Server.Entities
{
    /// <summary>
    /// Base class for variable settings.
    /// </summary>
    public class BaseVariableSettings
    {
        /// <summary>
        /// Name of the variable.
        /// </summary>
        public string VariableName { get; private set; }

        /// <summary>
        /// Initializes the class with given parameters.
        /// </summary>
        /// <param name="name">The name of the variable.</param>
        public BaseVariableSettings(string name)
        {
            VariableName = name;
        }
    }
}
