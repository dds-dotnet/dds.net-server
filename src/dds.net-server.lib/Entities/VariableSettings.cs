namespace DDS.Net.Server.Entities
{
    public class VariableSettings
    {
        public string Name { get; private set; }

        public VariableSettings(string name)
        {
            Name = name;
        }
    }
}
