﻿using DDS.Net.Server.Core.Internal;

namespace DDS.Net.Server
{
    public partial class DdsServer
    {
        private void PrintLogPorts()
        {
            if (_serverConfig.EnableTCP)
            {
                _logger.Info($"    TCP -        Port = {_serverConfig.ListeningAddressIPv4}:{_serverConfig.ListeningPortTCP}");
                _logger.Info($"        - Max clients = {_serverConfig.MaxClientsTCP}");
            }
            else
            {
                _logger.Info($"    TCP - Disabled");
            }

            if (_serverConfig.EnableUDP)
            {
                _logger.Info($"    UDP -        Port = {_serverConfig.ListeningAddressIPv4}:{_serverConfig.ListeningPortUDP}");
            }
            else
            {
                _logger.Info($"    UDP - Disabled");
            }
        }

        private void PrintLogStarting()
        {
            _logger.Info($"Starting {VersionInfo.SERVER_NAME} v{VersionInfo.SERVER_VERSION} " +
                         $"at {DateTime.UtcNow} with config:");

            PrintLogPorts();
        }

        private void PrintLogStopping()
        {
            _logger.Info($"Stopping at {DateTime.UtcNow} with config:");

            PrintLogPorts();
        }
    }
}
