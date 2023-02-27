using DDS.Net.Server.Core.Internal.InterfaceImplementations;
using DDS.Net.Server.Core.Internal.Interfaces;

namespace DDS.Net.Server.Core.Internal.Base
{
    internal abstract class PipedThreadBase<T_Commands, T_Responses> : IDisposable

        where T_Commands : struct
        where T_Responses : struct
    {
        public ISyncDataInputQueueEnd<T_Commands> QueuedCommands { get; private set; }
        public ISyncDataOutputQueueEnd<T_Responses> QueuedResponses { get; private set; }

        protected readonly SyncQueueStruct<T_Commands> commandsInputQueue;
        protected readonly SyncQueueStruct<T_Responses> responsesOutputQueue;

        protected PipedThreadBase(int commandsQueueSize, int responsesQueueSize)
        {
            commandsInputQueue = new SyncQueueStruct<T_Commands>(commandsQueueSize);
            responsesOutputQueue = new SyncQueueStruct<T_Responses>(responsesQueueSize);

            QueuedCommands = commandsInputQueue;
            QueuedResponses = responsesOutputQueue;
        }

        protected abstract void Process();

        public abstract void Dispose();
    }
}
