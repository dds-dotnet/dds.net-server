using DDS.Net.Server.Core.Internal.Interfaces;

namespace DDS.Net.Server.Core.Internal.InterfaceImplementations
{
    internal class NonBlockingQueue<T>
        : INonBlockingDataOutputQueueEnd<T>, INonBlockingDataInputQueueEnd<T>
    {
        public event EventHandler<T>? InputDataAvailable;

        private T[] _queue;
        private readonly int _size;
        private int _nextWriteIndex;
        private int _nextReadIndex;

        public NonBlockingQueue(int queueSize)
        {
            if (queueSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(queueSize));
            }

            _size = queueSize;

            _queue = new T[_size];

            _nextWriteIndex = 0;
            _nextReadIndex = 0;
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
