using DDS.Net.Server.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDS.Net.Server.Core.Internal.SimpleServer
{
    internal class TcpServer : BaseServer
    {
        private enum ServerStatus
        {
            Stopped,
            Running
        }

        private volatile ServerStatus status;
        private volatile bool isConnectionListenerThreadRunning = false;
        private Thread? connectionListenerThread = null;

        public TcpServer(string IPv4, ushort port, int maxClients, ILogger logger)
            : base(IPv4, port, maxClients, ServerType.TCP, logger)
        {
            status = ServerStatus.Stopped;
        }

        public override void StartServer()
        {
            lock (this)
            {
                if (connectionListenerThread == null)
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
            while (isConnectionListenerThreadRunning)
            {

            }

            isConnectionListenerThreadRunning = false;
            connectionListenerThread = null;
        }
    }
}
