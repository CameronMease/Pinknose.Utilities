using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Pinknose.Utilities.UnitTests
{
    [TestClass]
    public class ReusableThreadSafeTimerUnitTests
    {
        [TestMethod]
        public void ThreadSafeTimerTest()
        {
            bool elapsedEventOccurred = false;

            ReusableThreadSafeTimer timer = new ReusableThreadSafeTimer()
            {
                AutoReset = false,
                Enabled = false,
                Interval = 100
            };

            timer.Elapsed += (sender, e) => elapsedEventOccurred = true;

            Assert.IsFalse(timer.AutoReset);
            Assert.IsFalse(timer.Enabled);
            Assert.AreEqual(timer.Interval, 100);

            timer.Start();
            Assert.IsTrue(timer.Enabled);
            Assert.IsFalse(elapsedEventOccurred);
            Thread.Sleep(50);
            Assert.IsTrue(timer.Enabled);
            Assert.IsFalse(elapsedEventOccurred);
            Thread.Sleep(100);
            Assert.IsFalse(timer.Enabled);
            Assert.IsTrue(elapsedEventOccurred);

            Thread.Sleep(1000);
            Assert.IsFalse(timer.AutoReset);
            Assert.IsFalse(timer.Enabled);
            Assert.AreEqual(timer.Interval, 100);

            timer.Dispose();

            timer = new ReusableThreadSafeTimer(100)
            {
                AutoReset = true
            };

            Assert.IsFalse(timer.Enabled);
            Assert.IsTrue(timer.AutoReset);
            Assert.AreEqual(timer.Interval, 100);

            int count = 0;

            timer.Elapsed += (sender, e) => count++;

            timer.Start();
            Thread.Sleep(1000);
            timer.Stop();
            Assert.IsFalse(timer.Enabled);
            Assert.IsTrue(count >= 9);

            timer.Dispose();

            timer = new ReusableThreadSafeTimer(TimeSpan.FromSeconds(2));

            Assert.AreEqual(timer.Interval, 2000);

            timer.Dispose();

            timer = new ReusableThreadSafeTimer(1000);
            DateTime finishTime = DateTime.Now;
            DateTime startTime;

            timer.Elapsed += (sender, e) => finishTime = DateTime.Now;

            startTime = DateTime.Now;
            timer.Start();
            Thread.Sleep(500);
            timer.Restart();
            Thread.Sleep(1250);

            TimeSpan totalTime = finishTime - startTime;

            Assert.IsTrue(totalTime > TimeSpan.FromMilliseconds(1200) && totalTime < TimeSpan.FromMilliseconds(1700));

            timer.Dispose();
        }

    }
}
