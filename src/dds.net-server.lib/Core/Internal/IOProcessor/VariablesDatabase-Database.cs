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
        private Dictionary<ushort, PrimitiveStringVariable> _dbStrings = new();
        private Dictionary<ushort, PrimitiveVariable<bool>> _dbBooleans = new();
        private Dictionary<ushort, PrimitiveVariable<sbyte>> _dbBytes = new();
        private Dictionary<ushort, PrimitiveVariable<short>> _dbWords = new();
        private Dictionary<ushort, PrimitiveVariable<int>> _dbDWords = new();
        private Dictionary<ushort, PrimitiveVariable<long>> _dbQWords = new();
        private Dictionary<ushort, PrimitiveVariable<byte>> _dbUnsignedBytes = new();
        private Dictionary<ushort, PrimitiveVariable<ushort>> _dbUnsignedWords = new();
        private Dictionary<ushort, PrimitiveVariable<uint>> _dbUnsignedDWords = new();
        private Dictionary<ushort, PrimitiveVariable<ulong>> _dbUnsignedQWords = new();
        private Dictionary<ushort, PrimitiveVariable<float>> _dbSingles = new();
        private Dictionary<ushort, PrimitiveVariable<double>> _dbDoubles = new();
        private Dictionary<ushort, CompoundVariable> _dbCompoundVariables = new();

        private void InitializeDatabase()
        {
            lock (_dbMutex)
            {
                _dbNameToId.Clear();

                _dbStrings.Clear();
                _dbBooleans.Clear();
                _dbBytes.Clear();
                _dbWords.Clear();
                _dbDWords.Clear();
                _dbQWords.Clear();
                _dbUnsignedBytes.Clear();
                _dbUnsignedWords.Clear();
                _dbUnsignedDWords.Clear();
                _dbUnsignedQWords.Clear();
                _dbSingles.Clear();
                _dbDoubles.Clear();
                _dbCompoundVariables.Clear();

                foreach (VariableSettings settings in variablesConfiguration.Settings)
                {
                    ushort id = IdGenerator.GetNextVariableId();

                    _dbNameToId.Add(settings.VariableName, id);

                    if (settings is PrimitiveVariableSettings p)
                    {

                    }
                    else if (settings is CompoundVariableSettings c)
                    {
                        _dbCompoundVariables.Add(id, new CompoundVariable(id, settings.VariableName));
                    }
                }
            }
        }

        private void ClearDatabase()
        {
            lock (_dbMutex)
            {
                _dbNameToId.Clear();

                _dbStrings.Clear();
                _dbBooleans.Clear();
                _dbBytes.Clear();
                _dbWords.Clear();
                _dbDWords.Clear();
                _dbQWords.Clear();
                _dbUnsignedBytes.Clear();
                _dbUnsignedWords.Clear();
                _dbUnsignedDWords.Clear();
                _dbUnsignedQWords.Clear();
                _dbSingles.Clear();
                _dbDoubles.Clear();
                _dbCompoundVariables.Clear();
            }
        }
    }
}
