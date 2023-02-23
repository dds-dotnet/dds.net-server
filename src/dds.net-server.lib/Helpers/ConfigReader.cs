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

        /// <summary>
        /// Getting a string value from INI file
        /// </summary>
        /// <param name="key">Section Name / Property Name</param>
        /// <param name="defaultValue">Return the value when key is not found</param>
        /// <returns></returns>
        public string GetString(string key, string defaultValue = "")
        {
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
            return defaultValue;
        }
    }
}
