using DDS.Net.Server.Entities;
using DDS.Net.Server.Helpers;
using DDS.Net.Server.Interfaces;
using DDS.Net.Server.WpfApp.Configuration;
using DDS.Net.Server.WpfApp.Interfaces.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DDS.Net.Server.WpfApp.AppWindows
{
    public partial class MainWindow : Window
    {
        private FileLogger? _logger;
        private DdsServer? _server;

        public MainWindow()
        {
            InitializeComponent();

            _logger = new FileLogger(AppConstants.LOG_FILENAME);

            (bool isEnabled, ServerConfiguration? config) =
                ConfigurationProvider.GetServerConfiguration(AppConstants.SERVER_01_CONFIG_FILENAME, _logger);

            if (isEnabled && config != null)
            {
                _server = new DdsServer(config);
                _server.Start();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _logger?.Dispose();
            _logger = null;
        }
    }
}
