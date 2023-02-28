using DDS.Net.Server.Core.Internal.InterfaceImplementations;
using DDS.Net.Server.Core.Internal.Interfaces;

namespace DDS.Net.Server.Core.Internal.Base
{
    internal abstract class PipedThreadBase<T_Commands, T_Responses> : IDisposable

        where T_Commands : struct
        where T_Responses : struct
    {
        public ISyncDataInputQueueEnd<T_Commands> Commands { get; private set; }
        public ISyncDataOutputQueueEnd<T_Responses> Responses { get; private set; }

        protected readonly SyncQueueStruct<T_Commands> commandsQueue;
        protected readonly SyncQueueStruct<T_Responses> responsesQueue;

        protected PipedThreadBase(int commandsQueueSize, int responsesQueueSize, bool startThread = true)
        {
            commandsQueue = new SyncQueueStruct<T_Commands>(commandsQueueSize);
            responsesQueue = new SyncQueueStruct<T_Responses>(responsesQueueSize);

            Commands = commandsQueue;
            Responses = responsesQueue;

            if (startThread)
            {
                StartThread();
            }
        }

        private volatile bool _isThreadRunning = false;
        private Thread _thread;

        protected void StartThread(Action? threadFunction = null)
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
                            while (_isThreadRunning)
                            {
                                if (_isThreadRunning) DoWork();
                                if (_isThreadRunning) CheckCommands();
                                if (_isThreadRunning) DoWork();
                                if (_isThreadRunning) CheckInputs();
                                if (_isThreadRunning) DoWork();
                                if (_isThreadRunning) GenerateOutputs();

                                Thread.Yield();
                            }
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
        protected abstract void DoWork();

        public abstract void Dispose();
    }
}
