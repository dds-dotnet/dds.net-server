using DDS.Net.Server.Core.Internal.Interfaces;
using DDS.Net.Server.Core.Internal.Interfaces.Implementations;

namespace DDS.Net.Server.Core.Internal.Base
{
    internal abstract class DualPipedProducer<T_Input1, T_Output1, T_Input2, T_Output2, T_Commands, T_Responses>
        : SinglePipedProducer<T_Input1, T_Output1, T_Commands, T_Responses>, IDisposable

        where T_Input1 : class
        where T_Input2 : class
        where T_Output1 : class
        where T_Output2 : class
        where T_Commands : struct
        where T_Responses : struct
    {
        public ISyncQueueWriterEnd<T_Input2> InputWriter2 { get; private set; }
        public ISyncQueueReaderEnd<T_Output2> OutputReader2 { get; private set; }

        protected readonly SyncQueue<T_Input2> InputQueue2;
        protected readonly SyncQueue<T_Output2> OutputQueue2;

        protected DualPipedProducer(int inputQueue1Size, int outputQueue1Size,
                                    int inputQueue2Size, int outputQueue2Size,
                                    int commandsQueueSize, int responsesQueueSize,
                                    bool startThread = true)

            : base(inputQueue1Size, outputQueue1Size, commandsQueueSize, responsesQueueSize, false)
        {
            InputQueue2 = new SyncQueue<T_Input2>(inputQueue2Size);
            OutputQueue2 = new SyncQueue<T_Output2>(outputQueue2Size);

            InputWriter2 = InputQueue2;
            OutputReader2 = OutputQueue2;

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
                                if (_isThreadRunning && CommandQueue.CanDequeue()) ProcessCommand(CommandQueue.Dequeue());
                                if (_isThreadRunning) DoWork();
                                if (_isThreadRunning && InputQueue.CanDequeue()) CheckInputs();
                                if (_isThreadRunning) DoWork();
                                if (_isThreadRunning && InputQueue2.CanDequeue()) CheckInputs2();

                                Thread.Yield();
                            }

                            DoCleanup();
                        });
                    }

                    _thread.Start();
                }
            }
        }

        private void CheckInputs2()
        {
            while (InputQueue2.CanDequeue())
            {
                ProcessInput2(InputQueue2.Dequeue());
            }
        }

        protected abstract void ProcessInput2(T_Input2 input);
    }
}
