using DDS.Net.Server.Core.Internal.Interfaces;
using DDS.Net.Server.Core.Internal.Interfaces.Implementations;

namespace DDS.Net.Server.Core.Internal.Base
{
    internal abstract class SinglePipedThreadConsumer<T_Input, T_Output, T_Commands, T_Responses>
        : PipedThreadBase<T_Commands, T_Responses>, IDisposable

        where T_Input : class
        where T_Output : class
        where T_Commands : struct
        where T_Responses : struct
    {
        public ISyncQueueReaderEnd<T_Input> Input { get; private set; }
        public ISyncQueueWriterEnd<T_Output> Output { get; private set; }

        protected SinglePipedThreadConsumer(
                    ISyncQueueReaderEnd<T_Input> input,
                    ISyncQueueWriterEnd<T_Output> output,
                    int commandsQueueSize,
                    int responsesQueueSize,
                    bool startThread = true)

            : base(commandsQueueSize, responsesQueueSize, false)
        {
            Input = input ?? throw new ArgumentNullException(nameof(input));
            Output = output ?? throw new ArgumentNullException(nameof(output));

            if (startThread)
            {
                StartThread();
            }
        }

        protected override void CheckInputs()
        {
            while (Input.CanDequeue())
            {
                ProcessInput(Input.Dequeue());
            }
        }

        protected abstract void ProcessInput(T_Input input);
    }
}
