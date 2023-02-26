using DDS.Net.Server.Core.Internal.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDS.Net.Server.Core.Internal.Interfaces
{
    internal interface IThreadedDataIO
    {
        event EventHandler<ThreadedDataIOStatus>? ThreadedDataIOStatusChanged;
    }
}
