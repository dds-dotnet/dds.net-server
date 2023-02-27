using DDS.Net.Server.Core.Internal.Entities;

namespace DDS.Net.Server.Core.Internal.Interfaces
{
    internal interface IThreadedDataIO<T_InputQueue, T_OutputQueue>
    {
        event EventHandler<ThreadedDataIOStatus>? ThreadedDataIOStatusChanged;

        void SetOutputDataQueueEnd(ISyncDataOutputQueueEnd<T_OutputQueue> outputQueueEnd);
        void SetInputDataQueueEnd(ISyncDataInputQueueEnd<T_InputQueue> inputQueueEnd);

        ISyncDataOutputQueueEnd<T_OutputQueue> GetOutputDataQueueEnd();
        ISyncDataInputQueueEnd<T_InputQueue> GetInputDataQueueEnd();
    }
}
