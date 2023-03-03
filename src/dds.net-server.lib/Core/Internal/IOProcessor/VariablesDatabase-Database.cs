using DDS.Net.Server.Core.Internal.Base;
using DDS.Net.Server.Core.Internal.Base.Entities;
using DDS.Net.Server.Core.Internal.IOProcessor.Types;

namespace DDS.Net.Server.Core.Internal.IOProcessor
{
    internal partial class VariablesDatabase
        : SinglePipedConsumer<DataFromClient, DataToClient, VarsDbCommand, VarsDbStatus>
    {
        private Dictionary<string, Variable> _primitivesDbDict = new();

        private void InitializeDatabase()
        {
        }

        private void ClearDatabase()
        {
            lock (_primitivesDbDict)
            {
                _primitivesDbDict.Clear();
            }
        }
    }
}
