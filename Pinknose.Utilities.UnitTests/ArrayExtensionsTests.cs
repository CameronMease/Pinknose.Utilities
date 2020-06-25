using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pinknose.Utilities.UnitTests
{
    [TestClass]
    public class ArrayExtensionsTests
    {
        [TestMethod]
        public void ArrayRangeUnitTest()
        {
            int[] array = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            Assert.IsTrue(array.RangeByIndex(0, 10).SequenceEqual(array));
            Assert.IsTrue(array.RangeByIndex(0, 0).SequenceEqual(new int[] { 0 }));
            Assert.IsTrue(array.RangeByIndex(10, 10).SequenceEqual(new int[] { 10 }));
            Assert.IsTrue(array.RangeByIndex(3, 5).SequenceEqual(new int[] { 3, 4, 5 }));

            Assert.IsTrue(array.RangeByLength(0, 11).SequenceEqual(array));
            Assert.IsTrue(array.RangeByLength(0, 1).SequenceEqual(new int[] { 0 }));
            Assert.IsTrue(array.RangeByLength(10, 1).SequenceEqual(new int[] { 10 }));
            Assert.IsTrue(array.RangeByLength(3, 3).SequenceEqual(new int[] { 3, 4, 5 }));

            Assert.IsTrue(array.RangeFromEnd(11).SequenceEqual(array));
            Assert.IsTrue(array.RangeFromEnd(1).SequenceEqual(new int[] { 10 }));
            Assert.IsTrue(array.RangeFromEnd(3).SequenceEqual(new int[] { 8, 9, 10 }));

            Assert.IsTrue(array.RangeExcludeLast(0).SequenceEqual(array));
            Assert.IsTrue(array.RangeExcludeLast(1).SequenceEqual(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }));
            Assert.IsTrue(array.RangeExcludeLast(3).SequenceEqual(new int[] { 0, 1, 2, 3, 4, 5, 6, 7 }));

            Assert.IsTrue(array.RangeExcludeLast(0, 0).SequenceEqual(array));
            Assert.IsTrue(array.RangeExcludeLast(1, 1).SequenceEqual(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }));
            Assert.IsTrue(array.RangeExcludeLast(0, 10).SequenceEqual(new int[] { 0 }));

            Assert.IsTrue(array.RangeExcludeFirst(0).SequenceEqual(array));
            Assert.IsTrue(array.RangeExcludeFirst(1).SequenceEqual(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }));
            Assert.IsTrue(array.RangeExcludeFirst(10).SequenceEqual(new int[] { 10 }));
        }
    }
}
