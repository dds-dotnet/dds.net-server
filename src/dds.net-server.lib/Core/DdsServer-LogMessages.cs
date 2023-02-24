namespace DDS.Net.Server
{
    public partial class DdsServer
    {
        private void PrintLogPorts()
        {
            if (_config.EnableTCP)
            {
                _logger.Info($"    TCP Port {_config.ListeningAddressIPv4}:{_config.ListeningPortTCP}");
            }
            else
            {
                _logger.Info($"    TCP Connections - Disabled");
            }

            if (_config.EnableUDP)
            {
                _logger.Info($"    UDP Port {_config.ListeningAddressIPv4}:{_config.ListeningPortUDP}");
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
