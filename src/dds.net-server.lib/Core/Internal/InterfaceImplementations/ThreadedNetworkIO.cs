using DDS.Net.Server.Core.Internal.Entities;
using DDS.Net.Server.Core.Internal.Interfaces;
using DDS.Net.Server.Core.Internal.SimpleServer;
using DDS.Net.Server.Core.Internal.SimpleServer.Types;
using DDS.Net.Server.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDS.Net.Server.Core.Internal.InterfaceImplementations
{
    internal class ThreadedNetworkIO : IThreadedDataIO<DataToClient, DataFromClient>
    {
        private readonly ISyncDataInputQueueEnd<DataToClient> inputQueue;
        private readonly ISyncDataOutputQueueEnd<DataFromClient> outputQueue;

        private readonly ILogger logger;

        private readonly string listeningIPv4Address;

        private readonly bool tcpEnable;
        private readonly ushort tcpPort;
        private readonly int tcpMaxClients;

        private readonly bool udpEnable;
        private readonly ushort udpPort;
        private readonly int udpMaxClients;

        private ThreadedDataIOStatus threadedDataIOStatus;
        public event EventHandler<ThreadedDataIOStatus>? ThreadedDataIOStatusChanged;

        private volatile bool isThreadRunning;
        private Thread thread;

        public ThreadedNetworkIO(
            ISyncDataInputQueueEnd<DataToClient> inputQueue,
            ISyncDataOutputQueueEnd<DataFromClient> outputQueue,

            ILogger logger,

            string listeningIPv4Address,
            bool tcpEnable, ushort tcpPort, int tcpMaxClients,
            bool udpEnable, ushort udpPort, int udpMaxClients)
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
            this.udpMaxClients = udpMaxClients;

            this._tcpInputQueue = null!;
            this._udpInputQueue = null!;
            this._tcpOutputQueue = null!;
            this._udpOutputQueue = null!;

            this._tcpServer = null!;
            this._udpServer = null!;

            this.isThreadRunning = false;
            this.thread = null!;
        }

        public ISyncDataInputQueueEnd<DataToClient> GetInputDataQueueEnd()
        {
            return inputQueue;
        }

        public ISyncDataOutputQueueEnd<DataFromClient> GetOutputDataQueueEnd()
        {
            return outputQueue;
        }

        public void SetInputDataQueueEnd(ISyncDataInputQueueEnd<DataToClient> inputQueueEnd)
        {
            throw new Exception("The input queue cannot be updated once initialized through constructor");
        }

        public void SetOutputDataQueueEnd(ISyncDataOutputQueueEnd<DataFromClient> outputQueueEnd)
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
                        udpMaxClients,
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

                        }

                        //- Data to TCP Server
                        while (inputQueue.CanDequeue() && _tcpOutputQueue.CanEnqueue())
                        {

                        }
                    }

                    if (_udpServer != null)
                    {
                        //- Data from UDP Server
                        while (_udpInputQueue.CanDequeue() && outputQueue.CanEnqueue())
                        {

                        }

                        //- Data to UDP Server
                        while (inputQueue.CanDequeue() && _udpOutputQueue.CanEnqueue())
                        {

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
