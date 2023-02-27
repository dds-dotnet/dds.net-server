namespace DDS.Net.Server.Core.Internal.Interfaces
{
    internal interface ISyncDataOutputQueueEnd<T>
    {
        event EventHandler<T>? DataAvailableForOutput;

        bool CanDequeue();
        T Dequeue();
    }
}
