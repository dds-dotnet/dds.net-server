﻿using DDS.Net.Server.Core.Internal;
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
        private readonly ServerConfiguration _config;
        private readonly ILogger _logger;

        private ServerStatus _status = ServerStatus.Stopped;

        private ISyncQueueReaderEnd<DataFromClient> _dataFromNetwork = null!;
        private ISyncQueueWriterEnd<DataToClient> _dataToNetwork = null!;

        private NetworkIO? _networkIO;
        private VarsHandler? _varsHandler;

        public DdsServer(ServerConfiguration config)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));

            _config = config;

            if (_config.Logger != null)
                _logger = _config.Logger;
            else
                throw new Exception($"No instance of {nameof(ILogger)} is provided");

            _networkIO = null;
            _varsHandler = null;
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
                        _networkIO = new NetworkIO(
                            _logger,

                            InternalSettings.NETWORK_DATA_TO_CLIENTS_QUEUE_SIZE,
                            InternalSettings.NETWORK_DATA_FROM_CLIENTS_QUEUE_SIZE,
                            InternalSettings.NETWORK_COMMANDS_QUEUE_SIZE,
                            InternalSettings.NETWORK_RESPONSES_QUEUE_SIZE,


                            _config.ListeningAddressIPv4,

                            _config.EnableTCP, _config.ListeningPortTCP, _config.MaxClientsTCP,
                            _config.EnableUDP, _config.ListeningPortUDP);

                        _dataFromNetwork = _networkIO.Output;
                        _dataToNetwork = _networkIO.Input;

                        _networkIO.Responses.DataAvailableForReading += OnNetworkIOStatusChanged;


                        _varsHandler = new VarsHandler(
                            _dataFromNetwork,
                            _dataToNetwork,
                            InternalSettings.VARS_HANDLER_COMMANDS_QUEUE_SIZE,
                            InternalSettings.VARS_HANDLER_RESPONSES_QUEUE_SIZE);

                        _varsHandler.Responses.DataAvailableForReading += OnVarsHandlerStatusChanged;


                        _networkIO.StartIO();
                        _varsHandler.StartHandler();
                    }
                    catch (Exception ex)
                    {
                        _networkIO = null;
                        _varsHandler = null;

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

        private void OnVarsHandlerStatusChanged(object? sender, VarsHandlerStatus e)
        {
        }

        private void OnNetworkIOStatusChanged(object? sender, DataIOProviderStatus e)
        {
            switch (e)
            {
                case DataIOProviderStatus.Stopped:
                    SetServerStatus(ServerStatus.Stopped);
                    _networkIO = null;
                    break;

                case DataIOProviderStatus.Starting:
                    SetServerStatus(ServerStatus.Starting);
                    break;

                case DataIOProviderStatus.Started:
                    SetServerStatus(ServerStatus.Started);
                    break;

                case DataIOProviderStatus.Stopping:
                    SetServerStatus(ServerStatus.Stopping);
                    break;

                case DataIOProviderStatus.Paused:
                    break;
            }
        }

        public void Stop()
        {
            if (_status == ServerStatus.Started)
            {
                SetServerStatus(ServerStatus.Stopping);

                PrintLogStopping();

                if (_varsHandler != null)
                {
                    _varsHandler.StopHandler();
                    _varsHandler = null;
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
