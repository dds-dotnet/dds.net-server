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
        private Dictionary<string, ushort> _dbVariableIds = new();
        private Dictionary<ushort, BaseVariable> _dbVariables = new();
        private List<VariableSubscriber> _dbSubscribers = new();

        private void InitializeDatabase()
        {
            lock (_dbMutex)
            {
                _dbVariableIds.Clear();
                _dbVariables.Clear();
                _dbSubscribers.Clear();

                //- 
                //- Processing Primitive variables
                //- 
                foreach (VariableSettings settings in variablesConfiguration.Settings)
                {
                    ushort id = IdGenerator.GetNextVariableId();

                    _dbVariableIds.Add(settings.VariableName, id);

                    if (settings is PrimitiveVariableSettings p)
                    {
                        AddPrimitiveVariableFromSettings(id, p);
                    }
                }

                //- 
                //- Processing Compound variables
                //- 
                foreach (VariableSettings settings in variablesConfiguration.Settings)
                {
                    ushort id = IdGenerator.GetNextVariableId();

                    _dbVariableIds.Add(settings.VariableName, id);

                    if (settings is CompoundVariableSettings c)
                    {
                        CompoundVariable cv = new(id, settings.VariableName);

                        _dbVariables.Add(id, cv);
                    }
                }
            }
        }
        /// <summary>
        /// Adds a primitive variable to the database according to the provided settings.
        /// It must not be used outside the context of database initialization from
        /// provided configuration file / initialization settings.
        /// </summary>
        /// <param name="id">The ID already assigned to the variable name</param>
        /// <param name="primitiveSettings">The settings object</param>
        private void AddPrimitiveVariableFromSettings(ushort id, PrimitiveVariableSettings primitiveSettings)
        {
            switch (primitiveSettings.PrimitiveType)
            {
                case PrimitiveType.String:
                    _dbVariables.Add(id, new StringVariable(id, primitiveSettings.VariableName));
                    break;

                case PrimitiveType.Boolean:
                    _dbVariables.Add(id, new BooleanVariable(id, primitiveSettings.VariableName));
                    break;

                case PrimitiveType.Byte:
                    _dbVariables.Add(id, new ByteVariable(id, primitiveSettings.VariableName));
                    break;

                case PrimitiveType.Word:
                    _dbVariables.Add(id, new WordVariable(id, primitiveSettings.VariableName));
                    break;

                case PrimitiveType.DWord:
                    _dbVariables.Add(id, new DWordVariable(id, primitiveSettings.VariableName));
                    break;

                case PrimitiveType.QWord:
                    _dbVariables.Add(id, new QWordVariable(id, primitiveSettings.VariableName));
                    break;

                case PrimitiveType.UnsignedByte:
                    _dbVariables.Add(id, new UnsignedByteVariable(id, primitiveSettings.VariableName));
                    break;

                case PrimitiveType.UnsignedWord:
                    _dbVariables.Add(id, new UnsignedWordVariable(id, primitiveSettings.VariableName));
                    break;

                case PrimitiveType.UnsignedDWord:
                    _dbVariables.Add(id, new UnsignedDWordVariable(id, primitiveSettings.VariableName));
                    break;

                case PrimitiveType.UnsignedQWord:
                    _dbVariables.Add(id, new UnsignedQWordVariable(id, primitiveSettings.VariableName));
                    break;

                case PrimitiveType.Single:
                    _dbVariables.Add(id, new SingleVariable(id, primitiveSettings.VariableName));
                    break;

                case PrimitiveType.Double:
                    _dbVariables.Add(id, new DoubleVariable(id, primitiveSettings.VariableName));
                    break;

                case PrimitiveType.UnknownPrimitiveType:
                    _dbVariables.Add(id, new UnknownVariable(id, primitiveSettings.VariableName));
                    logger.Error($"Unknown primitive \"{primitiveSettings.VariableName}\" must be avoided");
                    break;
            }
        }
        /// <summary>
        /// Clears out all the held variables and their associated data.
        /// </summary>
        private void ClearDatabase()
        {
            lock (_dbMutex)
            {
                _dbVariableIds.Clear();
                _dbVariables.Clear();
                _dbSubscribers.Clear();
            }
        }
    }
}
