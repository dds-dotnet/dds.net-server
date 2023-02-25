using DDS.Net.Server.Core.Internal.Extensions;
using DDS.Net.Server.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DDS.Net.Server.Core.Internal.SimpleServer
{
    internal abstract class BaseServer
    {
        protected readonly string localAddressIPv4;
        protected readonly ushort localPort;
        protected readonly int maxNumberOfClients;

        protected readonly ILogger logger;

        protected BaseServer(string localAddressIPv4, ushort localPort, int maxNumberOfClients, ILogger logger)
        {
            this.localAddressIPv4 = localAddressIPv4;
            this.localPort = localPort;
            this.maxNumberOfClients = maxNumberOfClients;

            this.logger = logger;

            // -------------
            // Validating the given IP address
            // ---------
            Regex spacesPattern = new Regex(@"\s*");

            if (localAddressIPv4.IsEmpty() ||
                localAddressIPv4.ContainsAnyIgnoringCase("any", "all"))
            {
                this.localAddressIPv4 = "0.0.0.0";
            }
            else if (localAddressIPv4.IsInvalidIPv4Address())
            {
                logger.Warning($"Invalid IPv4 Address: \"{localAddressIPv4}\", using \"0.0.0.0\" instead");
                this.localAddressIPv4 = "0.0.0.0";
            }
            else
            {
                this.localAddressIPv4 = spacesPattern.Replace(this.localAddressIPv4, "");

                if (this.localAddressIPv4.IsIPAddressAssignedToAnUpInterface())
                {
                    logger.Warning($"Local IPv4 Address \"{localAddressIPv4}\" does not exist, using \"0.0.0.0\" instead");
                    this.localAddressIPv4 = "0.0.0.0";
                }
            }

            // -------------
            // Validating given max-clients
            // ---------
            if (maxNumberOfClients <= 0)
            {
                logger.Warning($"Invalid maximum number of clients: \"{maxNumberOfClients}\", using 10 instead");
                this.maxNumberOfClients = 10;
            }
        }

        public abstract void StartServer();
        public abstract void StopServer();
    }
}
