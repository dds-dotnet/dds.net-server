namespace DDS.Net.Server.Core.Internal.Base
{
    internal abstract class PipedThreadBase
    {
        protected PipedThreadBase()
        {
        }

        protected abstract void ProcessingLoop();
    }
}
