using DDS.Net.Server.Core.Internal.Interfaces;

namespace DDS.Net.Server.Core.Internal.InterfaceImplementations
{
    internal class NonBlockingQueue<T>
        : INonBlockingDataOutputQueueEnd<T>, INonBlockingDataInputQueueEnd<T>, IDisposable
        where T : class
    {
        public event EventHandler<T>? InputDataAvailable;

        private Mutex _mutex;

        private T[] _queue;
        private int _nextWriteIndex;
        private int _nextReadIndex;

        public NonBlockingQueue(int queueSize)
        {
            if (queueSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(queueSize));
            }

            _queue = new T[queueSize];

            _nextWriteIndex = 0;
            _nextReadIndex = 0;

            _mutex = new Mutex(false);
        }

        public bool CanDequeueData()
        {
            lock (_mutex)
            {
                if (_queue[_nextReadIndex] != null)
                    return true;

                return false;
            }
        }

        public bool CanEnqueueData()
        {
            lock (_mutex)
            {
                if (_queue[_nextWriteIndex] == null)
                    return true;

                return false;
            }
        }

        public T DequeueData()
        {
            throw new NotImplementedException();
        }

        public void EnqueueData(T data)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
