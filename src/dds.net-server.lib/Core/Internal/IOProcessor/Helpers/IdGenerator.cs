namespace DDS.Net.Server.Core.Internal.IOProcessor.Helpers
{
    internal static class IdGenerator
    {
        private static Mutex nextVariableIdMutex = new();
        private static ushort nextVariableId = 0;

        public static ushort GetNextVariableId()
        {
            lock (nextVariableIdMutex)
            {
                nextVariableId++;
                return nextVariableId;
            }
        }
    }
}
