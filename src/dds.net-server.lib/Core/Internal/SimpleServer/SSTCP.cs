using DDS.Net.Server.Core.Internal.Interfaces;
using DDS.Net.Server.Core.Internal.SimpleServer.Types;
using DDS.Net.Server.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DDS.Net.Server.Core.Internal.SimpleServer
{
    internal class SSTCP : SSBase
    {
        private volatile bool isConnectionListenerThreadRunning = false;
        private Thread? connectionListenerThread = null;

        public SSTCP(
            ISyncDataInputQueueEnd<SSPacket> dataInputQueue,
            ISyncDataOutputQueueEnd<SSPacket> dataOutputQueue,
            
            string IPv4, ushort port, int maxClients, ILogger logger)

            : base(dataInputQueue, dataOutputQueue,
                   IPv4, port, maxClients, SSType.TCP, logger)
        {
        }

        public override void StartServer()
        {
            lock (this)
            {
                if (connectionListenerThread == null &&
                    localSocket != null)
                {
                    isConnectionListenerThreadRunning = true;
                    connectionListenerThread = new Thread(ConnectionListenerThread);
                    connectionListenerThread.IsBackground = true;
                    connectionListenerThread.Start();
                }
            }
        }

        public override void StopServer()
        {
            lock (this)
            {
                if (connectionListenerThread != null)
                {
                    isConnectionListenerThreadRunning = false;
                    connectionListenerThread.Join();
                }
            }
        }

        public override SSPacketSendingStatus SendPacket(SSPacket packet)
        {
            throw new NotImplementedException();
        }

        private void ConnectionListenerThread()
        {
            bool bindingOk = false;

            if (localSocket != null)
            {
                try
                {
                    localSocket.Bind(localEndPoint);

                    logger.Info($"TCP socket bound @{localEndPoint}");

                    bindingOk = true;
                }
                catch (Exception ex)
                {
                    logger.Error($"TCP socket binding error @{localEndPoint}: {ex.Message}");
                }
            }

            if (bindingOk)
            {
                SetServerStatus(SSStatus.Running);

                while (isConnectionListenerThreadRunning)
                {

                }
            }

            isConnectionListenerThreadRunning = false;
            connectionListenerThread = null;

            SetServerStatus(SSStatus.Stopped);
        }
    }
}
