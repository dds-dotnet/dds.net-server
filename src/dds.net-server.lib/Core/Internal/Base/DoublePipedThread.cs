using DDS.Net.Server.Core.Internal.InterfaceImplementations;
using DDS.Net.Server.Core.Internal.Interfaces;

namespace DDS.Net.Server.Core.Internal.Base
{
    internal abstract class DoublePipedThread<T_Input1, T_Output1, T_Input2, T_Output2, T_Commands, T_Responses>
        : SinglePipedThread<T_Input1, T_Output1, T_Commands, T_Responses>, IDisposable

        where T_Input1 : class
        where T_Input2 : class
        where T_Output1 : class
        where T_Output2 : class
        where T_Commands : struct
        where T_Responses : struct
    {
        public ISyncDataWriterQueueEnd<T_Input2> Input2 { get; private set; }
        public ISyncDataReaderQueueEnd<T_Output2> Output2 { get; private set; }

        protected readonly SyncQueue<T_Input2> inputQueue2;
        protected readonly SyncQueue<T_Output2> outputQueue2;

        protected DoublePipedThread(int inputQueue1Size, int outputQueue1Size,
                                    int inputQueue2Size, int outputQueue2Size,
                                    int commandsQueueSize, int responsesQueueSize,
                                    bool startThread = true)

            : base(inputQueue1Size, outputQueue1Size, commandsQueueSize, responsesQueueSize, false)
        {
            inputQueue2 = new SyncQueue<T_Input2>(inputQueue2Size);
            outputQueue2 = new SyncQueue<T_Output2>(outputQueue2Size);

            Input2 = inputQueue2;
            Output2 = outputQueue2;

            if (startThread)
            {
                StartThread();
            }
        }

        protected override void StartThread(Action? threadFunction = null)
        {
            lock (this)
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
                                if (_isThreadRunning && (inputQueue.CanDequeue() || inputQueue2.CanDequeue())) CheckInputs();
                                if (_isThreadRunning) DoWork();
                                if (_isThreadRunning && (outputQueue.CanEnqueue() || outputQueue2.CanEnqueue())) GenerateOutputs();

                                Thread.Yield();
                            }

                            DoCleanup();
                        });
                    }

                    _thread.Start();
                }
            }
        }
    }
}
