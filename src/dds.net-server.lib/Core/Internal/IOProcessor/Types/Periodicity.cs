namespace DDS.Net.Server.Core.Internal.IOProcessor.Types
{
    internal enum Periodicity
    {
        /// <summary>
        /// Updates only when changed
        /// </summary>
        OnChange = 0,
        /// <summary>
        /// Updates every { Settings.BASE_TIME_SLOT_MS * 1 } milliseconds
        /// </summary>
        Highest,
        /// <summary>
        /// Updates every { Settings.BASE_TIME_SLOT_MS * 2 } milliseconds
        /// </summary>
        High,
        /// <summary>
        /// Updates every { Settings.BASE_TIME_SLOT_MS * 4 } milliseconds
        /// </summary>
        Normal,
        /// <summary>
        /// Updates every { Settings.BASE_TIME_SLOT_MS * 8 } milliseconds
        /// </summary>
        Low,
        /// <summary>
        /// Updates every { Settings.BASE_TIME_SLOT_MS * 16 } milliseconds
        /// </summary>
        Lowest
    }
}
