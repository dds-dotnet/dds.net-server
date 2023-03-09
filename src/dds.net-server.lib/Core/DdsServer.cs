using DDS.Net.Server.Core.Internal;
using DDS.Net.Server.Core.Internal.Base.Entities;
using DDS.Net.Server.Core.Internal.Interfaces;
using DDS.Net.Server.Core.Internal.IOProcessor;
using DDS.Net.Server.Core.Internal.IOProviders;
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
        private readonly ServerConfiguration _serverConfig;
        private readonly VariablesConfiguration _variablesConfig;
        private readonly ILogger _logger;

        private ServerStatus _status = ServerStatus.Stopped;

        private ISyncQueueReaderEnd<DataFromClient> _dataFromNetwork = null!;
        private ISyncQueueWriterEnd<DataToClient> _dataToNetwork = null!;

        private NetworkIO? _networkIO;
        private VariablesDatabase? _varsDatabase;

        public DdsServer(ServerConfiguration config, VariablesConfiguration variablesConfig)
        {
            _serverConfig = config ?? throw new ArgumentNullException(nameof(config));
            _variablesConfig = variablesConfig ?? throw new ArgumentNullException(nameof(variablesConfig));

            if (_serverConfig.Logger != null)
                _logger = _serverConfig.Logger;
            else
                throw new Exception($"No instance of {nameof(ILogger)} is provided");

            _networkIO = null;
            _varsDatabase = null;
        }

        public void Start()
        {
            if (_status == ServerStatus.Stopped)
            {
                SetServerStatus(ServerStatus.Starting);

                PrintLogStarting();

                if (_networkIO == null &&
                    (_serverConfig.EnableTCP || _serverConfig.EnableUDP))
                {
                    try
                    {
                        _networkIO = new NetworkIO(
                            _logger,

                            QueueSizeSettings.NETWORK_DATA_TO_CLIENTS_QUEUE_SIZE,
                            QueueSizeSettings.NETWORK_DATA_FROM_CLIENTS_QUEUE_SIZE,
                            QueueSizeSettings.NETWORK_COMMANDS_QUEUE_SIZE,
                            QueueSizeSettings.NETWORK_RESPONSES_QUEUE_SIZE,


                            _serverConfig.ListeningAddressIPv4,

                            _serverConfig.EnableTCP, _serverConfig.ListeningPortTCP, _serverConfig.MaxClientsTCP,
                            _serverConfig.EnableUDP, _serverConfig.ListeningPortUDP);

                        _dataFromNetwork = _networkIO.OutputReader;
                        _dataToNetwork = _networkIO.InputWriter;

                        _networkIO.ResponseReader.DataAvailableForReading += OnNetworkIOStatusChanged;


                        _varsDatabase = new VariablesDatabase(
                            _dataFromNetwork,
                            _dataToNetwork,
                            QueueSizeSettings.VARS_HANDLER_COMMANDS_QUEUE_SIZE,
                            QueueSizeSettings.VARS_HANDLER_RESPONSES_QUEUE_SIZE,
                            _variablesConfig,
                            _logger);

                        _varsDatabase.ResponseReader.DataAvailableForReading += OnVarsHandlerStatusChanged;


                        _networkIO.StartIO();
                        _varsDatabase.StartDatabase();
                    }
                    catch (Exception ex)
                    {
                        _networkIO = null;
                        _varsDatabase = null;

                        _logger.Error($"Cannot start NetworkIO and VarsHandler: {ex.Message}");
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

        private void OnVarsHandlerStatusChanged(object? sender, VarsDbStatus e)
        {
        }

        private void OnNetworkIOStatusChanged(object? sender, DataIOProviderStatus e)
        {
            switch (e.Status)
            {
                case DataIOProviderRunningStatus.Stopped:
                    SetServerStatus(ServerStatus.Stopped);
                    _networkIO = null;
                    break;

                case DataIOProviderRunningStatus.Starting:
                    SetServerStatus(ServerStatus.Starting);
                    break;

                case DataIOProviderRunningStatus.Started:
                    SetServerStatus(ServerStatus.Started);
                    break;

                case DataIOProviderRunningStatus.Stopping:
                    SetServerStatus(ServerStatus.Stopping);
                    break;

                case DataIOProviderRunningStatus.Paused:
                    break;
            }
        }

        public void Stop()
        {
            if (_status == ServerStatus.Started)
            {
                SetServerStatus(ServerStatus.Stopping);

                PrintLogStopping();

                if (_varsDatabase != null)
                {
                    _varsDatabase.StopDatabase();
                    _varsDatabase = null;
                }

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
