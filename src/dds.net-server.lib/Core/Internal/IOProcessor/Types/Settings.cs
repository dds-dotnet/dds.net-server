namespace DDS.Net.Server.Core.Internal.IOProcessor.Types
{
    internal class Settings
    {
        public static readonly int BASE_TIME_SLOT_MS = 50;

        public static readonly int MAX_VARIABLE_NAME_LENGTH = 127;
        public static readonly int MAX_STRING_VARIABLES_PER_MESSAGE = 10;
        public static readonly int MAX_PRIMITIVE_VARIABLES_PER_MESSAGE = 100;
    }
}
