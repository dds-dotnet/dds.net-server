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

        public DdsServer(ServerConfiguration configuration)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            _config = configuration;

            if (_config.Logger != null)
                _logger = _config.Logger;
            else
                throw new Exception($"No instance of {nameof(ILogger)} is provided");
        }
    }
}
