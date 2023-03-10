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
        /// <summary>
        /// Initializing the database from provided settings
        /// </summary>
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
                    if (settings is PrimitiveVariableSettings p)
                    {
                        if (_dbVariableIds.ContainsKey(settings.VariableName))
                        {
                            logger.Error($"Variable named \"{settings.VariableName}\" cannot be added twice");
                            continue;
                        }

                        ushort id = IdGenerator.GetNextVariableId();

                        _dbVariableIds.Add(settings.VariableName, id);

                        AddPrimitiveVariableFromSettings(id, p);
                    }
                }

                //- 
                //- Processing Compound variables
                //- 
                foreach (VariableSettings settings in variablesConfiguration.Settings)
                {
                    if (settings is CompoundVariableSettings c)
                    {
                        if (_dbVariableIds.ContainsKey(settings.VariableName))
                        {
                            logger.Error($"Variable named \"{settings.VariableName}\" cannot be added twice");
                            continue;
                        }

                        ushort id = IdGenerator.GetNextVariableId();

                        _dbVariableIds.Add(settings.VariableName, id);

                        CompoundVariable cv = new(id, settings.VariableName);

                        foreach (string pn in c.PrimitiveNames)
                        {
                            if (_dbVariableIds.ContainsKey(pn))
                            {
                                ushort vid = _dbVariableIds[pn];

                                if (_dbVariables.ContainsKey(vid))
                                {
                                    cv.AddVariable(_dbVariables[vid]);
                                }
                                else
                                {
                                    logger.Error($"Variable named \"{pn}\" with id {vid} " +
                                                 $"does not exist in DB to add to CompoundVariable named \"{c.VariableName}\"");
                                }
                            }
                            else
                            {
                                logger.Error($"Variable named \"{pn}\" does not exist in DB " +
                                             $"to add to CompoundVariable named \"{c.VariableName}\"");
                            }
                        }

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
                    return;

                case PrimitiveType.Boolean:
                    _dbVariables.Add(id, new BooleanVariable(id, primitiveSettings.VariableName));
                    return;

                case PrimitiveType.Byte:
                    _dbVariables.Add(id, new ByteVariable(id, primitiveSettings.VariableName));
                    return;

                case PrimitiveType.Word:
                    _dbVariables.Add(id, new WordVariable(id, primitiveSettings.VariableName));
                    return;

                case PrimitiveType.DWord:
                    _dbVariables.Add(id, new DWordVariable(id, primitiveSettings.VariableName));
                    return;

                case PrimitiveType.QWord:
                    _dbVariables.Add(id, new QWordVariable(id, primitiveSettings.VariableName));
                    return;

                case PrimitiveType.UnsignedByte:
                    _dbVariables.Add(id, new UnsignedByteVariable(id, primitiveSettings.VariableName));
                    return;

                case PrimitiveType.UnsignedWord:
                    _dbVariables.Add(id, new UnsignedWordVariable(id, primitiveSettings.VariableName));
                    return;

                case PrimitiveType.UnsignedDWord:
                    _dbVariables.Add(id, new UnsignedDWordVariable(id, primitiveSettings.VariableName));
                    return;

                case PrimitiveType.UnsignedQWord:
                    _dbVariables.Add(id, new UnsignedQWordVariable(id, primitiveSettings.VariableName));
                    return;

                case PrimitiveType.Single:
                    _dbVariables.Add(id, new SingleVariable(id, primitiveSettings.VariableName));
                    return;

                case PrimitiveType.Double:
                    _dbVariables.Add(id, new DoubleVariable(id, primitiveSettings.VariableName));
                    return;

                case PrimitiveType.UnknownPrimitiveType:
                    _dbVariables.Add(id, new UnknownVariable(id, primitiveSettings.VariableName));
                    logger.Error($"Unknown primitive \"{primitiveSettings.VariableName}\" must be avoided");
                    return;
            }

            throw new NotImplementedException($"Variable type {primitiveSettings.PrimitiveType} not implemented!");
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
        /// <summary>
        /// Removes the client from subscription and provision; i.e., the client is disconnected.
        /// </summary>
        /// <param name="clientRef">Reference to the client.</param>
        private void RemoveClient(string clientRef)
        {
            lock (_dbMutex)
            {
                // TODO: To be implemented
            }
        }
        /// <summary>
        /// Registers a client for specified variable.
        /// </summary>
        /// <param name="clientRef">Reference to the client.</param>
        /// <param name="variableName">Name of the variable.</param>
        /// <param name="periodicity">Update periodicity.</param>
        /// <returns>Variable ID</returns>
        private ushort RegisterVariableClient(string clientRef, string variableName, Periodicity periodicity)
        {
            lock (_dbMutex)
            {
                // TODO: To be implemented
                throw new NotImplementedException();
            }
        }
        /// <summary>
        /// Unregisters a client for specified variable.
        /// </summary>
        /// <param name="clientRef">Reference to the client.</param>
        /// <param name="variableName">Name of the variable.</param>
        /// <param name="periodicity">Update periodicity.</param>
        /// <returns>Variable ID</returns>
        private ushort UnregisterVariableClient(string clientRef, string variableName, Periodicity periodicity)
        {
            lock (_dbMutex)
            {
                // TODO: To be implemented
                throw new NotImplementedException();
            }
        }
    }
}
