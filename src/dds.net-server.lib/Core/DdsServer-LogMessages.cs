using DDS.Net.Server.Entities;
using DDS.Net.Server.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDS.Net.Server
{
    public partial class DdsServer
    {
        private void PrintPorts()
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

        private void PrintStartingLog()
        {
            _logger.Info($"Starting with config:");

            PrintPorts();
        }

        private void PrintStoppingLog()
        {
            _logger.Info($"Stopping with config:");

            PrintPorts();
        }
    }
}
