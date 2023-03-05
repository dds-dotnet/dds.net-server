namespace DDS.Net.Server.Entities
{
    public class CompoundVariableSettings : VariableSettings
    {
        public List<string> PrimitiveNames { get; private set; }

        public CompoundVariableSettings(string name, List<string> primitives) : base(name)
        {
            PrimitiveNames = primitives;
        }
    }
}
