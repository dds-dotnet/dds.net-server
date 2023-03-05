namespace DDS.Net.Server.Entities
{
    public class VariableConfiguration
    {
        public string Name { get; private set; }

        public VariableConfiguration(string name)
        {
            Name = name;
        }
    }
}
