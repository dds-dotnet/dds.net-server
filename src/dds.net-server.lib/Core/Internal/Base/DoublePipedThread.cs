using DDS.Net.Server.Core.Internal.InterfaceImplementations;
using DDS.Net.Server.Core.Internal.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDS.Net.Server.Core.Internal.Base
{
    internal abstract class DoublePipedThread<T_Input1, T_Output1, T_Input2, T_Output2, T_Commands, T_Responses>
        : PipedThreadBase, IDisposable

        where T_Input1 : class
        where T_Input2 : class
        where T_Output1 : class
        where T_Output2 : class
        where T_Commands : struct
        where T_Responses : struct
    {
        public ISyncDataInputQueueEnd<T_Input1> QueuedInput1 { get; private set; }
        public ISyncDataOutputQueueEnd<T_Output1> QueuedOutput1 { get; private set; }

        public ISyncDataInputQueueEnd<T_Input2> QueuedInput2 { get; private set; }
        public ISyncDataOutputQueueEnd<T_Output2> QueuedOutput2 { get; private set; }

        public ISyncDataInputQueueEnd<T_Commands> QueuedCommands { get; private set; }
        public ISyncDataOutputQueueEnd<T_Responses> QueuedResponses { get; private set; }

        protected readonly SyncQueue<T_Input1> dataInputQueue1;
        protected readonly SyncQueue<T_Output1> dataOutputQueue1;

        protected readonly SyncQueue<T_Input2> dataInputQueue2;
        protected readonly SyncQueue<T_Output2> dataOutputQueue2;

        protected readonly SyncQueueStruct<T_Commands> commandsInputQueue;
        protected readonly SyncQueueStruct<T_Responses> responsesOutputQueue;

        protected DoublePipedThread(int inputQueue1Size, int outputQueue1Size,
                                    int inputQueue2Size, int outputQueue2Size,
                                    int commandsQueueSize, int responsesQueueSize)
        {
            dataInputQueue1 = new SyncQueue<T_Input1>(inputQueue1Size);
            dataOutputQueue1 = new SyncQueue<T_Output1>(outputQueue1Size);

            dataInputQueue2 = new SyncQueue<T_Input2>(inputQueue2Size);
            dataOutputQueue2 = new SyncQueue<T_Output2>(outputQueue2Size);

            commandsInputQueue = new SyncQueueStruct<T_Commands>(commandsQueueSize);
            responsesOutputQueue = new SyncQueueStruct<T_Responses>(responsesQueueSize);

            QueuedInput1 = dataInputQueue1;
            QueuedOutput1 = dataOutputQueue1;

            QueuedInput2 = dataInputQueue2;
            QueuedOutput2 = dataOutputQueue2;

            QueuedCommands = commandsInputQueue;
            QueuedResponses = responsesOutputQueue;
        }

        public void Dispose()
        {
        }
    }
}
