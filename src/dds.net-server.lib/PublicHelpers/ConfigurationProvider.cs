﻿using DDS.Net.Server.Entities;
using DDS.Net.Server.Interfaces;

namespace DDS.Net.Server.PublicHelpers
{
    public static class ConfigurationProvider
    {
        /// <summary>
        /// Reading ServerConfiguration object from given .ini file.
        /// </summary>
        /// <param name="filename">.ini file containing configuration</param>
        /// <param name="logger">for logging and adding instance to returned configuration</param>
        /// <returns>(boolean isEnabled, ServerConfiguration)</returns>
        public static Tuple<bool, ServerConfiguration?> GetServerConfiguration(string filename, ILogger logger)
        {
            INIConfigIO _confReader = new INIConfigIO(filename, logger);

            string serverEnabledConfig = _confReader.GetString("DDS Connections/Enabled").ToLower();

            if (serverEnabledConfig.Contains("true") ||
                serverEnabledConfig.Contains("yes"))
            {
                string tcpEnabledConfig = _confReader.GetString("DDS Connections/TCP-Enabled").ToLower();
                string udpEnabledConfig = _confReader.GetString("DDS Connections/UDP-Enabled").ToLower();

                return new(true, new ServerConfiguration(

                    listeningIPv4Address: _confReader.GetString("DDS Connections/ListeningIPv4"),

                    enableTCP: tcpEnabledConfig.Contains("true") || tcpEnabledConfig.Contains("yes"),
                    tcpPort: (ushort)_confReader.GetInteger("DDS Connections/TCP-ListeningPort"),
                    tcpMaxClients: _confReader.GetInteger("DDS Connections/TCP-MaxClients"),

                    enableUDP: udpEnabledConfig.Contains("true") || udpEnabledConfig.Contains("yes"),
                    udpPort: (ushort)_confReader.GetInteger("DDS Connections/UDP-ListeningPort"),
                    udpMaxClients: _confReader.GetInteger("DDS Connections/UDP-MaxClients"),

                    logger: logger

                    ));
            }

            return new(false, null);
        }
    }
}
