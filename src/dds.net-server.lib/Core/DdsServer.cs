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

            if (_config.Logger == null)
                throw new Exception($"No {nameof(_config.Logger)} instance is provided");

            _logger = _config.Logger;
        }
    }
}
