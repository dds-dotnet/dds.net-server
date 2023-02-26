using DDS.Net.Server.Core.Internal.Entities;
using DDS.Net.Server.Core.Internal.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDS.Net.Server.Core.Internal.InterfaceImplementations
{
    internal class ThreadedNetworkIO : IThreadedDataIO
    {
        public event EventHandler<ThreadedDataIOStatus> ThreadedDataIOStatusChanged;
    }
}
