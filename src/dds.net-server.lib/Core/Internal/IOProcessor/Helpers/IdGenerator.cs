namespace DDS.Net.Server.Core.Internal.IOProcessor.Helpers
{
    internal static class IdGenerator
    {
        private static Mutex nextVariableIdMutex = new();
        private static ushort lastAssignedVariableId = 0;

        /// <summary>
        /// Generates and provides a new unique ID that can be assigned to the next variable.
        /// </summary>
        /// <returns>Generated ID to be assigned to the next registered variable.</returns>
        public static ushort GetNextVariableId()
        {
            lock (nextVariableIdMutex)
            {
                lastAssignedVariableId++;
                return lastAssignedVariableId;
            }
        }
    }
}
