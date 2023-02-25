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
    internal class TcpServer : BaseServer
    {
        private volatile bool isConnectionListenerThreadRunning = false;
        private Thread? connectionListenerThread = null;

        public TcpServer(string IPv4, ushort port, int maxClients, ILogger logger)
            : base(IPv4, port, maxClients, SimpleServerType.TCP, logger)
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
                    connectionListenerThread = new Thread(ConnectionListenerThreadFunction);
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

        private void ConnectionListenerThreadFunction()
        {
            bool bindingOk = false;

            try
            {
                localSocket?.Bind(localEndPoint);

                logger.Info($"TCP socket bound @{localEndPoint}");

                bindingOk = true;
            }
            catch (Exception ex)
            {
                logger.Error($"TCP socket binding error @{localEndPoint}: {ex.Message}");
            }

            if (bindingOk)
            {
                SetServerStatus(SimpleServerStatus.Running);

                while (isConnectionListenerThreadRunning)
                {

                }
            }

            isConnectionListenerThreadRunning = false;
            connectionListenerThread = null;

            SetServerStatus(SimpleServerStatus.Stopped);
        }
    }
}
