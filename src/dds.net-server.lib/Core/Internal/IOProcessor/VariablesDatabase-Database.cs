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

                //- 
                //- Processing Primitive variables
                //- 
                foreach (VariableSettings settings in variablesConfiguration.Settings)
                {
                    ushort id = IdGenerator.GetNextVariableId();

                    _dbNameToId.Add(settings.VariableName, id);

                    if (settings is PrimitiveVariableSettings p)
                    {
                        AddPrimitiveVariableFromSettings(p);
                    }
                }

                //- 
                //- Processing Compound variables
                //- 
                foreach (VariableSettings settings in variablesConfiguration.Settings)
                {
                    ushort id = IdGenerator.GetNextVariableId();

                    _dbNameToId.Add(settings.VariableName, id);

                    if (settings is CompoundVariableSettings c)
                    {
                        CompoundVariable cv = new(id, settings.VariableName);

                        _dbCompoundVariables.Add(id, cv);
                    }
                }
            }
        }

        private void AddPrimitiveVariableFromSettings(PrimitiveVariableSettings primitiveSettings)
        {
            switch (primitiveSettings.PrimitiveType)
            {
                case PrimitiveType.String:
                    break;

                case PrimitiveType.Boolean:
                    break;

                case PrimitiveType.Byte:
                    break;

                case PrimitiveType.Word:
                    break;

                case PrimitiveType.DWord:
                    break;

                case PrimitiveType.QWord:
                    break;

                case PrimitiveType.UnsignedByte:
                    break;

                case PrimitiveType.UnsignedWord:
                    break;

                case PrimitiveType.UnsignedDWord:
                    break;

                case PrimitiveType.UnsignedQWord:
                    break;

                case PrimitiveType.Single:
                    break;

                case PrimitiveType.Double:
                    break;

                case PrimitiveType.UNKNOWN:
                    break;
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
