using DDS.Net.Server.Core.Internal.Base;
using DDS.Net.Server.Core.Internal.Base.Entities;
using DDS.Net.Server.Core.Internal.IOProcessor.Types;

namespace DDS.Net.Server.Core.Internal.IOProcessor
{
    internal partial class VariablesDatabase
        : SinglePipedConsumer<DataFromClient, DataToClient, VarsDbCommand, VarsDbStatus>
    {
        private Timer _periodicUpdatesTimer = null!;
        private int _periodicUpdatesCounter = 0;
        private volatile bool _isPeriodicUpdatesTimerRunning = false;

        /// <summary>
        /// Starts periodic updates of variables to their subscribers.
        /// </summary>
        private void StartPeriodicUpdates()
        {
            if (_isPeriodicUpdatesTimerRunning == false)
            {
                _periodicUpdatesCounter = 0;
                _isPeriodicUpdatesTimerRunning = true;

                try
                {
                    _periodicUpdatesTimer = new Timer(PeriodicUpdatesTimerCallback);
                    _periodicUpdatesTimer.Change(Settings.BASE_TIME_SLOT_MS, Timeout.Infinite);
                }
                catch (Exception ex)
                {
                    _periodicUpdatesTimer = null!;
                    logger.Error($"Variables Database - periodic updates cannot be executed: {ex.Message}");
                }
            }
        }
        /// <summary>
        /// Stops periodic updates of variables to their subscribers.
        /// </summary>
        private void StopPeriodicUpdates()
        {
            _isPeriodicUpdatesTimerRunning = false;

            if (_periodicUpdatesTimer != null)
            {
                _periodicUpdatesTimer.Change(Timeout.Infinite, Timeout.Infinite);
                _periodicUpdatesTimer.Dispose();
            }
        }
        /// <summary>
        /// The timer callback, invoked at every Settings.BASE_TIME_SLOT_MS milliseconds
        /// </summary>
        /// <param name="o">Unused object.</param>
        private void PeriodicUpdatesTimerCallback(object? o)
        {
            DecidePeriodicUpdates();

            if (_isPeriodicUpdatesTimerRunning)
            {
                try
                {
                    _periodicUpdatesTimer.Change(Settings.BASE_TIME_SLOT_MS, Timeout.Infinite);
                }
                catch (Exception) { }
            }
        }
        /// <summary>
        /// Decides upon which Periodicity of variables should be sent to their subscribers.
        /// </summary>
        private void DecidePeriodicUpdates()
        {
            _periodicUpdatesCounter++;

            DoPeriodicUpdate(Periodicity.Highest);

            if (_periodicUpdatesCounter % 2 == 0) DoPeriodicUpdate(Periodicity.High);
            if (_periodicUpdatesCounter % 4 == 0) DoPeriodicUpdate(Periodicity.Normal);
            if (_periodicUpdatesCounter % 8 == 0) DoPeriodicUpdate(Periodicity.Low);

            if (_periodicUpdatesCounter % 16 == 0)
            {
                DoPeriodicUpdate(Periodicity.Lowest);
                _periodicUpdatesCounter = 0;
            }
        }
        /// <summary>
        /// Sends variables with selected periodicity to their subscribers.
        /// </summary>
        /// <param name="periodicity"></param>
        private void DoPeriodicUpdate(Periodicity periodicity)
        {
        }
    }
}
