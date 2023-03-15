using DDS.Net.Server.Core.Internal;

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

            _logger.Info($"");
        }

        private void PrintLogStarting()
        {
            _logger.Info($"{VersionInfo.SERVER_NAME} v{VersionInfo.SERVER_VERSION}");
            _logger.Info($"");
            _logger.Info($"Starting at {DateTime.UtcNow:yyyy/MM/dd - HH:mm:ss} with network configuration:");

            PrintLogPorts();
        }

        private void PrintLogStopping()
        {
            _logger.Info($"");
            _logger.Info($"Stopping at {DateTime.UtcNow:yyyy/MM/dd - HH:mm:ss} with network configuration:");

            PrintLogPorts();
        }
    }
}
