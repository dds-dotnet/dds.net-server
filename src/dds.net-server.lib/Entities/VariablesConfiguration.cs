namespace DDS.Net.Server.Entities
{
    public class VariablesConfiguration
    {
        public List<VariableSettings> Settings { get; private set; } = new();

        public void AddVariableSettings(VariableSettings settings)
        {
            foreach (var setting in Settings)
            {
                if (setting.VariableName == settings.VariableName)
                {
                    Settings.Remove(setting);
                    break;
                }
            }

            Settings.Add(settings);
        }
    }
}
