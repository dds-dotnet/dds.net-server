namespace DDS.Net.Server.Core.Internal.Interfaces
{
    internal interface ISyncDataReaderQueueEnd<T>
    {
        event EventHandler<T>? DataAvailableForReading;

        bool CanDequeue();
        T Dequeue();
    }
}
