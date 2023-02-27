using DDS.Net.Server.Core.Internal.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDS.Net.Server.Core.Internal.Base
{
    internal abstract class DoublePipedThread<T_Input1, T_Output1, T_Input2, T_Output2, T_Commands, T_Responses>
        : PipedThreadBase, IDisposable

        where T_Input1 : class
        where T_Input2 : class
        where T_Output1 : class
        where T_Output2 : class
        where T_Commands : struct
        where T_Responses : struct
    {
        public ISyncDataInputQueueEnd<T_Input1> QueuedInput1 { get; private set; }
        public ISyncDataOutputQueueEnd<T_Output1> QueuedOutput1 { get; private set; }

        public ISyncDataInputQueueEnd<T_Input2> QueuedInput2 { get; private set; }
        public ISyncDataOutputQueueEnd<T_Output2> QueuedOutput2 { get; private set; }

        public ISyncDataInputQueueEnd<T_Commands> QueuedCommands { get; private set; }
        public ISyncDataOutputQueueEnd<T_Responses> QueuedResponses { get; private set; }

        public void Dispose()
        {
        }
    }
}
