using DDS.Net.Server.Core.Internal.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDS.Net.Server.Core.Internal.Base
{
    internal abstract class SinglePipedThread<T_Input, T_Output, T_Commands, T_Responses> : PipedThreadBase, IDisposable
        where T_Input : class
        where T_Output : class
        where T_Commands : struct
        where T_Responses : struct
    {
        public ISyncDataInputQueueEnd<T_Input> QueuedInput { get; private set; }
        public ISyncDataOutputQueueEnd<T_Output> QueuedOutput { get; private set; }

        public void Dispose()
        {
        }
    }
}
