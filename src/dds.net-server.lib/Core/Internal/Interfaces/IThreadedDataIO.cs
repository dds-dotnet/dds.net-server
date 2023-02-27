using DDS.Net.Server.Core.Internal.Entities;

namespace DDS.Net.Server.Core.Internal.Interfaces
{
    internal interface IThreadedDataIO<T_InputQueue, T_OutputQueue>
    {
        event EventHandler<ThreadedDataIOStatus>? ThreadedDataIOStatusChanged;

        void StartIO();
        void StopIO();

        ISyncDataOutputQueueEnd<T_OutputQueue> GetOutputDataQueueEnd();
        ISyncDataInputQueueEnd<T_InputQueue> GetInputDataQueueEnd();
    }
}
