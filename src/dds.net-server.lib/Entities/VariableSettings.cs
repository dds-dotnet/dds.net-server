namespace DDS.Net.Server.Entities
{
    public class VariableSettings
    {
        public string VariableName { get; private set; }

        public VariableSettings(string name)
        {
            VariableName = name;
        }
    }
}
