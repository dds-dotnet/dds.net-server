using DDS.Net.Server.Core.Internal.Entities;
using DDS.Net.Server.Core.Internal.Interfaces;
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

        public event EventHandler<ThreadedDataIOStatus>? ThreadedDataIOStatusChanged;

        private bool isThreadRunning;
        private Thread thread;

        public ThreadedNetworkIO(
            ISyncDataInputQueueEnd<DataToClient> inputQueue,
            ISyncDataOutputQueueEnd<DataFromClient> outputQueue,

            ILogger logger,

            string listeningIPv4Address,
            bool tcpEnable, ushort tcpPort, int tcpMaxClients,
            bool udpEnable, ushort udpPort, int udpMaxClients)
        {
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

        private void ThreadFunction()
        {
            throw new NotImplementedException();
        }
    }
}
