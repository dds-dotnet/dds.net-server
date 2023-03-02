namespace DDS.Net.Server.Core.Internal.IOProcessor.Helpers
{
    internal static class IDGenerator
    {
        private static Mutex mutex = new();
        private static ushort nextID = 0;

        public static ushort GetNextID()
        {
            lock (mutex)
            {
                nextID++;
                return nextID;
            }
        }
    }
}
