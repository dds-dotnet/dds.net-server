namespace DDS.Net.Server.Entities
{
    /// <summary>
    /// A child class of <c cref="BaseVariableSettings">BaseVariableSettings</c> to
    /// represent settings for <c cref="VariableType.RawBytes">VariableType.RawBytes</c> variables.
    /// </summary>
    public class RawBytesVariableSettings : BaseVariableSettings
    {
        public byte[] Data { get; private set; }

        /// <summary>
        /// Initializes the variable settings object with given parameters.
        /// </summary>
        /// <param name="name">The name of RawBytes variable.</param>
        /// <param name="data">Initial data for RawBytes variable.</param>
        public RawBytesVariableSettings(string name, byte[] data) : base(name)
        {
            Data = data;
        }
    }
}
