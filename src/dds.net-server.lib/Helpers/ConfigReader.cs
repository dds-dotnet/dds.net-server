using DDS.Net.Server.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDS.Net.Server.Helpers
{
    public class ConfigReader
    {
        public string Filename { get; set; }

        private Dictionary<string, Dictionary<string, string>> _config;

        public ConfigReader(string filename)
        {
            Filename = filename;
            _config = new Dictionary<string, Dictionary<string, string>>();

            if (File.Exists(Filename) == false) return;
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
    }
}
