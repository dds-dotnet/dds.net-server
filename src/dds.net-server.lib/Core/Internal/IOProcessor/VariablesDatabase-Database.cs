using DDS.Net.Server.Core.Internal.Base;
using DDS.Net.Server.Core.Internal.Base.Entities;
using DDS.Net.Server.Core.Internal.IOProcessor.Types;

namespace DDS.Net.Server.Core.Internal.IOProcessor
{
    internal partial class VariablesDatabase
        : SinglePipedConsumer<DataFromClient, DataToClient, VarsDbCommand, VarsDbStatus>
    {
        private Dictionary<string, Variable<ulong>>  _usignPrimDbDict = new();
        private Dictionary<string, Variable<long>>   _signPrimDbDict = new();
        private Dictionary<string, Variable<double>> _fpPrimDbDict = new();

        private void InitializeDatabase()
        {
        }

        private void ClearDatabase()
        {
            lock (_usignPrimDbDict)
            {
                _usignPrimDbDict.Clear();
            }

            lock (_signPrimDbDict)
            {
                _signPrimDbDict.Clear();
            }

            lock (_fpPrimDbDict)
            {
                _fpPrimDbDict.Clear();
            }
        }
    }
}
