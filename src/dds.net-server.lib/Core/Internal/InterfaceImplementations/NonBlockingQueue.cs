using DDS.Net.Server.Core.Internal.Interfaces;

namespace DDS.Net.Server.Core.Internal.InterfaceImplementations
{
    internal class NonBlockingQueue<T>
        : INonBlockingDataOutputQueueEnd<T>, INonBlockingDataInputQueueEnd<T>
    {
        public event EventHandler<T>? InputDataAvailable;

        private readonly int _size;

        private T[] _queue;
        private int _writeIndex;
        private int _readIndex;

        public NonBlockingQueue(int queueSize)
        {
            if (queueSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(queueSize));
            }

            _size = queueSize;

            _queue = new T[_size];

            _writeIndex = 0;
            _readIndex = 0;
        }

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
