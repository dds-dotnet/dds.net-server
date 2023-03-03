namespace DDS.Net.Server.Core.Internal.IOProcessor.Helpers
{
    internal static class IdGenerator
    {
        private static Mutex mutex = new();
        private static ushort nextId = 0;

        public static ushort GetNextId()
        {
            lock (mutex)
            {
                nextId++;
                return nextId;
            }
        }
    }
}
