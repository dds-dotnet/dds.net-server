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
        private readonly string _IPv4;
        private readonly ushort _port;
        private readonly ILogger _logger;

        public TcpServer(string IPv4, ushort port, ILogger logger)
        {
            _IPv4 = IPv4;
            this._port = port;
            this._logger = logger;
        }
    }
}
