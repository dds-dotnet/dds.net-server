using DDS.Net.Server.PublicExtensions;
using DDS.Net.Server.Interfaces;
using System.Text.RegularExpressions;

namespace DDS.Net.Server.PublicHelpers
{
    public class INIConfigIO
    {
        public string Filename { get; set; }

        private Dictionary<string, Dictionary<string, string>> _config;
        private ILogger? _logger;

        public INIConfigIO(string filename, ILogger? logger = null)
        {
            Filename = filename;
            _config = new();
            _logger = logger;

            LoadFile();
        }

        private void LoadFile()
        {
            _config.Clear();

            if (File.Exists(Filename) == false)
            {
                _logger?.Warning($"Cannot load configuration file \"{Filename}\"");
                return;
            }

            Regex commentPattern = new(@"[;].*$");
            Regex sectionPattern = new(@"^\s*\[([a-zA-Z0-9\s_-]+)\]\s*$");
            Regex propertyPattern = new(@"^\s*([a-zA-Z0-9\s_-]+)\s*=+\s*([a-zA-Z0-9.\s_-]+)\s*$");

            string? currentSection = null;

            using (StreamReader stream = File.OpenText(Filename))
            {
                while (stream.EndOfStream == false)
                {
                    string? line = stream.ReadLine();

                    if (string.IsNullOrEmpty(line) == false)
                    {
                        string trimmedLine = commentPattern.Replace(line, string.Empty).Trim();

                        if (trimmedLine != "")
                        {
                            if (trimmedLine.StartsWith('['))
                            {
                                currentSection = null;

                                if (sectionPattern.IsMatch(trimmedLine))
                                {
                                    currentSection = sectionPattern.Match(trimmedLine).Groups[1].Value;
                                }
                                else
                                {
                                    _logger?.Warning($"Skipping malformed section \"{trimmedLine}\" in configuration file \"{Filename}\"");
                                }
                            }
                            else
                            {
                                if (currentSection != null && propertyPattern.IsMatch(trimmedLine))
                                {
                                    Match match = propertyPattern.Match(trimmedLine);

                                    string property = match.Groups[1].Value.Trim();
                                    string value = match.Groups[2].Value.Trim();

                                    InsertValueInConfiguration(currentSection, property, value);
                                }
                            }
                        }
                    }
                }
            }

        }

        /// <summary>
        /// Cleans loaded configuration values in-memory only. File is not updated unless saved.
        /// </summary>
        public void Clear()
        {
            _logger?.Info($"Clearing configuration from file \"{Filename}\"");
            _config.Clear();
        }
        /// <summary>
        /// Saves currently held configuration values in-memory to already specified file.
        /// </summary>
        public void SaveFile()
        {
            _logger?.Info($"Writing configuration to file \"{Filename}\"");

            using (StreamWriter stream = File.CreateText(Filename))
            {
                bool isFirst = true;

                foreach (KeyValuePair<string, Dictionary<string, string>> section in _config)
                {
                    if (!isFirst)
                    {
                        stream.WriteLine($"");
                    }

                    stream.WriteLine($"[{section.Key}]");

                    foreach (KeyValuePair<string, string> prop in section.Value)
                    {
                        stream.WriteLine($"{prop.Key} = {prop.Value}");
                    }

                    isFirst = false;
                }
            }
        }

        private void InsertValueInConfiguration(string sectionName, string propertyName, string propertyValue)
        {
            Dictionary<string, string>? section = GetSection(sectionName);

            if (section == null)
            {
                section = new();
                _config.Add(sectionName, section);
            }

            if (section.ContainsKey(propertyName) == false)
            {
                section.Add(propertyName, propertyValue);
            }
            else
            {
                section[propertyName] = propertyValue;
            }
        }

        private Dictionary<string, string>? GetSection(string sectionName)
        {
            if (_config.ContainsKey(sectionName)) return _config[sectionName];

            return null;
        }

        private string? GetValueFromSection(string sectionName, string propertyName)
        {
            Dictionary<string, string>? section = GetSection(sectionName);

            if (section == null) return null;

            if (section.ContainsKey(propertyName)) return section[propertyName];

            return null;
        }

        private Tuple<string?, string?> GetSectionAndPropertyName(string key)
        {
            if (string.IsNullOrEmpty(key) == false)
            {
                List<string> parts = key.TrimmedParts('/');

                if (parts.Count >= 2)
                {
                    return new Tuple<string?, string?>(parts[0], parts[1]);
                }
            }

            return new Tuple<string?, string?>(null, null);
        }

        /// <summary>
        /// Getting all the section names from INI file
        /// </summary>
        /// <returns>IEnumerable<string> of section names</returns>
        public IEnumerable<string> GetSectionNames()
        {
            foreach (string sectionName in _config.Keys)
            {
                yield return sectionName;
            }
        }

        /// <summary>
        /// Getting a string value from INI file
        /// </summary>
        /// <param name="key">Section Name / Property Name</param>
        /// <param name="defaultValue">Return the value when key is not found</param>
        /// <returns></returns>
        public string GetString(string key, string defaultValue = "")
        {
            (string? section, string? property) = GetSectionAndPropertyName(key);

            if (section != null && property != null)
            {
                string? value = GetValueFromSection(section, property);

                if (value != null) return value;
            }

            return defaultValue;
        }
        /// <summary>
        /// Getting an integer value from INI file
        /// </summary>
        /// <param name="key">Section Name / Property Name</param>
        /// <param name="defaultValue">Return the value when key is not found</param>
        /// <returns></returns>
        public int GetInteger(string key, int defaultValue = -1)
        {
            (string? section, string? property) = GetSectionAndPropertyName(key);

            if (section != null && property != null)
            {
                string? value = GetValueFromSection(section, property);

                if (value != null)
                {
                    if (int.TryParse(value, out int result))
                    {
                        return result;
                    }
                }
            }

            return defaultValue;
        }
        /// <summary>
        /// Getting a float value from INI file
        /// </summary>
        /// <param name="key">Section Name / Property Name</param>
        /// <param name="defaultValue">Return the value when key is not found</param>
        /// <returns></returns>
        public float GetFloat(string key, float defaultValue = 0)
        {
            (string? section, string? property) = GetSectionAndPropertyName(key);

            if (section != null && property != null)
            {
                string? value = GetValueFromSection(section, property);

                if (value != null)
                {
                    if (float.TryParse(value, out float result))
                    {
                        return result;
                    }
                }
            }

            return defaultValue;
        }
        /// <summary>
        /// Getting a double value from INI file
        /// </summary>
        /// <param name="key">Section Name / Property Name</param>
        /// <param name="defaultValue">Return the value when key is not found</param>
        /// <returns></returns>
        public double GetDouble(string key, double defaultValue = 0)
        {
            (string? section, string? property) = GetSectionAndPropertyName(key);

            if (section != null && property != null)
            {
                string? value = GetValueFromSection(section, property);

                if (value != null)
                {
                    if (double.TryParse(value, out double result))
                    {
                        return result;
                    }
                }
            }

            return defaultValue;
        }

        /// <summary>
        /// Setting a string value in INI file
        /// </summary>
        /// <param name="key">Section Name / Property Name</param>
        /// <param name="value">Desired value</param>
        /// <returns></returns>
        public void SetString(string key, string value)
        {
            (string? section, string? property) = GetSectionAndPropertyName(key);

            if (section != null && property != null)
            {
                InsertValueInConfiguration(section, property, value);
            }
            else
            {
                _logger?.Warning($"Invalid key \"{key}\" for setting \"{value}\"");
            }
        }
        /// <summary>
        /// Setting an integer value in INI file
        /// </summary>
        /// <param name="key">Section Name / Property Name</param>
        /// <param name="value">Desired value</param>
        /// <returns></returns>
        public void SetInteger(string key, int value)
        {
            (string? section, string? property) = GetSectionAndPropertyName(key);

            if (section != null && property != null)
            {
                InsertValueInConfiguration(section, property, $"{value}");
            }
            else
            {
                _logger?.Warning($"Invalid key \"{key}\" for setting integer {value}");
            }
        }
        /// <summary>
        /// Setting a float value in INI file
        /// </summary>
        /// <param name="key">Section Name / Property Name</param>
        /// <param name="value">Desired value</param>
        /// <returns></returns>
        public void SetFloat(string key, float value)
        {
            (string? section, string? property) = GetSectionAndPropertyName(key);

            if (section != null && property != null)
            {
                InsertValueInConfiguration(section, property, $"{value}");
            }
            else
            {
                _logger?.Warning($"Invalid key \"{key}\" for setting float {value}");
            }
        }
        /// <summary>
        /// Setting a double value in INI file
        /// </summary>
        /// <param name="key">Section Name / Property Name</param>
        /// <param name="value">Desired value</param>
        /// <returns></returns>
        public void SetDouble(string key, double value)
        {
            (string? section, string? property) = GetSectionAndPropertyName(key);

            if (section != null && property != null)
            {
                InsertValueInConfiguration(section, property, $"{value}");
            }
            else
            {
                _logger?.Warning($"Invalid key \"{key}\" for setting double {value}");
            }
        }
    }
}
