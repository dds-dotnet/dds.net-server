namespace DDS.Net.Server.Core.Internal.Interfaces
{
    internal interface ISyncDataInputQueueEnd<T>
    {
        bool CanEnqueue();
        void Enqueue(T data);
    }
}
