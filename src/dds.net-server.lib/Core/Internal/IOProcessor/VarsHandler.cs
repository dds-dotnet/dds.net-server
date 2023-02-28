using DDS.Net.Server.Core.Internal.Base.Entities;
using DDS.Net.Server.Core.Internal.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDS.Net.Server.Core.Internal.Interfaces;

namespace DDS.Net.Server.Core.Internal.IOProcessor
{
    internal class VarsHandler
        : SingleConsumerPipedThread<DataFromClient, DataToClient, VarsHandlerCommands, VarsHandlerStatus>
    {
        public VarsHandler(
                    ISyncQueueReaderEnd<DataFromClient> dataReaderEnd,
                    ISyncQueueWriterEnd<DataToClient> dataWriterEnd,
                    int commandsQueueSize,
                    int responsesQueueSize)

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

        public override void Dispose()
        {
            throw new NotImplementedException();
        }

        protected override void DoCleanup()
        {
            throw new NotImplementedException();
        }

        protected override void DoInit()
        {
            throw new NotImplementedException();
        }

        protected override void DoWork()
        {
            throw new NotImplementedException();
        }

        protected override void ProcessCommand(VarsHandlerCommands command)
        {
            throw new NotImplementedException();
        }

        protected override void ProcessInput(DataFromClient input)
        {
            throw new NotImplementedException();
        }
    }
}
