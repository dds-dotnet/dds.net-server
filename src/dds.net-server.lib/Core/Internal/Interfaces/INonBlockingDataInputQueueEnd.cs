namespace DDS.Net.Server.Core.Internal.Interfaces
{
    internal interface INonBlockingDataInputQueueEnd<T>
    {
        event EventHandler<T> InputDataAvailable;

        bool IsDataAvailable();
        T DequeueData();
    }
}
