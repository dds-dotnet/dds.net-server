namespace DDS.Net.Server.Entities
{
    /// <summary>
    /// The class <c>VariablesConfiguration</c> represents starting configuration
    /// for variables.
    /// </summary>
    public class VariablesConfiguration
    {
        /// <summary>
        /// List of settings for individual variables.
        /// </summary>
        public List<BaseVariableSettings> Settings { get; private set; } = new();

        /// <summary>
        /// Adds a new settings object to the list. Overrides previous variable
        /// settings object (if any available) that has the same name as
        /// the newly provided variable settings object.
        /// </summary>
        /// <param name="settings">The new settings object.</param>
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
