using DDS.Net.Server.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDS.Net.Server.Core.Internal
{
    internal class UdpServer : CommonServer
    {
        private readonly string _IPv4;
        private readonly ushort _port;
        private readonly ILogger _logger;

        public UdpServer(string IPv4, ushort port, ILogger logger)
        {
            _IPv4 = IPv4;
            _port = port;
            _logger = logger;
        }
    }
}
