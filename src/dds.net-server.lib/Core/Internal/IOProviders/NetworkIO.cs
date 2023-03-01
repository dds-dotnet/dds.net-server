using DDS.Net.Server.Core.Internal.Base;
using DDS.Net.Server.Core.Internal.Base.Entities;
using DDS.Net.Server.Core.Internal.Interfaces.Implementations;
using DDS.Net.Server.Core.Internal.IOProviders.SimpleServer;
using DDS.Net.Server.Core.Internal.IOProviders.SimpleServer.Types;
using DDS.Net.Server.Core.Internal.SimpleServer;
using DDS.Net.Server.Interfaces;
using System.Net;

namespace DDS.Net.Server.Core.Internal.IOProviders
{
    internal class NetworkIO
        : SinglePipedProducer<DataToClient, DataFromClient, DataIOProviderCommand, DataIOProviderStatus>
    {
        private DataIOProviderStatus threadedDataIOStatus;

        private readonly ILogger logger;

        private readonly string listeningIPv4Address;

        private readonly bool tcpEnable;
        private readonly ushort tcpPort;
        private readonly int tcpMaxClients;

        private readonly bool udpEnable;
        private readonly ushort udpPort;

        public NetworkIO(
            ILogger logger,

            int inputQueueSize,
            int outputQueueSize,
            int commandsQueueSize,
            int responsesQueueSize,

            string listeningIPv4Address,
            bool tcpEnable, ushort tcpPort, int tcpMaxClients,
            bool udpEnable, ushort udpPort)

            : base(inputQueueSize, outputQueueSize, commandsQueueSize, responsesQueueSize, false)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

            this.listeningIPv4Address = listeningIPv4Address ?? throw new ArgumentNullException(nameof(listeningIPv4Address));

            threadedDataIOStatus = DataIOProviderStatus.Stopped;

            if (ResponseQueue.CanEnqueue())
            {
                ResponseQueue.Enqueue(threadedDataIOStatus);
            }
            else
            {
                logger.Error("NetworkIO response queue full");
            }

            this.tcpEnable = tcpEnable;
            this.tcpPort = tcpPort;
            this.tcpMaxClients = tcpMaxClients;

            this.udpEnable = udpEnable;
            this.udpPort = udpPort;

            _tcpInputQueue = null!;
            _udpInputQueue = null!;
            _tcpOutputQueue = null!;
            _udpOutputQueue = null!;

            _tcpServer = null!;
            _udpServer = null!;
        }

        public void StartIO()
        {
            StartThread();
        }

        public void StopIO()
        {
            Exit();
        }

        private SyncQueue<SSPacket> _tcpInputQueue;
        private SyncQueue<SSPacket> _udpInputQueue;

        private SyncQueue<SSPacket> _tcpOutputQueue;
        private SyncQueue<SSPacket> _udpOutputQueue;

        private SSBase _tcpServer;
        private SSBase _udpServer;

        private void StartServers()
        {
            if (_tcpServer == null && tcpEnable)
            {
                _tcpInputQueue = new SyncQueue<SSPacket>(InternalSettings.TCP_DATA_FROM_CLIENTS_QUEUE_SIZE);
                _tcpOutputQueue = new SyncQueue<SSPacket>(InternalSettings.TCP_DATA_TO_CLIENTS_QUEUE_SIZE);

                try
                {
                    _tcpServer = new SSTCP(
                        _tcpOutputQueue,
                        _tcpInputQueue,
                        listeningIPv4Address,
                        tcpPort,
                        tcpMaxClients,
                        logger);

                    _tcpServer.StartServer();
                }
                catch (Exception ex)
                {
                    _tcpServer = null!;

                    _tcpInputQueue.Dispose();
                    _tcpOutputQueue.Dispose();

                    _tcpInputQueue = null!;
                    _tcpOutputQueue = null!;

                    logger.Error($"Cannot start TCP Server: {ex.Message}");
                }
            }

            if (_udpServer == null && udpEnable)
            {
                _udpInputQueue = new SyncQueue<SSPacket>(InternalSettings.UDP_DATA_FROM_CLIENTS_QUEUE_SIZE);
                _udpOutputQueue = new SyncQueue<SSPacket>(InternalSettings.UDP_DATA_TO_CLIENTS_QUEUE_SIZE);

                try
                {
                    _udpServer = new SSUDP(
                        _udpOutputQueue,
                        _udpInputQueue,
                        listeningIPv4Address,
                        udpPort,
                        logger);

                    _udpServer.StartServer();
                }
                catch (Exception ex)
                {
                    _udpServer = null!;

                    _udpInputQueue.Dispose();
                    _udpOutputQueue.Dispose();

                    _udpInputQueue = null!;
                    _udpOutputQueue = null!;

                    logger.Error($"Cannot start UDP Server: {ex.Message}");
                }
            }
        }

        private void UpdateStatus(DataIOProviderStatus newStatus)
        {
            if (threadedDataIOStatus != newStatus)
            {
                threadedDataIOStatus = newStatus;

                if (ResponseQueue.CanEnqueue())
                {
                    ResponseQueue.Enqueue(newStatus);
                }
                else
                {
                    logger.Error("NetworkIO response queue full");
                }
            }
        }

        protected override int DoInit()
        {
            UpdateStatus(DataIOProviderStatus.Starting);

            StartServers();

            if (_tcpServer != null || _udpServer != null)
            {
                UpdateStatus(DataIOProviderStatus.Started);
            }
            else
            {
                logger.Error("Unable to start network I/O");
                UpdateStatus(DataIOProviderStatus.Stopped);
            }

            return 0;
        }

        protected override int DoWork()
        {
            int workDone = 0;

            if (_tcpServer != null)
            {
                //- Data from TCP Server
                while (_tcpInputQueue.CanDequeue() && OutputQueue.CanEnqueue())
                {
                    SSPacket packet = _tcpInputQueue.Dequeue();
                    OutputQueue.Enqueue(new DataFromClient($"TCP:{packet.ClientInfo}", packet.PacketData));

                    workDone++;
                }
            }

            if (_udpServer != null)
            {
                //- Data from UDP Server
                while (_udpInputQueue.CanDequeue() && OutputQueue.CanEnqueue())
                {
                    SSPacket packet = _udpInputQueue.Dequeue();
                    OutputQueue.Enqueue(new DataFromClient($"UDP:{packet.ClientInfo}", packet.PacketData));

                    workDone++;
                }
            }

            return workDone;
        }

        protected override int DoCleanup()
        {
            _tcpServer?.StopServer();
            _udpServer?.StopServer();

            _tcpServer = null!;
            _udpServer = null!;

            _tcpInputQueue = null!;
            _tcpOutputQueue = null!;

            _udpInputQueue = null!;
            _udpOutputQueue = null!;

            return 0;
        }

        protected override int ProcessInput(DataToClient input)
        {
            string targetRef = input.ClientRef;

            if (targetRef.StartsWith("TCP:") && _tcpServer != null)
            {
                IPEndPoint target = IPEndPoint.Parse(targetRef.Replace("TCP:", ""));
                _tcpOutputQueue.Enqueue(new SSPacket(target, input.Data));
            }
            else if (targetRef.StartsWith("UDP:") && _udpServer != null)
            {
                IPEndPoint target = IPEndPoint.Parse(targetRef.Replace("UDP:", ""));
                _udpOutputQueue.Enqueue(new SSPacket(target, input.Data));
            }
            else
            {
                logger.Error($"NetworkIO sending to network failed for {targetRef}");
            }

            return 1;
        }

        protected override int ProcessCommand(DataIOProviderCommand command)
        {
            return 0;
        }

        public override void Dispose()
        {
        }
    }
}
