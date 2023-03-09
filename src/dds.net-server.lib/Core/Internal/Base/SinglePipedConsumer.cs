using DDS.Net.Server.Core.Internal.Interfaces;

namespace DDS.Net.Server.Core.Internal.Base
{
    internal abstract class SinglePipedConsumer<T_Input, T_Output, T_Command, T_Response>
        : BasePipedProcess<T_Command, T_Response>, IDisposable

        where T_Input : class
        where T_Output : class
        where T_Command : class
        where T_Response : class
    {
        protected ISyncQueueReaderEnd<T_Input> InputQueue { get; private set; }
        protected ISyncQueueWriterEnd<T_Output> OutputQueue { get; private set; }

        protected SinglePipedConsumer(
                    ISyncQueueReaderEnd<T_Input> inputQueueEnd,
                    ISyncQueueWriterEnd<T_Output> outputQueueEnd,
                    int commandsQueueSize,
                    int responsesQueueSize,
                    bool startThread = true)

            : base(commandsQueueSize, responsesQueueSize, false)
        {
            InputQueue = inputQueueEnd ?? throw new ArgumentNullException(nameof(inputQueueEnd));
            OutputQueue = outputQueueEnd ?? throw new ArgumentNullException(nameof(outputQueueEnd));

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
