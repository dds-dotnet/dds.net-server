using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDS.Net.Server.Helpers
{
    public class ConfigReader
    {
        public string Filename { get; set; }

        public ConfigReader(string filename)
        {
            Filename = filename;
        }

        public string GetString(string key, string defaultValue = "")
        {
            return defaultValue;
        }

        public int GetInteger(string key, int defaultValue = -1)
        {
            return defaultValue;
        }

        public float GetFloat(string key, float defaultValue = 0)
        {
            return defaultValue;
        }

        public double GetDouble(string key, double defaultValue = 0)
        {
            return defaultValue;
        }
    }
}
