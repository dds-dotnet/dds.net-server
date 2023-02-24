using DDS.Net.Server.Interfaces;
using System;

namespace DDS.Net.Server.WpfApp.Interfaces.Logger
{
    internal class SplitLogger : ILogger, IDisposable
    {
        private readonly ILogger? _logger01;
        private readonly ILogger? _logger02;
        private readonly ILogger? _logger03;
        private readonly ILogger? _logger04;
        private readonly ILogger? _logger05;

        public SplitLogger(ILogger logger01, ILogger logger02)
        {
            _logger01 = logger01;
            _logger02 = logger02;
            _logger03 = null;
            _logger04 = null;
            _logger05 = null;
        }

        public SplitLogger(ILogger logger01, ILogger logger02, ILogger logger03)
        {
            _logger01 = logger01;
            _logger02 = logger02;
            _logger03 = logger03;
            _logger04 = null;
            _logger05 = null;
        }

        public SplitLogger(ILogger logger01, ILogger logger02, ILogger logger03, ILogger logger04)
        {
            _logger01 = logger01;
            _logger02 = logger02;
            _logger03 = logger03;
            _logger04 = logger04;
            _logger05 = null;
        }

        public SplitLogger(ILogger logger01, ILogger logger02, ILogger logger03, ILogger logger04, ILogger logger05)
        {
            _logger01 = logger01;
            _logger02 = logger02;
            _logger03 = logger03;
            _logger04 = logger04;
            _logger05 = logger05;
        }

        public void Dispose()
        {
            if (_logger01 != null && _logger01 is IDisposable disposable01) disposable01.Dispose();
            if (_logger02 != null && _logger02 is IDisposable disposable02) disposable02.Dispose();
            if (_logger03 != null && _logger03 is IDisposable disposable03) disposable03.Dispose();
            if (_logger04 != null && _logger04 is IDisposable disposable04) disposable04.Dispose();
            if (_logger05 != null && _logger05 is IDisposable disposable05) disposable05.Dispose();
        }

        public void Error(string message)
        {
            _logger01?.Error(message);
            _logger02?.Error(message);
            _logger03?.Error(message);
            _logger04?.Error(message);
            _logger05?.Error(message);
        }

        public void Info(string message)
        {
            _logger01?.Info(message);
            _logger02?.Info(message);
            _logger03?.Info(message);
            _logger04?.Info(message);
            _logger05?.Info(message);
        }

        public void Warning(string message)
        {
            _logger01?.Warning(message);
            _logger02?.Warning(message);
            _logger03?.Warning(message);
            _logger04?.Warning(message);
            _logger05?.Warning(message);
        }
    }
}
