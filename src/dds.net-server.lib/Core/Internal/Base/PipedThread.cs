using DDS.Net.Server.Core.Internal.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDS.Net.Server.Core.Internal.Base
{
    internal class PipedThread<T_Input, T_Output, T_Commands, T_Responses>
        : IDisposable
        where T_Input : class
        where T_Output : class
        where T_Commands : struct
        where T_Responses : struct
    {
        public ISyncDataInputQueueEnd<T_Input> InputQueueEnd { get; private set; }
        public ISyncDataOutputQueueEnd<T_Output> OutputQueueEnd { get; private set; }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
