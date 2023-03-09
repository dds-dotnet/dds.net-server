using DDS.Net.Server.Core.Internal.Interfaces;
using DDS.Net.Server.Core.Internal.Interfaces.Implementations;

namespace DDS.Net.Server.Core.Internal.Base
{
    internal abstract class SinglePipedProducer<T_Input, T_Output, T_Command, T_Response>
        : BasePipedProcess<T_Command, T_Response>, IDisposable

        where T_Input : class
        where T_Output : class
        where T_Command : class
        where T_Response : class
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

        /// <summary>
        /// Process the input coming from queue.
        /// </summary>
        /// <param name="input">Input coming from queue.</param>
        /// <returns>Number of tasks done, 0 if nothing is done during invocation.</returns>
        protected abstract int ProcessInput(T_Input input);
    }
}
