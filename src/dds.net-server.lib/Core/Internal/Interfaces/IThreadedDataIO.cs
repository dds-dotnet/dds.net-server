using DDS.Net.Server.Core.Internal.Entities;

namespace DDS.Net.Server.Core.Internal.Interfaces
{
    internal interface IThreadedDataIO
    {
        event EventHandler<ThreadedDataIOStatus>? ThreadedDataIOStatusChanged;

        void SetOutputDataQueueEnd(INonBlockingDataOutputQueue outputQueueEnd);
        void SetInputDataQueueEnd(INonBlockingDataInputQueue inputQueueEnd);

        INonBlockingDataOutputQueue GetOutputDataQueueEnd();
        INonBlockingDataInputQueue GetInputDataQueueEnd();
    }
}
