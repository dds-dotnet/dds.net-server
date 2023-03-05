using DDS.Net.Server.Core.Internal.Base;
using DDS.Net.Server.Core.Internal.Base.Entities;
using DDS.Net.Server.Core.Internal.IOProcessor.Types;
using DDS.Net.Server.Entities;

namespace DDS.Net.Server.Core.Internal.IOProcessor
{
    internal partial class VariablesDatabase
        : SinglePipedConsumer<DataFromClient, DataToClient, VarsDbCommand, VarsDbStatus>
    {
        private Dictionary<string, ushort> _dbNameToId = new();
        private Dictionary<ushort, Variable> _dbIdToValue = new();

        private void InitializeDatabase()
        {
        }

        private void ClearDatabase()
        {
        }
    }
}
