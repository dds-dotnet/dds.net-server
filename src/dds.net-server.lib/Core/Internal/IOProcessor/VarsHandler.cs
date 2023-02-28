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
        : SinglePipedThreadConsumer<DataFromClient, DataToClient, VarsHandlerCommands, VarsHandlerStatus>
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

        protected override void DoInit()
        {
        }

        protected override void DoWork()
        {
        }

        protected override void DoCleanup()
        {
        }

        protected override void ProcessCommand(VarsHandlerCommands command)
        {
        }

        protected override void ProcessInput(DataFromClient input)
        {
        }

        public override void Dispose()
        {
        }
    }
}
