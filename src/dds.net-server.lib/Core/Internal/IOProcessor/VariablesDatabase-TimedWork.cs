using DDS.Net.Server.Core.Internal.Base;
using DDS.Net.Server.Core.Internal.Base.Entities;
using DDS.Net.Server.Core.Internal.IOProcessor.Types;

namespace DDS.Net.Server.Core.Internal.IOProcessor
{
    internal partial class VariablesDatabase
        : SinglePipedConsumer<DataFromClient, DataToClient, VarsDbCommand, VarsDbStatus>
    {
        private Timer _timer;
        private int _timedWorkCounter = 0;
        private volatile bool _isTimedWorkRunning = false;

        private void StartTimedWork()
        {
            if (_isTimedWorkRunning == false)
            {
                _timedWorkCounter = 0;
                _isTimedWorkRunning = true;

                try
                {
                    _timer = new Timer(TimerCallback);
                    _timer.Change(Settings.BASE_TIME_SLOT_MS, Timeout.Infinite);
                }
                catch (Exception ex)
                {
                    _timer = null!;
                    logger.Error($"Variables Database - Timed work cannot be executed: {ex.Message}");
                }
            }
        }

        private void StopTimedWork()
        {
            _isTimedWorkRunning = false;

            if (_timer != null)
            {
                _timer.Change(Timeout.Infinite, Timeout.Infinite);
                _timer.Dispose();
            }
        }

        private void TimerCallback(object? o)
        {
            DoTimedWork();

            if (_isTimedWorkRunning)
            {
                try
                {
                    _timer.Change(Settings.BASE_TIME_SLOT_MS, Timeout.Infinite);
                }
                catch (Exception) { }
            }
        }

        private void DoTimedWork()
        {
            _timedWorkCounter++;

            UpdateClients(Periodicity.Highest);

            if (_timedWorkCounter % 2 == 0) UpdateClients(Periodicity.High);
            if (_timedWorkCounter % 4 == 0) UpdateClients(Periodicity.Normal);
            if (_timedWorkCounter % 8 == 0) UpdateClients(Periodicity.Low);

            if (_timedWorkCounter % 16 == 0)
            {
                UpdateClients(Periodicity.Lowest);
                _timedWorkCounter = 0;
            }
        }

        private void UpdateClients(Periodicity periodicity)
        {
        }
    }
}
