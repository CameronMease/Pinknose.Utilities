using System;
using System.Collections.Generic;
using System.Text;

namespace Pinknose.Utilities
{
    public static class ArrayExtensions
    {
        public static T[] RangeByIndex<T>(this T[] array, int start, int end) where T: struct
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

            if (length < 0 || start+length > array.Length)
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
    }
}
