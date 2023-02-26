using DDS.Net.Server.Core.Internal.Interfaces;

namespace DDS.Net.Server.Core.Internal.InterfaceImplementations
{
    internal class NonBlockingQueue<T> : INonBlockingDataOutputQueueEnd<T>, INonBlockingDataInputQueueEnd<T>
    {
        public event EventHandler<T>? InputDataAvailable;

        public bool CanDequeueData()
        {
            throw new NotImplementedException();
        }

        public bool CanEnqueueData()
        {
            throw new NotImplementedException();
        }

        public T DequeueData()
        {
            throw new NotImplementedException();
        }

        public void EnqueueData(T data)
        {
            throw new NotImplementedException();
        }
    }
}
