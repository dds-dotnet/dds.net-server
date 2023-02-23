using DDS.Net.Server.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDS.Net.Server.Services
{
    internal static class DependencyService
    {
        private static ILogger _logger = null;

        public static ILogger GetLogger()
        {
            return _logger;
        }
    }
}
