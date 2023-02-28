namespace DDS.Net.Server.Core.Internal.Interfaces
{
    internal interface ISyncQueueWriterEnd<T>
    {
        bool CanEnqueue();
        void Enqueue(T data);
    }
}
