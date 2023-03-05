using DDS.Net.Server.Core.Internal.Base;
using DDS.Net.Server.Core.Internal.Base.Entities;
using DDS.Net.Server.Core.Internal.IOProcessor.Helpers;
using DDS.Net.Server.Core.Internal.IOProcessor.Types;
using DDS.Net.Server.Entities;

namespace DDS.Net.Server.Core.Internal.IOProcessor
{
    internal partial class VariablesDatabase
        : SinglePipedConsumer<DataFromClient, DataToClient, VarsDbCommand, VarsDbStatus>
    {
        private Mutex _dbMutex = new();
        private Dictionary<string, ushort> _dbNameToId = new();
        private Dictionary<ushort, Variable> _dbIdToValue = new();

        private void InitializeDatabase()
        {
            lock (_dbMutex)
            {
                _dbNameToId.Clear();
                _dbIdToValue.Clear();

                foreach (VariableSettings settings in variablesConfiguration.Settings)
                {
                    ushort id = IdGenerator.GetNextVariableId();

                    _dbNameToId.Add(settings.VariableName, id);

                    if (settings is PrimitiveVariableSettings p)
                    {

                    }
                    else if (settings is CompoundVariableSettings c)
                    {

                    }
                }
            }
        }

        private void ClearDatabase()
        {
        }
    }
}
