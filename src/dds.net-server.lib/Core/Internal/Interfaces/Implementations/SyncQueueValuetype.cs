using DDS.Net.Server.Core.Internal.Interfaces;

namespace DDS.Net.Server.Core.Internal.Interfaces.Implementations
{
    internal class SyncQueueValuetype<T>
        : ISyncDataWriterQueueEnd<T>, ISyncDataReaderQueueEnd<T>, IDisposable
        where T : struct
    {
        public event EventHandler<T>? DataAvailableForReading;

        private readonly int SLEEP_TIME_MS_WHEN_DATA_CANNOT_BE_DEQUEUED = 5;
        private readonly int SLEEP_TIME_MS_WHEN_DATA_CANNOT_BE_ENQUEUED = 5;

        private Mutex _mutex;

        private T[] _queue;
        private bool[] _queueElementPresent;
        private int _nextWriteIndex;
        private int _nextReadIndex;

        public SyncQueueValuetype(int queueSize)
        {
            if (queueSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(queueSize));
            }

            _queue = new T[queueSize];
            _queueElementPresent = new bool[queueSize];

            for (int i = 0; i < queueSize; i++)
            {
                _queue[i] = default;
                _queueElementPresent[i] = false;
            }

            _nextWriteIndex = 0;
            _nextReadIndex = 0;

            _mutex = new Mutex(false);
        }

        public bool CanDequeue()
        {
            lock (_mutex)
            {
                return _queueElementPresent[_nextReadIndex];
            }
        }

        public bool CanEnqueue()
        {
            lock (_mutex)
            {
                return _queueElementPresent[_nextWriteIndex] == false;
            }
        }

        public T Dequeue()
        {
            while (!CanDequeue()) Thread.Sleep(SLEEP_TIME_MS_WHEN_DATA_CANNOT_BE_DEQUEUED);

            lock (_mutex)
            {
                T data = _queue[_nextReadIndex];
                _queueElementPresent[_nextReadIndex] = false;

                _nextReadIndex++;
                if (_nextReadIndex == _queue.Length)
                    _nextReadIndex = 0;

                return data;
            }
        }

        public void Enqueue(T data)
        {
            while (!CanEnqueue()) Thread.Sleep(SLEEP_TIME_MS_WHEN_DATA_CANNOT_BE_ENQUEUED);

            lock (_mutex)
            {
                _queue[_nextWriteIndex] = data;
                _queueElementPresent[_nextWriteIndex] = true;

                _nextWriteIndex++;
                if (_nextWriteIndex == _queue.Length)
                    _nextWriteIndex = 0;
            }

            DataAvailableForReading?.Invoke(this, data);
        }

        public void Dispose()
        {

        }
    }
}
