namespace DDS.Net.Server.Entities
{
    public class VariablesConfiguration
    {
        public List<BaseVariableSettings> Settings { get; private set; } = new();

        public void AddVariableSettings(BaseVariableSettings settings)
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
