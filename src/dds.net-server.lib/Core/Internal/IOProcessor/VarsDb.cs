using DDS.Net.Server.Core.Internal.Base.Entities;
using DDS.Net.Server.Core.Internal.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDS.Net.Server.Core.Internal.Interfaces;
using DDS.Net.Server.Interfaces;

namespace DDS.Net.Server.Core.Internal.IOProcessor
{
    internal class VarsDb
        : SinglePipedConsumer<DataFromClient, DataToClient, VarsDbCommand, VarsDbStatus>
    {
        public VarsDb(
                    ISyncQueueReaderEnd<DataFromClient> dataReaderEnd,
                    ISyncQueueWriterEnd<DataToClient> dataWriterEnd,
                    int commandsQueueSize,
                    int responsesQueueSize,
                    ILogger logger)

            : base(dataReaderEnd, dataWriterEnd, commandsQueueSize, responsesQueueSize)
        {
            
        }

        public void StartHandler()
        {
            StartThread();
        }

        public void StopHandler()
        {
            Exit();
        }

        protected override int DoInit()
        {
            return 0;
        }

        protected override int DoWork()
        {
            return 0;
        }

        protected override int DoCleanup()
        {
            return 0;
        }

        protected override int ProcessCommand(VarsDbCommand command)
        {
            return 0;
        }

        protected override int ProcessInput(DataFromClient input)
        {
            return 0;
        }

        public override void Dispose()
        {
        }
    }
}
