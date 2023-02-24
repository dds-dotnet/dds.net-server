using DDS.Net.Server.Extensions;
using DDS.Net.Server.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DDS.Net.Server.Core.Internal
{
    internal abstract class BaseServer
    {
        protected readonly string _IPv4;
        protected readonly ushort _port;
        protected readonly int _maxClients;

        protected readonly ILogger _logger;

        protected BaseServer(string IPv4, ushort port, int maxClients, ILogger logger)
        {
            _IPv4 = IPv4;
            _port = port;
            _maxClients = maxClients;

            _logger = logger;

            // -------------
            // Validating the given IP address
            // ---------
            Regex ipv4Pattern = new Regex(@"\s*(\d{1,3})\s*\.\s*(\d{1,3})\s*\.\s*(\d{1,3})\s*\.\s*(\d{1,3})\s*");
            Regex spacesPattern = new Regex(@"\s*");

            if (string.IsNullOrEmpty(IPv4) ||
                IPv4.ToLower().Contains("any") ||
                IPv4.ToLower().Contains("all"))
            {
                _IPv4 = "0.0.0.0";
            }
            else if (ipv4Pattern.IsMatch(IPv4) == false)
            {
                logger.Warning($"Invalid IPv4 Address: \"{IPv4}\", using \"0.0.0.0\" instead");
                _IPv4 = "0.0.0.0";
            }
            else
            {
                _IPv4 = spacesPattern.Replace(_IPv4, "");

                if (_IPv4.IsIPAddressAssignedToAnUpInterface())
                {
                    logger.Warning($"Local IPv4 Address \"{IPv4}\" does not exist, using \"0.0.0.0\" instead");
                    _IPv4 = "0.0.0.0";
                }
            }

            // -------------
            // Validating given max-clients
            // ---------
            if (maxClients <= 0)
            {
                logger.Warning($"Invalid maximum number of clients: \"{maxClients}\", using 10 instead");
                _maxClients = 10;
            }
        }

        public abstract void StartServer();
        public abstract void StopServer();
    }
}
