using DDS.Net.Server.Core.Internal.Entities;

namespace DDS.Net.Server.Core.Internal.Interfaces
{
    internal interface IThreadedDataIO<T_InputQueue, T_OutputQueue>
    {
        event EventHandler<ThreadedDataIOStatus>? ThreadedDataIOStatusChanged;

        void SetOutputDataQueueEnd(INonBlockingDataOutputQueueEnd<T_OutputQueue> outputQueueEnd);
        void SetInputDataQueueEnd(INonBlockingDataInputQueueEnd<T_InputQueue> inputQueueEnd);

        INonBlockingDataOutputQueueEnd<T_OutputQueue> GetOutputDataQueueEnd();
        INonBlockingDataInputQueueEnd<T_InputQueue> GetInputDataQueueEnd();
    }
}
