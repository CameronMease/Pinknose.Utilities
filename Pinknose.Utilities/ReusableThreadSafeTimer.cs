using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

namespace Pinknose.Utilities
{
    /// <summary>
    /// A timer that can be reset, stopped and started.  It will not dispose itself when it is stopped.
    /// </summary>
    public class ReusableThreadSafeTimer : IDisposable
    {
        private class PvtTimer : Timer
        {
            public PvtTimer() : base()
            {
                this.Disposed += PvtTimer_Disposed;
                IsDisposed = false;
                IsDisposing = false;
            }

            /*
            public PvtTimer(double interval) : base(interval)
            {
                this.Disposed += PvtTimer_Disposed;
                IsDisposed = false;
                IsDisposing = false;
            }
            */

            private void PvtTimer_Disposed(object sender, EventArgs e)
            {
                IsDisposed = true;
            }

            protected override void Dispose(bool disposing)
            {
                IsDisposing = true;
                base.Dispose(disposing);
            }

            public bool IsDisposing { get; private set; }
            public bool IsDisposed { get; private set; }
        }

        private object lockObject = new object();
        private PvtTimer _timer = null;

        public ReusableThreadSafeTimer()
        {

        }

        public ReusableThreadSafeTimer(double interval)
        {
            Interval = interval;
        }

        public ReusableThreadSafeTimer(TimeSpan interval)
        {
            Interval = interval.TotalMilliseconds;
        }

        bool autoReset;
        public bool AutoReset
        {
            get
            {
                return autoReset;
            }
            set
            {
                lock (lockObject)
                {
                    autoReset = value;
                    GetTimer().AutoReset = value;
                }
            }
        }

        bool enabled;
        public bool Enabled
        {
            get
            {
                return enabled;
            }
            set
            {
                lock (lockObject)
                {
                    if (enabled != value)
                    {
                        enabled = value;
                        GetTimer().Enabled = value;
                    }
                }
            }
        }

        double interval = 1;
        public double Interval
        {
            get
            {
                return interval;
            }
            set
            {
                lock (lockObject)
                {
                    interval = value;
                    GetTimer().Interval = value;
                }
            }
        }

        public object Tag { get; set; }

        public event ElapsedEventHandler Elapsed;

        public void Start()
        {
            lock (lockObject)
            {
                PvtTimer timer = GetTimer();
                timer.Start();
                Enabled = true;
            }
        }

        public void Stop()
        {
            lock (lockObject)
            {
                PvtTimer timer = GetTimer();

                timer.Dispose();
                _timer = null;
                Enabled = false;
            }
        }

        public void Restart()
        {
            lock (lockObject)
            {
                Stop();
                Start();
            }
        }

        /// <summary>
        /// Gets the current internal timer or creates a new one if the last one disposed itself.
        /// </summary>
        /// <returns></returns>
        private PvtTimer GetTimer()
        {
            lock (lockObject)
            {
                if (_timer == null ||
                _timer.IsDisposed ||
                _timer.IsDisposing)
                {
                    _timer = new PvtTimer()
                    {
                        Interval = this.Interval,
                        Enabled = this.Enabled,
                        AutoReset = this.AutoReset
                    };

                    _timer.Elapsed += _timer_Elapsed;
                }
                return _timer;
            }
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!AutoReset)
            {
                Enabled = false;
            }

            if (Elapsed != null)
            {
                Elapsed(this, e);
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            lock (lockObject)
            {
                if (!disposedValue)
                {
                    if (disposing)
                    {
                        // TODO: dispose managed state (managed objects).
                        if (_timer != null)
                        {
                            _timer.Dispose();
                        }
                    }

                    disposedValue = true;
                }
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~ReusableTimer()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            lock (lockObject)
            {
                // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
                Dispose(true);
                // TODO: uncomment the following line if the finalizer is overridden above.
                // GC.SuppressFinalize(this);
            }
        }
        #endregion
    }
}
