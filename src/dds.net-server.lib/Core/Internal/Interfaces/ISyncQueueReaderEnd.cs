namespace DDS.Net.Server.Core.Internal.Interfaces
{
    internal interface ISyncQueueReaderEnd<T>
    {
        event EventHandler<T>? DataAvailableForReading;

        bool CanDequeue();
        T Dequeue();
    }
}
