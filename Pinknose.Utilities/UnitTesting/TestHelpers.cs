/////////////////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2020-2022 Cameron Mease
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
/////////////////////////////////////////////////////////////////////////////////////////
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pinknose.Utilities.UnitTesting
{
    public static class TestHelpers
    {
        #region Methods

        public static void AssertApproximatelyEqual<T>(this T value1, T value2, T acceptableDifference) where T : struct, IComparable<T>
        {
            var diff = Math.Abs((dynamic)value1 - (dynamic)value2);
            Assert.IsTrue(diff <= acceptableDifference);
        }

        public static void AssertContainsType<ListT, TargetT>(IEnumerable<ListT> list) => AssertContainsType(list, typeof(TargetT));

        public static void AssertContainsType<ListT>(IEnumerable<ListT> list, Type itemType) => Assert.IsTrue(list.Any(item => item?.GetType() == itemType));

        public static void AssertDoesNotContainType<ListT, TargetT>(IEnumerable<ListT> list) => AssertDoesNotContainType(list, typeof(TargetT));

        public static void AssertDoesNotContainType<T>(IEnumerable<T> list, Type itemType) => Assert.IsFalse(list.Any(item => item?.GetType() == itemType));

#if NETCOREAPP
#else

        public static void AssertApproximatelyEqual(this double value1, double value2, double acceptableDifference)
        {
            var diff = Math.Abs(value1 - value2);
            Assert.IsTrue(diff <= acceptableDifference);
        }

        public static void AssertApproximatelyEqual(this float value1, float value2, float acceptableDifference)
        {
            var diff = Math.Abs(value1 - value2);
            Assert.IsTrue(diff <= acceptableDifference);
        }

#endif

        public static void AssertEqualWhenRounded(double expectedValue, double actualValue, int digits)
        {
            Assert.AreEqual(expectedValue, Math.Round(actualValue, digits));
        }

        public static void AssertEventCaught<T>(T testObject, string eventName, Action test)
        {
            var eventInfo = typeof(T).GetEvent(eventName);

            Assert.IsNotNull(eventInfo);

            bool eventCaught = false;

            var handler = new EventHandler(delegate (object sender, EventArgs e) { eventCaught = true; });

            eventInfo.AddEventHandler(testObject, handler);

            Assert.IsFalse(eventCaught);

            test.Invoke();

            eventInfo.RemoveEventHandler(testObject, handler);

            Assert.IsTrue(eventCaught);
        }

        public static void AssertExceptionCaught<T>(Action test) where T : Exception
                    => AssertExceptionCaught(typeof(T), test);

        public static void AssertExceptionCaught(Type expectedExceptionType, Action test)
        {
            Assert.IsTrue(expectedExceptionType.IsAssignableTo(typeof(Exception)));

            bool exceptionCaught = false;

            try
            {
                test.Invoke();
            }
            catch (Exception ex)
            {
                if (ex.GetType() == expectedExceptionType)
                {
                    exceptionCaught = true;
                }
            }

            Assert.IsTrue(exceptionCaught);
        }

        #endregion Methods
    }
}