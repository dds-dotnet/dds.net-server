namespace DDS.Net.Server.Entities
{
    public class BaseVariableSettings
    {
        public string VariableName { get; private set; }

        public BaseVariableSettings(string name)
        {
            VariableName = name;
        }
    }
}
