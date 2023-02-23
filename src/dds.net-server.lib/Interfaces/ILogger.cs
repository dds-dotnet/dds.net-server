using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDS.Net.Server.Interfaces
{
    internal interface ILogger
    {
        void Info(string message);
        void Warning(string message);
        void Error(string message);
    }
}
