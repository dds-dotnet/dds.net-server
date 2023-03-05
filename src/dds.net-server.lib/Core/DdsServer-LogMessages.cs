namespace DDS.Net.Server
{
    public partial class DdsServer
    {
        private void PrintLogPorts()
        {
            if (_serverConfig.EnableTCP)
            {
                _logger.Info($"    TCP Port {_serverConfig.ListeningAddressIPv4}:{_serverConfig.ListeningPortTCP}");
            }
            else
            {
                _logger.Info($"    TCP Connections - Disabled");
            }

            if (_serverConfig.EnableUDP)
            {
                _logger.Info($"    UDP Port {_serverConfig.ListeningAddressIPv4}:{_serverConfig.ListeningPortUDP}");
            }
            else
            {
                _logger.Info($"    UDP Connections - Disabled");
            }
        }

        private void PrintLogStarting()
        {
            _logger.Info($"Starting at {DateTime.UtcNow} with config:");

            PrintLogPorts();
        }

        private void PrintLogStopping()
        {
            _logger.Info($"Stopping at {DateTime.UtcNow} with config:");

            PrintLogPorts();
        }
    }
}
