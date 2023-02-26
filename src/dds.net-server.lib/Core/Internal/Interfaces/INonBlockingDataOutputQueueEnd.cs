namespace DDS.Net.Server.Core.Internal.Interfaces
{
    internal interface INonBlockingDataOutputQueueEnd<T>
    {
        bool CanEnqueueData();
        void EnqueueData(T data);
    }
}
