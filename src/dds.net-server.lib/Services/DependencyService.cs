using DDS.Net.Server.Interfaces;
using DDS.Net.Server.Interfaces.DefaultImplementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDS.Net.Server.Services
{
    internal static class DependencyService
    {
        private static ILogger _logger = new BlankLogger();

        public static ILogger GetLogger()
        {
            return _logger;
        }

        public static void SetLogger(ILogger logger)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));

            _logger = logger;
        }
    }
}
