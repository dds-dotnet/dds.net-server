using DDS.Net.Server.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDS.Net.Server.Core.Internal
{
    internal class CommonServer
    {
        protected readonly string _IPv4;
        protected readonly ushort _port;
        protected readonly ILogger _logger;

        public CommonServer(string IPv4, ushort port, ILogger logger)
        {
            _IPv4 = IPv4;
            _port = port;
            _logger = logger;
        }
    }
}
