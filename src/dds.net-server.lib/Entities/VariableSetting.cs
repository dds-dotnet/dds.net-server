namespace DDS.Net.Server.Entities
{
    public class VariableSetting
    {
        public string Name { get; private set; }

        public VariableSetting(string name)
        {
            Name = name;
        }
    }
}
