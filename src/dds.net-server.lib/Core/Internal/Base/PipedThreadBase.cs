using DDS.Net.Server.Core.Internal.InterfaceImplementations;
using DDS.Net.Server.Core.Internal.Interfaces;

namespace DDS.Net.Server.Core.Internal.Base
{
    internal abstract class PipedThreadBase<T_Commands, T_Responses> : IDisposable

        where T_Commands : struct
        where T_Responses : struct
    {
        public ISyncDataWriterQueueEnd<T_Commands> Commands { get; private set; }
        public ISyncDataReaderQueueEnd<T_Responses> Responses { get; private set; }

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
                                if (_isThreadRunning && commandsQueue.CanDequeue()) CheckCommands();
                                if (_isThreadRunning) DoWork();
                                if (_isThreadRunning) CheckInputs();
                                if (_isThreadRunning) DoWork();
                                if (_isThreadRunning) GenerateOutputs();

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

        protected abstract void CheckCommands();
        protected abstract void CheckInputs();
        protected abstract void GenerateOutputs();
        protected abstract void DoInit();
        protected abstract void DoWork();
        protected abstract void DoCleanup();

        public abstract void Dispose();
    }
}
