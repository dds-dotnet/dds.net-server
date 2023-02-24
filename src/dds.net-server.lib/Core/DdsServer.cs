using DDS.Net.Server.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDS.Net.Server
{
    public class DdsServer
    {
        private readonly ServerConfiguration _configuration;

        public DdsServer(ServerConfiguration configuration)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            _configuration = configuration;

            if (_configuration.Logger == null)
                throw new Exception($"No {nameof(_configuration.Logger)} instance is provided");
        }
    }
}
