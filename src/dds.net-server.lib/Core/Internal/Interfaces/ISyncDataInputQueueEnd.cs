namespace DDS.Net.Server.Core.Internal.Interfaces
{
    internal interface ISyncDataInputQueueEnd<T>
    {
        event EventHandler<T>? InputDataAvailable;

        bool CanDequeue();
        T Dequeue();
    }
}
