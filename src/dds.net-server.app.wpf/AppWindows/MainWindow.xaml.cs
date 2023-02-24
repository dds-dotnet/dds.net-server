using DDS.Net.Server.Entities;
using DDS.Net.Server.Helpers;
using DDS.Net.Server.Interfaces;
using DDS.Net.Server.WpfApp.Configuration;
using DDS.Net.Server.WpfApp.Interfaces.Logger;
﻿using System;
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
        private ILogger _logger;
        public MainWindow()
        {
            InitializeComponent();

            _logger = new FileLogger(Constants.LOG_FILENAME);
        }
    }
}
