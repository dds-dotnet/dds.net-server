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
        public UdpServer(string IPv4, ushort port, int maxClients, ILogger logger)
            : base(IPv4, port, maxClients, logger)
        {
        }
    }
}
