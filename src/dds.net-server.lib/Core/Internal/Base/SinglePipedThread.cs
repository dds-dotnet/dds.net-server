using DDS.Net.Server.Core.Internal.Entities;
using DDS.Net.Server.Core.Internal.InterfaceImplementations;
using DDS.Net.Server.Core.Internal.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDS.Net.Server.Core.Internal.Base
{
    internal abstract class SinglePipedThread<T_Input, T_Output, T_Commands, T_Responses>
        : PipedThreadBase<T_Commands, T_Responses>, IDisposable

        where T_Input : class
        where T_Output : class
        where T_Commands : struct
        where T_Responses : struct
    {
        public ISyncDataInputQueueEnd<T_Input> Input { get; private set; }
        public ISyncDataOutputQueueEnd<T_Output> Output { get; private set; }

        protected readonly SyncQueue<T_Input> inputQueue;
        protected readonly SyncQueue<T_Output> outputQueue;


        protected SinglePipedThread(
                    int inputQueueSize,
                    int outputQueueSize,
                    int commandsQueueSize,
                    int responsesQueueSize,
                    bool startThread = true)

            : base(commandsQueueSize, responsesQueueSize, false)
        {
            inputQueue = new SyncQueue<T_Input>(inputQueueSize);
            outputQueue = new SyncQueue<T_Output>(outputQueueSize);

            Input = inputQueue;
            Output = outputQueue;

            if (startThread)
            {
                StartThread();
            }
        }

        protected override void StartThread()
        {
            base.StartThread();
        }
    }
}
