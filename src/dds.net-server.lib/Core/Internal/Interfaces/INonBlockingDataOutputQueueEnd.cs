namespace DDS.Net.Server.Core.Internal.Interfaces
{
    internal interface INonBlockingDataOutputQueueEnd<T>
    {
        bool CanEnqueue();
        void Enqueue(T data);
    }
}
