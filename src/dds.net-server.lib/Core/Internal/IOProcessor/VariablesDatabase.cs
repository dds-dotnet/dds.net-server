using DDS.Net.Server.Core.Internal.Base;
using DDS.Net.Server.Core.Internal.Base.Entities;
using DDS.Net.Server.Core.Internal.Interfaces;
using DDS.Net.Server.Core.Internal.IOProcessor.EncodersAndDecoders;
using DDS.Net.Server.Core.Internal.IOProcessor.Types;
using DDS.Net.Server.Entities;
using DDS.Net.Server.Interfaces;

namespace DDS.Net.Server.Core.Internal.IOProcessor
{
    internal partial class VariablesDatabase
        : SinglePipedConsumer<DataFromClient, DataToClient, VarsDbCommand, VarsDbStatus>
    {
        private readonly VariablesConfiguration variablesConfiguration;
        private readonly ILogger logger;

        public VariablesDatabase(
                    ISyncQueueReaderEnd<DataFromClient> dataReaderEnd,
                    ISyncQueueWriterEnd<DataToClient> dataWriterEnd,
                    int commandsQueueSize,
                    int responsesQueueSize,
                    VariablesConfiguration variablesConfiguration,
                    ILogger logger)

            : base(dataReaderEnd, dataWriterEnd, commandsQueueSize, responsesQueueSize)
        {
            this.variablesConfiguration = variablesConfiguration;
            this.logger = logger;
        }

        /// <summary>
        /// Starts the variables' handling database processing thread.
        /// </summary>
        public void StartDatabase()
        {
            StartThread();
        }

        /// <summary>
        /// Stops the variables' handling database processing thread.
        /// </summary>
        public void StopDatabase()
        {
            Exit();
        }

        protected override int DoInit()
        {
            logger.Info("Starting Variables Database");

            InitializeDatabase();
            StartPeriodicUpdates();

            return 0;
        }

        protected override int DoWork()
        {
            //- 
            //- No need to do anything here as we are using Periodic Updates
            //- through timer to do periodic work, and packet parsing to do
            //- work related with communication.
            //- 

            return 0;
        }

        protected override int DoCleanup()
        {
            logger.Info("Ending Variables Database");

            StopPeriodicUpdates();
            ClearDatabase();

            return 0;
        }

        protected override int ProcessCommand(VarsDbCommand command)
        {
            return 0;
        }

        protected override int ProcessInput(DataFromClient input)
        {
            if (input != null && !string.IsNullOrEmpty(input.ClientRef))
            {
                if (input.Data == null)
                {
                    RemoveClient(input.ClientRef);
                }
                else
                {
                    int offset = 0;

                    try
                    {
                        PacketId pid = input.Data.ReadPacketId(ref offset);

                        switch (pid)
                        {
                            case PacketId.HandShake:
                                ProcessPacket_HandShake(input.ClientRef, input.Data, ref offset);
                                break;

                            case PacketId.VariablesRegistration:
                                ProcessPacket_VariablesRegistration(input.ClientRef, input.Data, ref offset);
                                break;

                            case PacketId.VariablesUpdateAtServer:
                                ProcessPacket_VariablesUpdateAtServer(input.ClientRef, input.Data, ref offset);
                                break;

                            case PacketId.VariablesUpdateFromServer:
                                logger.Warning($"Wrong packet \"VariablesUpdateFromServer\" received from {input.ClientRef}");
                                break;

                            case PacketId.ErrorResponseFromServer:
                                logger.Warning($"Wrong packet \"ErrorResponseFromServer\" received from {input.ClientRef}");
                                break;

                            default:
                                logger.Warning($"Unhandled packet \"{pid}\" received from {input.ClientRef}");
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Error($"Packet processing error: {ex.Message}");
                    }
                }

                return 1;
            }

            return 0;
        }

        public override void Dispose()
        {
        }
    }
}
