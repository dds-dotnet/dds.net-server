using DDS.Net.Server.Core.Internal.SimpleServer.Types;
using DDS.Net.Server.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDS.Net.Server.Core.Internal.SimpleServer
{
    internal class UdpServer : BaseServer
    {
        private volatile ServerStatus status;
        private volatile bool isClientListenerThreadRunning = false;
        private Thread? clientListenerThread = null;

        public UdpServer(string IPv4, ushort port, int maxClients, ILogger logger)
            : base(IPv4, port, maxClients, SimpleServerType.UDP, logger)
        {
            status = ServerStatus.Stopped;
        }

        public override void StartServer()
        {
            lock (this)
            {
                if (clientListenerThread == null &&
                    localSocket != null)
                {
                    isClientListenerThreadRunning = true;
                    clientListenerThread = new Thread(ClientListenerThreadFunction);
                    clientListenerThread.IsBackground = true;
                    clientListenerThread.Start();
                }
            }
        }

        public override void StopServer()
        {
            lock (this)
            {
                if (clientListenerThread != null)
                {
                    isClientListenerThreadRunning = false;
                    clientListenerThread.Join();
                }
            }
        }

        private void ClientListenerThreadFunction()
        {
            while (isClientListenerThreadRunning)
            {

            }

            isClientListenerThreadRunning = false;
            clientListenerThread = null;

            status = ServerStatus.Stopped;
        }
    }
}
