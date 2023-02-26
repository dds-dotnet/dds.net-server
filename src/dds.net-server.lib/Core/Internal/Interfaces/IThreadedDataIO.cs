using DDS.Net.Server.Core.Internal.Entities;

namespace DDS.Net.Server.Core.Internal.Interfaces
{
    internal interface IThreadedDataIO
    {
        event EventHandler<ThreadedDataIOStatus>? ThreadedDataIOStatusChanged;

        void SetOutputDataQueueEnd(INonBlockingDataOutputQueueEnd outputQueueEnd);
        void SetInputDataQueueEnd(INonBlockingDataInputQueueEnd inputQueueEnd);

        INonBlockingDataOutputQueueEnd GetOutputDataQueueEnd();
        INonBlockingDataInputQueueEnd GetInputDataQueueEnd();
    }
}
