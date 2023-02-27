using DDS.Net.Server.Core.Internal.Entities;
using DDS.Net.Server.Core.Internal.Interfaces;
using DDS.Net.Server.Core.Internal.SimpleServer;
using DDS.Net.Server.Core.Internal.SimpleServer.Types;
using DDS.Net.Server.Interfaces;
using System.Net;

namespace DDS.Net.Server.Core.Internal.InterfaceImplementations
{
    internal class ThreadedNetworkIO : IThreadedDataIO<DataToClient, DataFromClient>
    {
        private readonly ISyncDataOutputQueueEnd<DataToClient> inputQueue;
        private readonly ISyncDataInputQueueEnd<DataFromClient> outputQueue;

        private readonly ILogger logger;

        private readonly string listeningIPv4Address;

        private readonly bool tcpEnable;
        private readonly ushort tcpPort;
        private readonly int tcpMaxClients;

        private readonly bool udpEnable;
        private readonly ushort udpPort;

        private ThreadedDataIOStatus threadedDataIOStatus;
        public event EventHandler<ThreadedDataIOStatus>? ThreadedDataIOStatusChanged;

        private volatile bool isThreadRunning;
        private Thread thread;

        public ThreadedNetworkIO(
            ISyncDataOutputQueueEnd<DataToClient> inputQueue,
            ISyncDataInputQueueEnd<DataFromClient> outputQueue,

            ILogger logger,

            string listeningIPv4Address,
            bool tcpEnable, ushort tcpPort, int tcpMaxClients,
            bool udpEnable, ushort udpPort)
        {
            this.threadedDataIOStatus = ThreadedDataIOStatus.Stopped;

            this.inputQueue = inputQueue ?? throw new ArgumentNullException(nameof(inputQueue));
            this.outputQueue = outputQueue ?? throw new ArgumentNullException(nameof(outputQueue));

            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

            this.listeningIPv4Address = listeningIPv4Address ?? throw new ArgumentNullException(nameof(listeningIPv4Address));

            this.tcpEnable = tcpEnable;
            this.tcpPort = tcpPort;
            this.tcpMaxClients = tcpMaxClients;

            this.udpEnable = udpEnable;
            this.udpPort = udpPort;

            this._tcpInputQueue = null!;
            this._udpInputQueue = null!;
            this._tcpOutputQueue = null!;
            this._udpOutputQueue = null!;

            this._tcpServer = null!;
            this._udpServer = null!;

            this.isThreadRunning = false;
            this.thread = null!;
        }

        public ISyncDataOutputQueueEnd<DataToClient> GetInputDataQueueEnd()
        {
            return inputQueue;
        }

        public ISyncDataInputQueueEnd<DataFromClient> GetOutputDataQueueEnd()
        {
            return outputQueue;
        }

        public void SetInputDataQueueEnd(ISyncDataOutputQueueEnd<DataToClient> inputQueueEnd)
        {
            throw new Exception("The input queue cannot be updated once initialized through constructor");
        }

        public void SetOutputDataQueueEnd(ISyncDataInputQueueEnd<DataFromClient> outputQueueEnd)
        {
            throw new Exception("The output queue cannot be updated once initialized through constructor");
        }

        public void StartIO()
        {
            lock (this)
            {
                if (thread == null)
                {
                    isThreadRunning = true;

                    thread = new Thread(ThreadFunction);

                    thread.Priority = ThreadPriority.Normal;
                    thread.IsBackground = true;

                    thread.Start();
                }
            }
        }

        public void StopIO()
        {
            lock (this)
            {
                if (thread != null)
                {
                    isThreadRunning = false;

                    thread.Join();
                    thread = null!;
                }
            }
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

        private void ThreadFunction()
        {
            UpdateStatus(ThreadedDataIOStatus.Starting);

            StartServers();

            if (_tcpServer != null || _udpServer != null)
            {
                UpdateStatus(ThreadedDataIOStatus.Started);

                while (isThreadRunning)
                {
                    if (_tcpServer != null)
                    {
                        //- Data from TCP Server
                        while (_tcpInputQueue.CanDequeue() && outputQueue.CanEnqueue())
                        {
                            SSPacket packet = _tcpInputQueue.Dequeue();
                            outputQueue.Enqueue(new DataFromClient($"TCP:{packet.ClientInfo}", packet.PacketData));
                        }

                        //- Data to TCP Server
                        while (inputQueue.CanDequeue() && _tcpOutputQueue.CanEnqueue())
                        {
                            DataToClient packet = inputQueue.Dequeue();
                            IPEndPoint target = IPEndPoint.Parse(packet.ClientRef.Replace("TCP:", ""));
                            _tcpOutputQueue.Enqueue(new SSPacket(target, packet.Data));
                        }
                    }

                    if (_udpServer != null)
                    {
                        //- Data from UDP Server
                        while (_udpInputQueue.CanDequeue() && outputQueue.CanEnqueue())
                        {
                            SSPacket packet = _udpInputQueue.Dequeue();
                            outputQueue.Enqueue(new DataFromClient($"UDP:{packet.ClientInfo}", packet.PacketData));
                        }

                        //- Data to UDP Server
                        while (inputQueue.CanDequeue() && _udpOutputQueue.CanEnqueue())
                        {
                            DataToClient packet = inputQueue.Dequeue();
                            IPEndPoint target = IPEndPoint.Parse(packet.ClientRef.Replace("UDP:", ""));
                            _udpOutputQueue.Enqueue(new SSPacket(target, packet.Data));
                        }
                    }

                    Thread.Yield();
                }
            }
            else
            {
                logger.Error("Unable to start network I/O");
                UpdateStatus(ThreadedDataIOStatus.Stopped);
                thread = null!;
            }

            _tcpServer = null!;
            _udpServer = null!;

            _tcpInputQueue = null!;
            _tcpOutputQueue = null!;

            _udpInputQueue = null!;
            _udpOutputQueue = null!;
        }

        private void UpdateStatus(ThreadedDataIOStatus newStatus)
        {
            if (threadedDataIOStatus != newStatus)
            {
                threadedDataIOStatus = newStatus;
                ThreadedDataIOStatusChanged?.Invoke(this, newStatus);
            }
        }
    }
}
