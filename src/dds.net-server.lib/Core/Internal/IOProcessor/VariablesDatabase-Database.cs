using DDS.Net.Server.Core.Internal.Base;
using DDS.Net.Server.Core.Internal.Base.Entities;
using DDS.Net.Server.Core.Internal.IOProcessor.Types;

namespace DDS.Net.Server.Core.Internal.IOProcessor
{
    internal partial class VariablesDatabase
        : SinglePipedConsumer<DataFromClient, DataToClient, VarsDbCommand, VarsDbStatus>
    {
        private Mutex _primitivesDatabaseMutex = new();
        private Dictionary<string, Variable> _primitivesDatabaseDictionary = new();

        private void InitializeDatabase()
        {
            lock (_primitivesDatabaseMutex)
            {
            }
        }

        private void ClearDatabase()
        {
            lock (_primitivesDatabaseMutex)
            {
                _primitivesDatabaseDictionary.Clear();
            }
        }
    }
}
