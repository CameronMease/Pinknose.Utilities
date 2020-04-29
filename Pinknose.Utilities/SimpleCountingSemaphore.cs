using System;
using System.Threading;

namespace Pinknose.Utilities
{
    public class SemaphoreCountChangedEventArgs : EventArgs
    {
        /// <summary>Initializes a new instance of the <see cref="SemaphoreCountChangedEventArgs" /> class.</summary>
        public SemaphoreCountChangedEventArgs(int count)
        {
            Count = count;
        }

        public int Count { get; private set; }
    }

    public class SimpleCountingSemaphore : IDisposable
    {
        private int count = 0;
        private ManualResetEvent resetEvent = new ManualResetEvent(false);
        private Mutex protectCountMutex = new Mutex();

        public event EventHandler<SemaphoreCountChangedEventArgs> SemaphoreCountChanged;

        public SimpleCountingSemaphore(int maxCount, int startCount = 0)
        {
            MaxCount = maxCount;

            if (startCount > maxCount || startCount < 0)
            {
                throw new ArgumentException();
            }

            count = startCount;
        }

        int _maxCount;
        public int MaxCount
        {
            get
            {
                return _maxCount;
            }
            set
            {
                if (value >= 0)
                {
                    protectCountMutex.WaitOne();
                    _maxCount = value;
                    protectCountMutex.ReleaseMutex();
                    resetEvent.Set();
                }
            }
        }

        public void Take()
        {
            Take(waitForSemaphore: true);
        }


        private bool Take(bool waitForSemaphore)
        {
            bool gotSemaphore = false;

            do
            {
                protectCountMutex.WaitOne();
                if (count < MaxCount)
                {
                    count++;
                    protectCountMutex.ReleaseMutex();
                    gotSemaphore = true;

                    SemaphoreCountChanged?.Invoke(this, new SemaphoreCountChangedEventArgs(count));

                    // Log.Verbose("Counting Semaphore Take: Count={count}", count);
                }
                else
                {
                    protectCountMutex.ReleaseMutex();
                    resetEvent.WaitOne();
                }
            } while (!gotSemaphore && waitForSemaphore);


            return gotSemaphore;
        }

        public bool TryTake()
        {
            return Take(waitForSemaphore: false);
        }

        public void Give()
        {
            protectCountMutex.WaitOne();

            if (count > 0)
            {
                count--;
                //Log.Verbose("Counting Semaphore Give: Count={count}", count);
                resetEvent.Set();

                SemaphoreCountChanged?.Invoke(this, new SemaphoreCountChangedEventArgs(count));
            }

            protectCountMutex.ReleaseMutex();
        }

        public int Count { get => count; }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    protectCountMutex?.Dispose();
                    resetEvent?.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~SimpleCountingSemaphore()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
