using DDS.Net.Server.Core.Internal.Base;
using DDS.Net.Server.Core.Internal.Base.Entities;
using DDS.Net.Server.Core.Internal.IOProcessor.Helpers;
using DDS.Net.Server.Core.Internal.IOProcessor.Types;
using DDS.Net.Server.Core.Internal.IOProcessor.Types.Variable;
using DDS.Net.Server.Entities;

namespace DDS.Net.Server.Core.Internal.IOProcessor
{
    internal partial class VariablesDatabase
        : SinglePipedConsumer<DataFromClient, DataToClient, VarsDbCommand, VarsDbStatus>
    {
        private Mutex _dbMutex = new();
        private Dictionary<string, ushort> _dbNameToId = new();
        private Dictionary<ushort, BaseVariable> _dbVariables = new();
        private Dictionary<VariableSubscriber, List<BaseVariable>> _dbSubscribedVariables = new();

        private void InitializeDatabase()
        {
            lock (_dbMutex)
            {
                _dbNameToId.Clear();
                _dbVariables.Clear();
                _dbSubscribedVariables.Clear();

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

                        _dbVariables.Add(id, cv);
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
                _dbVariables.Clear();
                _dbSubscribedVariables.Clear();
            }
        }
    }
}
