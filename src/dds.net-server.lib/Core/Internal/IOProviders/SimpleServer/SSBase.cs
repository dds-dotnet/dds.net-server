using DDS.Net.Server.Core.Internal.Extensions;
using DDS.Net.Server.Core.Internal.Interfaces;
using DDS.Net.Server.Core.Internal.IOProviders.SimpleServer.Types;
using DDS.Net.Server.Interfaces;
using System.Net;
using System.Net.Sockets;

namespace DDS.Net.Server.Core.Internal.SimpleServer
{
    internal abstract partial class SSBase
    {
        protected volatile SSStatus serverStatus = SSStatus.Stopped;

        protected readonly ISyncDataReaderQueueEnd<SSPacket> dataInputQueue;
        protected readonly ISyncDataWriterQueueEnd<SSPacket> dataOutputQueue;

        protected readonly string localAddressIPv4;
        protected readonly ushort localPort;

        protected readonly IPEndPoint localEndPoint;
        protected readonly Socket localSocket;

        private readonly SSType serverType;

        protected readonly int maxNumberOfClients;
        protected readonly ILogger logger;

        protected SSBase(
            ISyncDataReaderQueueEnd<SSPacket> dataInputQueue,
            ISyncDataWriterQueueEnd<SSPacket> dataOutputQueue,

            string localAddressIPv4,
            ushort localPort,
            int maxNumberOfClients,
            SSType serverType,

            ILogger logger)
        {
            SetServerStatus(SSStatus.Stopped);

            this.dataInputQueue = dataInputQueue;
            this.dataOutputQueue = dataOutputQueue;

            this.localAddressIPv4 = localAddressIPv4;
            this.localPort = localPort;
            this.maxNumberOfClients = maxNumberOfClients;
            this.serverType = serverType;

            this.logger = logger;

            this.localSocket = null!;

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

            if (this.localAddressIPv4 == "0.0.0.0")
            {
                localEndPoint = new IPEndPoint(IPAddress.Any, localPort);
            }
            else
            {
                localEndPoint = new IPEndPoint(IPAddress.Parse(this.localAddressIPv4), localPort);
            }

            // -------------
            // Creating socket
            // ---------
            switch (this.serverType)
            {
                case SSType.TCP:

                    try
                    {
                        localSocket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
                        {
                            Blocking = false
                        };
                    }
                    catch (Exception e)
                    {
                        localSocket = null!;
                        logger.Error($"TCP Socket creation error: {e.Message}");
                    }

                    break;

                case SSType.UDP:

                    try
                    {
                        localSocket = new(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp)
                        {
                            Blocking = false
                        };
                    }
                    catch (Exception e)
                    {
                        localSocket = null!;
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

        protected void SetServerStatus(SSStatus newStatus)
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
