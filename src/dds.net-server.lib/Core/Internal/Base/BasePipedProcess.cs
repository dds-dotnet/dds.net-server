using DDS.Net.Server.Core.Internal.Interfaces;
using DDS.Net.Server.Core.Internal.Interfaces.Implementations;

namespace DDS.Net.Server.Core.Internal.Base
{
    internal abstract class BasePipedProcess<T_Command, T_Response> : IDisposable

        where T_Command : struct
        where T_Response : struct
    {
        private static int SLEEP_TIME_MS_WHEN_DONE_NOTHING = 10;

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
                        //- 
                        //- Default thread function
                        //- 
                        _thread = new Thread(() =>
                        {
                            DoInit();

                            int workStatus1 = 0;
                            int workStatus2 = 0;
                            int processCommandStatus = 0;
                            int checkInputsStatus = 0;

                            while (_isThreadRunning)
                            {
                                workStatus1 = 0;
                                workStatus2 = 0;
                                processCommandStatus = 0;
                                checkInputsStatus = 0;

                                if (_isThreadRunning) { workStatus1 = DoWork(); }
                                if (_isThreadRunning && CommandQueue.CanDequeue()) { processCommandStatus = ProcessCommand(CommandQueue.Dequeue()); }
                                if (_isThreadRunning) { workStatus2 = DoWork(); } 
                                if (_isThreadRunning) { checkInputsStatus = CheckInputs(); }

                                if (_isThreadRunning &&
                                    workStatus1 == 0 &&
                                    workStatus2 == 0 &&
                                    processCommandStatus == 0 &&
                                    checkInputsStatus == 0)
                                {
                                    Thread.Sleep(SLEEP_TIME_MS_WHEN_DONE_NOTHING);
                                }
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

        protected abstract int ProcessCommand(T_Command command);
        protected abstract int CheckInputs();
        protected abstract int DoInit();
        protected abstract int DoWork();
        protected abstract int DoCleanup();

        public abstract void Dispose();
    }
}
