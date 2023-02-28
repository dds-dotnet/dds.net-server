using DDS.Net.Server.Core.Internal.Interfaces;
using DDS.Net.Server.Core.Internal.Interfaces.Implementations;

namespace DDS.Net.Server.Core.Internal.Base
{
    internal abstract class BasePipedProcess<T_Command, T_Response> : IDisposable

        where T_Command : struct
        where T_Response : struct
    {
        public ISyncQueueWriterEnd<T_Command> CommandWriter { get; private set; }
        public ISyncQueueReaderEnd<T_Response> ResponseReader { get; private set; }

        protected readonly SyncQueueValuetype<T_Command> CommandQueue;
        protected readonly SyncQueueValuetype<T_Response> ResponseQueue;

        protected BasePipedProcess(int commandsQueueSize, int responsesQueueSize, bool startThread = true)
        {
            CommandQueue = new SyncQueueValuetype<T_Command>(commandsQueueSize);
            ResponseQueue = new SyncQueueValuetype<T_Response>(responsesQueueSize);

            CommandWriter = CommandQueue;
            ResponseReader = ResponseQueue;

            if (startThread)
            {
                StartThread();
            }
        }

        protected volatile bool _isThreadRunning = false;
        protected Thread _thread = null!;

        protected virtual void StartThread(Action? threadFunction = null)
        {
            lock(this)
            {
                if (_thread == null)
                {
                    _isThreadRunning = true;

                    if (threadFunction != null)
                    {
                        _thread = new Thread(threadFunction.Invoke);
                    }
                    else
                    {
                        _thread = new Thread(() =>
                        {
                            DoInit();

                            while (_isThreadRunning)
                            {
                                if (_isThreadRunning) DoWork();
                                if (_isThreadRunning && CommandQueue.CanDequeue()) ProcessCommand(CommandQueue.Dequeue());
                                if (_isThreadRunning) DoWork();
                                if (_isThreadRunning) CheckInputs();

                                Thread.Yield();
                            }

                            DoCleanup();
                        });
                    }

                    _thread.Start();
                }
            }
        }

        protected void Exit()
        {
            lock (this)
            {
                if (_thread != null)
                {
                    _isThreadRunning = false;
                    _thread = null!;
                }
            }
        }

        protected abstract void ProcessCommand(T_Command command);
        protected abstract void CheckInputs();
        protected abstract void DoInit();
        protected abstract void DoWork();
        protected abstract void DoCleanup();

        public abstract void Dispose();
    }
}
