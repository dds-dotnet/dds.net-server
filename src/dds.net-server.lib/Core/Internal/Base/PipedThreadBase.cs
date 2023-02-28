using DDS.Net.Server.Core.Internal.Interfaces;
using DDS.Net.Server.Core.Internal.Interfaces.Implementations;

namespace DDS.Net.Server.Core.Internal.Base
{
    internal abstract class PipedThreadBase<T_Commands, T_Responses> : IDisposable

        where T_Commands : struct
        where T_Responses : struct
    {
        public ISyncQueueWriterEnd<T_Commands> Commands { get; private set; }
        public ISyncQueueReaderEnd<T_Responses> Responses { get; private set; }

        protected readonly SyncQueueValuetype<T_Commands> commandsQueue;
        protected readonly SyncQueueValuetype<T_Responses> responsesQueue;

        protected PipedThreadBase(int commandsQueueSize, int responsesQueueSize, bool startThread = true)
        {
            commandsQueue = new SyncQueueValuetype<T_Commands>(commandsQueueSize);
            responsesQueue = new SyncQueueValuetype<T_Responses>(responsesQueueSize);

            Commands = commandsQueue;
            Responses = responsesQueue;

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
                                if (_isThreadRunning && commandsQueue.CanDequeue()) ProcessCommand(commandsQueue.Dequeue());
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

        protected abstract void ProcessCommand(T_Commands command);
        protected abstract void CheckInputs();
        protected abstract void DoInit();
        protected abstract void DoWork();
        protected abstract void DoCleanup();

        public abstract void Dispose();
    }
}
