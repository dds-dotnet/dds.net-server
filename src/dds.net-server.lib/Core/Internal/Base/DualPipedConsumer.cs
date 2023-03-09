using DDS.Net.Server.Core.Internal.Interfaces;

namespace DDS.Net.Server.Core.Internal.Base
{
    internal abstract class DualPipedConsumer<T_Input1, T_Output1, T_Input2, T_Output2, T_Command, T_Response>
        : BasePipedProcess<T_Command, T_Response>, IDisposable

        where T_Input1 : class
        where T_Output1 : class
        where T_Command : class
        where T_Response : class
    {
        protected ISyncQueueReaderEnd<T_Input1> InputQueue { get; private set; }
        protected ISyncQueueWriterEnd<T_Output1> OutputQueue { get; private set; }

        protected ISyncQueueReaderEnd<T_Input2> InputQueue2 { get; private set; }
        protected ISyncQueueWriterEnd<T_Output2> OutputQueue2 { get; private set; }

        protected DualPipedConsumer(
                    ISyncQueueReaderEnd<T_Input1> inputQueueEnd1,
                    ISyncQueueWriterEnd<T_Output1> outputQueueEnd1,

                    ISyncQueueReaderEnd<T_Input2> inputQueueEnd2,
                    ISyncQueueWriterEnd<T_Output2> outputQueueEnd2,

                    int commandsQueueSize,
                    int responsesQueueSize,
                    bool startThread = true)

            : base(commandsQueueSize, responsesQueueSize, false)
        {
            InputQueue = inputQueueEnd1 ?? throw new ArgumentNullException(nameof(inputQueueEnd1));
            OutputQueue = outputQueueEnd1 ?? throw new ArgumentNullException(nameof(outputQueueEnd1));

            InputQueue2 = inputQueueEnd2 ?? throw new ArgumentNullException(nameof(inputQueueEnd2));
            OutputQueue2 = outputQueueEnd2 ?? throw new ArgumentNullException(nameof(outputQueueEnd2));

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

            while (InputQueue2.CanDequeue())
            {
                ProcessInput2(InputQueue2.Dequeue());
                workDone++;
            }

            return workDone;
        }

        /// <summary>
        /// Process the input coming from queue.
        /// </summary>
        /// <param name="input">Input coming from queue.</param>
        /// <returns>Number of tasks done, 0 if nothing is done during invocation.</returns>
        protected abstract int ProcessInput(T_Input1 input);
        /// <summary>
        /// Process the input coming from queue.
        /// </summary>
        /// <param name="input">Input coming from queue.</param>
        /// <returns>Number of tasks done, 0 if nothing is done during invocation.</returns>
        protected abstract int ProcessInput2(T_Input2 input);
    }
}
