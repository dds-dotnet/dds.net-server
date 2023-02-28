namespace DDS.Net.Server.Core.Internal.Interfaces
{
    internal interface ISyncDataWriterQueueEnd<T>
    {
        bool CanEnqueue();
        void Enqueue(T data);
    }
}
