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
    internal abstract class SinglePipedThread<T_Input, T_Output, T_Commands, T_Responses> : PipedThreadBase, IDisposable
        where T_Input : class
        where T_Output : class
        where T_Commands : struct
        where T_Responses : struct
    {
        public ISyncDataInputQueueEnd<T_Input> QueuedInput { get; private set; }
        public ISyncDataOutputQueueEnd<T_Output> QueuedOutput { get; private set; }

        public ISyncDataInputQueueEnd<T_Commands> QueuedCommands { get; private set; }
        public ISyncDataOutputQueueEnd<T_Responses> QueuedResponses { get; private set; }

        protected readonly SyncQueue<T_Input> dataInputQueue;
        protected readonly SyncQueue<T_Output> dataOutputQueue;

        protected readonly SyncQueueStruct<T_Commands> commandsInputQueue;
        protected readonly SyncQueueStruct<T_Responses> responsesOutputQueue;

        protected SinglePipedThread(int inputQueueSize, int outputQueueSize, int commandsQueueSize, int responsesQueueSize)
        {
            dataInputQueue = new SyncQueue<T_Input>(inputQueueSize);
            dataOutputQueue = new SyncQueue<T_Output>(outputQueueSize);

            commandsInputQueue = new SyncQueueStruct<T_Commands>(commandsQueueSize);
            responsesOutputQueue = new SyncQueueStruct<T_Responses>(responsesQueueSize);

            QueuedInput = dataInputQueue;
            QueuedOutput = dataOutputQueue;

            QueuedCommands = commandsInputQueue;
            QueuedResponses = responsesOutputQueue;
        }

        public void Dispose()
        {
        }
    }
}
