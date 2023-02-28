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
        private static int SLEEP_TIME_MS_WHEN_DONE_NOTHING = 10;

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

                            int workStatus1 = 0;
                            int workStatus2 = 0;
                            int workStatus3 = 0;
                            int processCommandStatus = 0;
                            int checkInputsStatus1 = 0;
                            int checkInputsStatus2 = 0;

                            while (_isThreadRunning)
                            {
                                workStatus1 = 0;
                                workStatus2 = 0;
                                workStatus3 = 0;
                                processCommandStatus = 0;
                                checkInputsStatus1 = 0;
                                checkInputsStatus2 = 0;

                                if (_isThreadRunning) { workStatus1 = DoWork(); }
                                if (_isThreadRunning && CommandQueue.CanDequeue()) { processCommandStatus = ProcessCommand(CommandQueue.Dequeue()); }
                                if (_isThreadRunning) { workStatus2 = DoWork(); }
                                if (_isThreadRunning && InputQueue.CanDequeue()) { checkInputsStatus1 = CheckInputs(); }
                                if (_isThreadRunning) { workStatus3 = DoWork(); }
                                if (_isThreadRunning && InputQueue2.CanDequeue()) { checkInputsStatus2 = CheckInputs2(); }

                                if (_isThreadRunning &&
                                    workStatus1 == 0 &&
                                    workStatus2 == 0 &&
                                    workStatus3 == 0 &&
                                    processCommandStatus == 0 &&
                                    checkInputsStatus1 == 0 &&
                                    checkInputsStatus2 == 0)
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

        private int CheckInputs2()
        {
            int workDone = 0;

            while (InputQueue2.CanDequeue())
            {
                ProcessInput2(InputQueue2.Dequeue());
                workDone++;
            }

            return workDone;
        }

        protected abstract void ProcessInput2(T_Input2 input);
    }
}
