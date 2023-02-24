using DDS.Net.Server.Entities;
using DDS.Net.Server.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDS.Net.Server
{
    public class DdsServer
    {
        private readonly ServerConfiguration _config;
        private readonly ILogger _logger;

        public DdsServer(ServerConfiguration config)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));

            _config = config;

            if (_config.Logger != null)
                _logger = _config.Logger;
            else
                throw new Exception($"No instance of {nameof(ILogger)} is provided");
        }

        public void Start()
        {
            PrintStartingLog();
        }

        private void PrintStartingLog()
        {
            _logger.Info($"Starting with config:");

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

        public void Stop()
        {

        }
    }
}
