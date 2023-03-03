namespace DDS.Net.Server.Core.Internal.IOProcessor.Helpers
{
    internal static class IdGenerator2
    {
        private static Mutex mutex = new();
        private static ushort nextId = 0;

        public static ushort GetNextID()
        {
            lock (mutex)
            {
                nextId++;
                return nextId;
            }
        }
    }
}
