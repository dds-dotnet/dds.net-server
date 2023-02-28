﻿using DDS.Net.Server.Core.Internal.Interfaces;

namespace DDS.Net.Server.Core.Internal.Base
{
    internal abstract class SinglePipedConsumer<T_Input, T_Output, T_Commands, T_Responses>
        : PipedProcessBase<T_Commands, T_Responses>, IDisposable

        where T_Input : class
        where T_Output : class
        where T_Commands : struct
        where T_Responses : struct
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

        protected override void CheckInputs()
        {
            while (InputQueue.CanDequeue())
            {
                ProcessInput(InputQueue.Dequeue());
            }
        }

        protected abstract void ProcessInput(T_Input input);
    }
}
