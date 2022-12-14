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

using System;

namespace Pinknose.Utilities
{
    public static class ArrayExtensions
    {
        #region Methods

        public static T[] RangeByIndex<T>(this T[] array, int start, int end) where T : struct
        {
            if (start < 0 || start > array.Length - 1)
            {
                throw new ArgumentOutOfRangeException(nameof(start));
            }

            if (end < 0 || end > array.Length - 1 || end < start)
            {
                throw new ArgumentOutOfRangeException(nameof(start));
            }

            var newArray = new T[end - start + 1];

            Array.Copy(array, start, newArray, 0, newArray.Length);

            return newArray;
        }

        public static T[] RangeByLength<T>(this T[] array, int start, int length) where T : struct
        {
            if (start < 0 || start > array.Length - 1)
            {
                throw new ArgumentOutOfRangeException(nameof(start));
            }

            if (length < 0 || start + length > array.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(start));
            }

            var newArray = new T[length];

            Array.Copy(array, start, newArray, 0, length);

            return newArray;
        }

        public static T[] RangeFromEnd<T>(this T[] array, int fromEnd) where T : struct
        {
            if (fromEnd < 0 || fromEnd > array.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(fromEnd));
            }

            var newArray = new T[fromEnd];

            Array.Copy(array, array.Length - fromEnd, newArray, 0, fromEnd);

            return newArray;
        }

        public static T[] RangeExcludeLast<T>(this T[] array, int exclude) where T : struct
        {
            return array.RangeByLength(0, array.Length - exclude);
        }

        public static T[] RangeExcludeLast<T>(this T[] array, int start, int exclude) where T : struct
        {
            return array.RangeByLength(start, array.Length - exclude - start);
        }

        public static T[] RangeExcludeFirst<T>(this T[] array, int exclude) where T : struct
        {
            return array.RangeByIndex(exclude, array.Length - 1);
        }

        #endregion Methods
    }
}