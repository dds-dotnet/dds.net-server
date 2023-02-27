using DDS.Net.Server.Core.Internal.InterfaceImplementations;
using DDS.Net.Server.Core.Internal.Interfaces;

namespace DDS.Net.Server.Core.Internal.Base
{
    internal abstract class PipedThreadBase<T_Commands, T_Responses> : IDisposable

        where T_Commands : struct
        where T_Responses : struct
    {
        public ISyncDataInputQueueEnd<T_Commands> Commands { get; private set; }
        public ISyncDataOutputQueueEnd<T_Responses> Responses { get; private set; }

        protected readonly SyncQueueStruct<T_Commands> commandsQueue;
        protected readonly SyncQueueStruct<T_Responses> responsesQueue;

        protected PipedThreadBase(int commandsQueueSize, int responsesQueueSize, bool startThread = true)
        {
            commandsQueue = new SyncQueueStruct<T_Commands>(commandsQueueSize);
            responsesQueue = new SyncQueueStruct<T_Responses>(responsesQueueSize);

            Commands = commandsQueue;
            Responses = responsesQueue;

            if (startThread)
            {
                StartThread();
            }
        }

        protected virtual void StartThread()
        {

        }

        protected abstract void DoWork();

        public abstract void Dispose();
    }
}
