namespace DDS.Net.Server.Core.Internal.Interfaces
{
    internal interface ISyncQueueWriterEnd<T>
    {
        /// <summary>
        /// Checks if the queue has enough space to enqueue any data element.
        /// </summary>
        /// <returns>True when the queue has empty space, False otherwise.</returns>
        bool CanEnqueue();
        /// <summary>
        /// Enqueues the provided data element into the queue.
        /// </summary>
        /// <param name="data">The data element</param>
        void Enqueue(T data);
    }
}
