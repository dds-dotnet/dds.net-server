using DDS.Net.Server.Core.Internal;
using DDS.Net.Server.Core.Internal.Entities;
using DDS.Net.Server.Core.Internal.InterfaceImplementations;
using DDS.Net.Server.Core.Internal.Interfaces;
using DDS.Net.Server.Core.Internal.SimpleServer;
using DDS.Net.Server.Entities;
using DDS.Net.Server.Interfaces;

namespace DDS.Net.Server
{
    public enum ServerStatus
    {
        Stopped,
        Starting,
        Started,
        Stopping
    }

    public partial class DdsServer
    {
        private readonly ServerConfiguration _config;
        private readonly ILogger _logger;

        private ServerStatus _status = ServerStatus.Stopped;

        private SyncQueue<DataFromClient> _networkClientsInputQueue;
        private SyncQueue<DataToClient> _networkClientsOutputQueue;

        private ThreadedNetworkIO? _networkIO;

        public DdsServer(ServerConfiguration config)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));

            _config = config;

            if (_config.Logger != null)
                _logger = _config.Logger;
            else
                throw new Exception($"No instance of {nameof(ILogger)} is provided");

            _networkClientsInputQueue = new SyncQueue<DataFromClient>(InternalSettings.NETWORK_CLIENTS_INPUT_QUEUE_SIZE);
            _networkClientsOutputQueue = new SyncQueue<DataToClient>(InternalSettings.NETWORK_CLIENTS_OUTPUT_QUEUE_SIZE);

            _networkIO = null;
        }

        public void Start()
        {
            if (_status == ServerStatus.Stopped)
            {
                SetServerStatus(ServerStatus.Starting);

                PrintLogStarting();

                if (_networkIO == null &&
                    (_config.EnableTCP || _config.EnableUDP))
                {
                    try
                    {
                        _networkIO = new ThreadedNetworkIO(
                            _networkClientsOutputQueue,
                            _networkClientsInputQueue,

                            _logger,

                            _config.ListeningAddressIPv4,

                            _config.EnableTCP, _config.ListeningPortTCP, _config.MaxClientsTCP,
                            _config.EnableUDP, _config.ListeningPortUDP, _config.MaxClientsUDP);

                        _networkIO.ThreadedDataIOStatusChanged += OnNetworkIOStatusChanged;
                        _networkIO.StartIO();
                    }
                    catch (Exception ex)
                    {
                        _networkIO = null;
                        _logger.Error($"Cannot start NetworkIO: {ex.Message}");
                    }
                }

                if (_networkIO == null)
                {
                    SetServerStatus(ServerStatus.Stopped);
                }
            }
            else
            {
                _logger.Warning("Cannot start server when it is not fully stopped");
            }
        }

        private void OnNetworkIOStatusChanged(object? sender, ThreadedDataIOStatus e)
        {
            switch (e)
            {
                case ThreadedDataIOStatus.Stopped:
                    SetServerStatus(ServerStatus.Stopped);
                    _networkIO = null;
                    break;

                case ThreadedDataIOStatus.Starting:
                    SetServerStatus(ServerStatus.Starting);
                    break;

                case ThreadedDataIOStatus.Started:
                    SetServerStatus(ServerStatus.Started);
                    break;

                case ThreadedDataIOStatus.Stopping:
                    SetServerStatus(ServerStatus.Stopping);
                    break;

                case ThreadedDataIOStatus.Paused:
                    break;
            }
        }

        public void Stop()
        {
            if (_status == ServerStatus.Started)
            {
                SetServerStatus(ServerStatus.Stopping);

                PrintLogStopping();

                if (_networkIO != null)
                {
                    try
                    {
                        _networkIO.StopIO();
                    }
                    catch (Exception ex)
                    {
                        _logger.Error($"NetworkIO reported error on stopping: {ex.Message}");
                    }

                    _networkIO = null;
                }

                SetServerStatus(ServerStatus.Stopped);
            }
            else
            {
                _logger.Warning("Cannot stop server when it is not fully started");
            }
        }

        private void SetServerStatus(ServerStatus newStatus)
        {
            if (_status != newStatus)
            {
                _status = newStatus;
                ServerStatusChanged?.Invoke(this, _status);
            }
        }
    }
}
