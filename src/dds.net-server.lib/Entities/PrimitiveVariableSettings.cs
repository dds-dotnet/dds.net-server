namespace DDS.Net.Server.Entities
{
    public class PrimitiveVariableSettings : VariableSettings
    {
        public PrimitiveType PrimitiveType { get; private set; }

        public PrimitiveVariableSettings(string name, PrimitiveType type) : base(name)
        {
            PrimitiveType = type;
        }
    }
}
