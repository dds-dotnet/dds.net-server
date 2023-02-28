using DDS.Net.Server.Core.Internal.Interfaces;

namespace DDS.Net.Server.Core.Internal.Base
{
    internal abstract class DualPipedConsumer<T_Input1, T_Output1, T_Input2, T_Output2, T_Commands, T_Responses>
        : BasePipedProcess<T_Commands, T_Responses>, IDisposable

        where T_Input1 : class
        where T_Output1 : class
        where T_Commands : struct
        where T_Responses : struct
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

        protected override void CheckInputs()
        {
            while (InputQueue.CanDequeue())
            {
                ProcessInput(InputQueue.Dequeue());
            }

            while (InputQueue2.CanDequeue())
            {
                ProcessInput2(InputQueue2.Dequeue());
            }
        }

        protected abstract void ProcessInput(T_Input1 input);
        protected abstract void ProcessInput2(T_Input2 input);
    }
}
