using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Timers;

namespace Pinknose.Utilities.UnitTests
{
    [TestClass]
    public class SimpleCountingSemaphoreTests
    {
        [TestMethod]
        public void InvalidConstructorInputsTest()
        {
            // startCount < 0
            CatchExceptionHelper.VerifyExceptionCaught<ArgumentException>(() => new SimpleCountingSemaphore(100, -1));

            // startCount < maxCoutn
            CatchExceptionHelper.VerifyExceptionCaught<ArgumentException>(() => new SimpleCountingSemaphore(100, 1000));

            // Valid inputs
            var sem = new SimpleCountingSemaphore(10, 0);

            Assert.AreEqual(sem.Count, 0);
            Assert.AreEqual(sem.MaxCount, 10);
        }

        [TestMethod]
        public void GiveTakeTest()
        {
            var sem = new SimpleCountingSemaphore(3, 0);

            sem.Take();
            Assert.AreEqual(sem.Count, 1);
            sem.Take();
            Assert.AreEqual(sem.Count, 2);
            sem.Take();
            Assert.AreEqual(sem.Count, 3);

            var result = sem.TryTake();
            Assert.IsFalse(result);
            Assert.AreEqual(sem.Count, 3);

            Timer timer = new Timer(200);
            timer.Elapsed += (sender, e) => sem.Give();
            timer.Start();

            sem.Take();
            Assert.AreEqual(sem.Count, 3);

            sem.Give();
            Assert.AreEqual(sem.Count, 2);
            sem.Give();
            Assert.AreEqual(sem.Count, 1);
            sem.Give();
            Assert.AreEqual(sem.Count, 0);
            sem.Give();
            Assert.AreEqual(sem.Count, 0);

            sem.Dispose();
            CatchExceptionHelper.VerifyExceptionCaught<ObjectDisposedException>(() => sem.Take());
        }

        
    }
}
