namespace DDS.Net.Server.Core.Internal.Interfaces
{
    internal interface ISyncDataOutputQueueEnd<T>
    {
        bool CanEnqueue();
        void Enqueue(T data);
    }
}
