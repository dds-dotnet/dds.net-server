using DDS.Net.Server.Core.Internal.Base;
using DDS.Net.Server.Core.Internal.Base.Entities;

namespace DDS.Net.Server.Core.Internal.IOProcessor
{
    internal partial class VariablesDatabase
        : SinglePipedConsumer<DataFromClient, DataToClient, VarsDbCommand, VarsDbStatus>
    {
        private void InitializeDatabase()
        {

        }

        private void ClearDatabase()
        {

        }
    }
}
