using System;
using System.Diagnostics;
using System.Timers;

namespace OgameBot.Tasks
{
    public abstract class TaskBase
    {
        private readonly Timer _timer;
        private bool _isRunning;
        public TimeSpan ExecutionInterval { get; set; }


        public TaskBase()
        {
            _timer = new Timer();
            _timer.Elapsed += TimerOnElapsed;
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            _timer.Stop();

            if (_isRunning)
            {
                // Run task
                try
                {
                    RunInternal();
                }
                catch (Exception)
                {
                    if (Debugger.IsAttached)
                        throw;
                }
            }

            if (_isRunning)
            {
                // Restart timer
                _timer.Interval = ExecutionInterval.TotalMilliseconds;
                _timer.Start();
            }
        }

        protected abstract void RunInternal();

        public void Start()
        {
            _timer.Stop();

            _isRunning = true;

            // Restart timer
            _timer.Interval = ExecutionInterval.TotalMilliseconds;
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
            _isRunning = false;
        }
    }
}