﻿using DDS.Net.Server.Core.Internal.Base;
using DDS.Net.Server.Core.Internal.Base.Entities;
using DDS.Net.Server.Core.Internal.IOProcessor.EncodersAndDecoders;
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

        #region Initialization - from provided settings




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
                    _dbVariables.Add(id, new UnknownPrimitiveVariable(id, primitiveSettings.VariableName));
                    logger.Error($"Unknown primitive \"{primitiveSettings.VariableName}\" must be avoided");
                    return;
            }

            throw new NotImplementedException($"Variable type {primitiveSettings.PrimitiveType} not implemented!");
        }
        
        


        #endregion
        #region Clearing all the data




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
        
        


        #endregion
        #region Removing a client - from subscription and provision




        /// <summary>
        /// Removes the client from subscription and provision; i.e., the client is disconnected.
        /// </summary>
        /// <param name="clientRef">Reference to the client.</param>
        private void RemoveClient(string clientRef)
        {
            lock (_dbMutex)
            {
                __RemoveVariableSubscribers(clientRef);
                __RemoveVariableProviders(clientRef);
            }
        }
        /// <summary>
        /// Removes <c>VariableSubscriber</c> instances from <c>_dbSubscribers</c>
        /// that have specified client.
        /// </summary>
        /// <param name="clientRef">Client identifier.</param>
        private void __RemoveVariableSubscribers(string clientRef)
        {
            List<VariableSubscriber> subscribersToBeRemoved = new();

            foreach (VariableSubscriber s in _dbSubscribers)
            {
                if (s.ClientRef == clientRef)
                {
                    subscribersToBeRemoved.Add(s);
                }
            }

            foreach (VariableSubscriber s in subscribersToBeRemoved)
            {
                _dbSubscribers.Remove(s);
            }
        }
        /// <summary>
        /// Removes <c>VariableProvider</c> instances from <c>_dbVariables.Providers</c>
        /// that have specified client.
        /// </summary>
        /// <param name="clientRef">Client identifier.</param>
        private void __RemoveVariableProviders(string clientRef)
        {
            List<VariableProvider> providersToBeRemoved = new();

            foreach (BaseVariable var in _dbVariables.Values)
            {
                providersToBeRemoved.Clear();

                foreach (VariableProvider p in var.Providers)
                {
                    if (p.ClientRef == clientRef)
                    {
                        providersToBeRemoved.Add(p);
                    }
                }

                foreach (VariableProvider p in providersToBeRemoved)
                {
                    var.Providers.Remove(p);
                }
            }
        }
        
        


        #endregion
        #region Registering / unregistering a client - subscription / unsubscription




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
                ushort id = __GetVariableIdCreateUnknownIfNotExists(variableName);

                bool exists = false;

                foreach (VariableSubscriber s in _dbSubscribers)
                {
                    if (s.VariableId == id &&
                        s.ClientRef == clientRef &&
                        s.Periodicity == periodicity)
                    {
                        exists = true;
                        break;
                    }
                }

                if (!exists)
                {
                    _dbSubscribers.Add(new VariableSubscriber(id, clientRef, periodicity));
                }

                return id;
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
                ushort id = __GetVariableIdCreateUnknownIfNotExists(variableName);

                foreach (VariableSubscriber s in _dbSubscribers)
                {
                    if (s.VariableId == id &&
                        s.ClientRef == clientRef &&
                        s.Periodicity == periodicity)
                    {
                        _dbSubscribers.Remove(s);
                        break;
                    }
                }

                return id;
            }
        }

        /// <summary>
        /// Gets variable's ID for provided name,
        /// creates variable of unknown type if the name does not exist.
        /// </summary>
        /// <param name="variableName">Name of the variable.</param>
        /// <returns>ID of the variable.</returns>
        private ushort __GetVariableIdCreateUnknownIfNotExists(string variableName)
        {
            if (_dbVariableIds.ContainsKey(variableName))
            {
                return _dbVariableIds[variableName];
            }

            ushort id = IdGenerator.GetNextVariableId();

            _dbVariableIds.Add(variableName, id);
            _dbVariables.Add(id, new UnknownVariable(id, variableName));

            return id;
        }
        
        


        #endregion
        #region Getting a variable - accessing a variable from outside the partial-class




        /// <summary>
        /// Returns an existing variable with specified <c>ID</c>.
        /// Throws exception otherwise.
        /// </summary>
        /// <param name="id">ID of the variable.</param>
        /// <returns>The designated variable.</returns>
        /// <exception cref="Exception"></exception>
        private BaseVariable GetVariableWithId(ushort id)
        {
            lock (_dbMutex)
            {
                if (_dbVariables.ContainsKey(id))
                {
                    return _dbVariables[id];
                }
                else
                {
                    throw new Exception($"Variable with ID {id} does not exist");
                }
            }
        }




        #endregion
        #region Updating variable values




        /// <summary>
        /// Assigns variable with value read from buffer.
        /// </summary>
        /// <param name="sender">Data sender's address.</param>
        /// <param name="variable">The variable to be assigned.</param>
        /// <param name="data">Data buffer.</param>
        /// <param name="offset">Offset from which data is to be read in the data buffer.</param>
        /// <param name="errorMessage">Error message when value is not assigned.</param>
        /// <returns>True = value is changed, False = variable's last value is retained.</returns>
        /// <exception cref="Exception"></exception>
        private bool AssignVariableWithValue(
            string sender,
            BaseVariable variable,
            byte[] data,
            ref int offset,
            out string errorMessage)
        {
            VariableType readVariableType = data.ReadVariableType(ref offset);

            __ThrowIfVariableTypeIncompatible(variable, readVariableType);

            if (variable.VariableType == VariableType.Compound)
            {
                variable.LastUpdatedAt = DateTime.Now;

                errorMessage = string.Empty;
                return true;
            }
            else if (variable.VariableType == VariableType.Primitive)
            {
                //- TODO
                throw new NotImplementedException();
            }
            else if (variable.VariableType == VariableType.UnknownVariableType &&
                     readVariableType != VariableType.UnknownVariableType)
            {
                //- TODO
                throw new NotImplementedException();
            }

            throw new Exception(
                $"Cannot assign {readVariableType} to " +
                $"local variable ({variable.Name}) of type {variable.VariableType}");
        }

        /// <summary>
        /// Checks for variable type compatibility, throws error if they are incompatible.
        /// </summary>
        /// <param name="variable">Variable that needs to be checked for compatibility.</param>
        /// <param name="variableType">Type against which variable needs to be checked.</param>
        /// <exception cref="Exception"></exception>
        private static void __ThrowIfVariableTypeIncompatible(BaseVariable variable, VariableType variableType)
        {
            if (variable.VariableType == variableType) return;

            if (variable.VariableType == VariableType.Compound &&
                variableType != VariableType.Compound)
            {
                throw new Exception(
                    $"Variable {variable.Name} " +
                    $"is of type {VariableType.Compound} " +
                    $"and is incompatible with given type {variableType}");
            }

            if (variable.VariableType == VariableType.Primitive &&
                variableType != VariableType.Primitive)
            {
                throw new Exception(
                    $"Variable {variable.Name} " +
                    $"is of type {VariableType.Primitive} " +
                    $"and is incompatible with given type {variableType}");
            }
        }




        #endregion
        #region Sending updated variables to their subscribers




        /// <summary>
        /// Sends updated variables to registered clients.
        /// </summary>
        /// <param name="updatedVariables">List of updated variables.</param>
        /// <param name="periodicity">Only select clients that have registered for specified periodicity.</param>
        private void SendUpdatedVariables(
            List<BaseVariable> updatedVariables,
            Periodicity periodicity = Periodicity.OnChange)
        {
            lock (_dbMutex)
            {
                List<BaseVariable> varsToBeSent = new();

                foreach (string clientRef in __GetUniqueSubscribers(updatedVariables))
                {
                    varsToBeSent.Clear();

                    int bufferSize = 0;

                    foreach (BaseVariable v in __GetVariablesForSubscriberWithPeriodicity(
                                                        updatedVariables,
                                                        clientRef,
                                                        periodicity))
                    {
                        varsToBeSent.Add(v);
                        bufferSize += v.GetSizeOnBuffer();
                    }

                    if (bufferSize > 0)
                    {
                        bufferSize += PacketId.VariablesUpdateFromServer.GetSizeOnBuffer();
                        bufferSize += periodicity.GetSizeOnBuffer();

                        byte[] buffer = new byte[bufferSize];
                        int bufferOffset = 0;

                        buffer.WritePacketId(ref bufferOffset, PacketId.VariablesUpdateFromServer);
                        buffer.WritePeriodicity(ref bufferOffset, periodicity);

                        foreach (BaseVariable v in varsToBeSent)
                        {
                            v.WriteOnBuffer(ref buffer, ref bufferOffset);
                        }

                        OutputQueue.Enqueue(new DataToClient(clientRef, buffer));
                    }
                }
            }
        }


        /// <summary>
        /// Sends variables with selected periodicity to their subscribers.
        /// </summary>
        /// <param name="periodicity"></param>
        private void DoPeriodicUpdate(Periodicity periodicity)
        {
            lock (_dbMutex)
            {
                List<BaseVariable> varsToBeSent = new();

                foreach (string clientRef in __GetUniqueSubscribers())
                {
                    varsToBeSent.Clear();

                    int bufferSize = 0;

                    foreach (BaseVariable v in __GetVariablesForSubscriberWithPeriodicity(clientRef, periodicity))
                    {
                        varsToBeSent.Add(v);
                        bufferSize += v.GetSizeOnBuffer();
                    }

                    if (bufferSize > 0)
                    {
                        bufferSize += PacketId.VariablesUpdateFromServer.GetSizeOnBuffer();
                        bufferSize += periodicity.GetSizeOnBuffer();

                        byte[] buffer = new byte[bufferSize];
                        int bufferOffset = 0;

                        buffer.WritePacketId(ref bufferOffset, PacketId.VariablesUpdateFromServer);
                        buffer.WritePeriodicity(ref bufferOffset, periodicity);

                        foreach (BaseVariable v in varsToBeSent)
                        {
                            v.WriteOnBuffer(ref buffer, ref bufferOffset);
                        }

                        OutputQueue.Enqueue(new DataToClient(clientRef, buffer));
                    }
                }
            }
        }

        /// <summary>
        /// Returns unique client addresses - i.e., unique subscribers.
        /// </summary>
        /// <returns>Uniquely identified subscribers.</returns>
        private IEnumerable<string> __GetUniqueSubscribers()
        {
            List<string> uniqueClientRefs = new();

            foreach (VariableSubscriber s in _dbSubscribers)
            {
                bool exists = false;

                foreach (string clientRef in uniqueClientRefs)
                {
                    if (s.ClientRef == clientRef)
                    {
                        exists = true;
                    }
                }

                if (!exists)
                {
                    uniqueClientRefs.Add(s.ClientRef);
                    yield return s.ClientRef;
                }
            }
        }
        /// <summary>
        /// Returns unique client addresses gainst given variables.
        /// </summary>
        /// <param name="variables">Variables' list.</param>
        /// <returns>Uniquely identified subscribers.</returns>
        private IEnumerable<string> __GetUniqueSubscribers(List<BaseVariable> variables)
        {
            List<string> uniqueClientRefs = new();

            foreach (BaseVariable v in variables)
            {
                foreach (VariableSubscriber s in _dbSubscribers)
                {
                    if (s.VariableId == v.Id)
                    {
                        bool exists = false;

                        foreach (string clientRef in uniqueClientRefs)
                        {
                            if (s.ClientRef == clientRef)
                            {
                                exists = true;
                            }
                        }

                        if (!exists)
                        {
                            uniqueClientRefs.Add(s.ClientRef);
                            yield return s.ClientRef;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Returns variables for the client with specified periodicity.
        /// </summary>
        /// <returns>Variables' list.</returns>
        private IEnumerable<BaseVariable>
            __GetVariablesForSubscriberWithPeriodicity(string clientRef, Periodicity periodicity)
        {
            foreach (VariableSubscriber s in _dbSubscribers)
            {
                if (s.ClientRef == clientRef && s.Periodicity == periodicity)
                {
                    yield return _dbVariables[s.VariableId];
                }
            }
        }
        /// <summary>
        /// Returns variables for the client with specified periodicity from specified list.
        /// </summary>
        /// <returns>Variables' list.</returns>
        private IEnumerable<BaseVariable>
            __GetVariablesForSubscriberWithPeriodicity(
                List<BaseVariable> variables,
                string clientRef,
                Periodicity periodicity)
        {
            foreach (BaseVariable v in variables)
            {
                foreach (VariableSubscriber s in _dbSubscribers)
                {
                    if (s.ClientRef == clientRef &&
                        s.Periodicity == periodicity &&
                        s.VariableId == v.Id)
                    {
                        yield return v;
                    }
                }
            }
        }




        #endregion
    }
}
