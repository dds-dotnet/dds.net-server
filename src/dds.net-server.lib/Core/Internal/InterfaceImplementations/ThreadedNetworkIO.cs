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
        public event EventHandler<ThreadedDataIOStatus>? ThreadedDataIOStatusChanged;

        public ThreadedNetworkIO(
            INonBlockingDataInputQueueEnd<DataToClient> inputQueue,
            INonBlockingDataOutputQueueEnd<DataFromClient> outputQueue,
            ILogger logger,

            string listeningIPv4Address,
            bool tcpEnable, ushort tcpPort, int tcpMaxClients,
            bool udpEnable, ushort udpPort, int udpMaxClients)
        {
            
        }

        public INonBlockingDataInputQueueEnd<DataToClient> GetInputDataQueueEnd()
        {
            throw new NotImplementedException();
        }

        public INonBlockingDataOutputQueueEnd<DataFromClient> GetOutputDataQueueEnd()
        {
            throw new NotImplementedException();
        }

        public void SetInputDataQueueEnd(INonBlockingDataInputQueueEnd<DataToClient> inputQueueEnd)
        {
            throw new NotImplementedException();
        }

        public void SetOutputDataQueueEnd(INonBlockingDataOutputQueueEnd<DataFromClient> outputQueueEnd)
        {
            throw new NotImplementedException();
        }
    }
}
