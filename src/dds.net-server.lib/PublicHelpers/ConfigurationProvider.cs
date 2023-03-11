﻿using DDS.Net.Server.Core.Internal.Extensions;
using DDS.Net.Server.Entities;
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

            if (_confReader.GetString("DDS Connections/Enabled").ContainsAnyIgnoringCase("true", "yes"))
            {
                return new(true, new ServerConfiguration(

                    listeningIPv4Address: _confReader.GetString("DDS Connections/ListeningIPv4"),

                    enableTCP: _confReader.GetString("DDS Connections/TCP-Enabled").ContainsAnyIgnoringCase("true", "yes"),
                    tcpPort: (ushort)_confReader.GetInteger("DDS Connections/TCP-ListeningPort"),
                    tcpMaxClients: _confReader.GetInteger("DDS Connections/TCP-MaxClients"),

                    enableUDP: _confReader.GetString("DDS Connections/UDP-Enabled").ContainsAnyIgnoringCase("true", "yes"),
                    udpPort: (ushort)_confReader.GetInteger("DDS Connections/UDP-ListeningPort"),

                    logger: logger

                    ));
            }

            return new(false, null);
        }

        /// <summary>
        /// Reading VariablesConfiguration object from provided .ini file.
        /// </summary>
        /// <param name="filename">.ini file containing configuration</param>
        /// <param name="logger">logging</param>
        /// <returns>VariablesConfiguration object read from .ini file</returns>
        /// <exception cref="NotImplementedException"></exception>
        public static VariablesConfiguration GetVariablesConfiguration(string filename, ILogger logger)
        {
            INIConfigIO _confReader = new INIConfigIO(filename, logger);
            VariablesConfiguration variablesConfiguration = new VariablesConfiguration();

            foreach (string variableName in _confReader.GetSectionNames())
            {
                try
                {
                    VariableType variableType =
                        Enum.Parse<VariableType>(_confReader.GetString($"{variableName}/VariableType"));

                    if (variableType == VariableType.Compound)
                    {
                        string primVal = _confReader.GetString($"{variableName}/PrimitiveNames");
                        string[] primValsSplit = primVal.Split(
                                            ',',
                                            StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                        variablesConfiguration.AddVariableSettings(
                            new CompoundVariableSettings(
                                variableName,
                                new List<string>(primValsSplit)));
                    }
                    else if (variableType == VariableType.Primitive)
                    {
                        PrimitiveType primitiveType =
                            Enum.Parse<PrimitiveType>(_confReader.GetString($"{variableName}/PrimitiveType"));

                        variablesConfiguration.AddVariableSettings(
                            new PrimitiveVariableSettings(variableName, primitiveType));
                    }
                }
                catch(Exception ex)
                {
                    logger.Error($"Config reading error: {ex.Message}");
                }
            }

            return variablesConfiguration;
        }
    }
}
