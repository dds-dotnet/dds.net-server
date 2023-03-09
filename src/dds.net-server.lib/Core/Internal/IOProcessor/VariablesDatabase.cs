using DDS.Net.Server.Core.Internal.Base.Entities;
using DDS.Net.Server.Core.Internal.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDS.Net.Server.Core.Internal.Interfaces;
using DDS.Net.Server.Interfaces;
using DDS.Net.Server.Entities;

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

        public void StartDatabase()
        {
            StartThread();
        }

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
