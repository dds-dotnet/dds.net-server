using DDS.Net.Server.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDS.Net.Server.Core.Internal
{
    internal class TcpServer : CommonServer
    {
        public TcpServer(string IPv4, ushort port, ILogger logger) : base(IPv4, port, logger)
        {
        }
    }
}
