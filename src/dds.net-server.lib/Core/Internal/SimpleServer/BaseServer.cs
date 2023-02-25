using DDS.Net.Server.Core.Internal.Extensions;
using DDS.Net.Server.Core.Internal.SimpleServer.Types;
using DDS.Net.Server.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DDS.Net.Server.Core.Internal.SimpleServer
{
    internal abstract partial class BaseServer
    {
        protected volatile SimpleServerStatus serverStatus = SimpleServerStatus.Stopped;

        protected readonly string localAddressIPv4;
        protected readonly ushort localPort;

        protected readonly IPEndPoint localEndPoint;
        protected readonly Socket? localSocket;

        private readonly SimpleServerType serverType;

        protected readonly int maxNumberOfClients;
        protected readonly ILogger logger;

        protected BaseServer(
            string localAddressIPv4,
            ushort localPort,
            int maxNumberOfClients,
            SimpleServerType serverType,
            ILogger logger)
        {
            SetServerStatus(SimpleServerStatus.Stopped);

            this.localAddressIPv4 = localAddressIPv4;
            this.localPort = localPort;
            this.maxNumberOfClients = maxNumberOfClients;
            this.serverType = serverType;
            this.logger = logger;

            // -------------
            // Validating the given IP address
            // ---------
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
                this.localAddressIPv4 = this.localAddressIPv4.RemoveSpaces();

                if (this.localAddressIPv4.IsIPAddressAssignedToAnUpInterface())
                {
                    logger.Warning($"Local IPv4 Address \"{localAddressIPv4}\" does not exist, using \"0.0.0.0\" instead");
                    this.localAddressIPv4 = "0.0.0.0";
                }
            }

            localEndPoint = new IPEndPoint(IPAddress.Parse(localAddressIPv4), localPort);

            // -------------
            // Creating socket
            // ---------
            switch (this.serverType)
            {
                case SimpleServerType.TCP:

                    try
                    {
                        localSocket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
                        {
                            Blocking = false
                        };
                    }
                    catch (Exception e)
                    {
                        localSocket = null;
                        logger.Error($"TCP Socket creation error: {e.Message}");
                    }

                    break;

                case SimpleServerType.UDP:

                    try
                    {
                        localSocket = new(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp)
                        {
                            Blocking = false
                        };
                    }
                    catch (Exception e)
                    {
                        localSocket = null;
                        logger.Error($"UDP Socket creation error: {e.Message}");
                    }

                    break;
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

        protected void SetServerStatus(SimpleServerStatus newStatus)
        {
            if (serverStatus != newStatus)
            {
                serverStatus = newStatus;
                ServerStatusChanged?.Invoke(this, serverStatus);
            }
        }

        public abstract void StartServer();
        public abstract void StopServer();
    }
}
