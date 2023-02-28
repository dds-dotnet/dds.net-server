using DDS.Net.Server.Core.Internal.Interfaces;
using DDS.Net.Server.Core.Internal.Interfaces.Implementations;

namespace DDS.Net.Server.Core.Internal.Base
{
    internal abstract class SinglePipedProducer<T_Input, T_Output, T_Commands, T_Responses>
        : BasePipedProcess<T_Commands, T_Responses>, IDisposable

        where T_Input : class
        where T_Output : class
        where T_Commands : struct
        where T_Responses : struct
    {
        public ISyncQueueWriterEnd<T_Input> InputWriter { get; private set; }
        public ISyncQueueReaderEnd<T_Output> OutputReader { get; private set; }

        protected readonly SyncQueue<T_Input> InputQueue;
        protected readonly SyncQueue<T_Output> OutputQueue;


        protected SinglePipedProducer(
                    int inputQueueSize,
                    int outputQueueSize,
                    int commandsQueueSize,
                    int responsesQueueSize,
                    bool startThread = true)

            : base(commandsQueueSize, responsesQueueSize, false)
        {
            InputQueue = new SyncQueue<T_Input>(inputQueueSize);
            OutputQueue = new SyncQueue<T_Output>(outputQueueSize);

            InputWriter = InputQueue;
            OutputReader = OutputQueue;

            if (startThread)
            {
                StartThread();
            }
        }

        protected override int CheckInputs()
        {
            int workDone = 0;

            while (InputQueue.CanDequeue())
            {
                ProcessInput(InputQueue.Dequeue());
                workDone++;
            }

            return workDone;
        }

        protected abstract void ProcessInput(T_Input input);
    }
}
