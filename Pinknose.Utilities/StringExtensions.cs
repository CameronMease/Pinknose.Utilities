using System;
using System.Text;

namespace Pinknose.Utilities
{
    public static class StringExtensions
    {
        #region Methods

        /// <summary>
        /// Adds carriage returns to string so when printed, it does not exceed the max width.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="maxWidth"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static string FormatToMaxWidth(this string str, int maxWidth) => FormatToMaxWidth(str, maxWidth, Environment.NewLine);

        public static string FormatToMaxWidthForHtml(this string str, int maxWidth) => FormatToMaxWidth(str, maxWidth, "<BR/>");

        private static string FormatToMaxWidth(this string str, int maxWidth, string lineTerminator)
        {
            if (str is null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            if (maxWidth < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(maxWidth));
            }

            var words = str.Split(' ');

            StringBuilder sb = new StringBuilder();

            int currentWidth = 0;

            for (int index = 0; index < words.Length; index++)
            {
                var currentWord = words[index];

                if (currentWord.Length + currentWidth > maxWidth)
                {
                    sb.Append(lineTerminator);
                    currentWidth = 0;
                }

                if (currentWidth > 0)
                {
                    sb.Append(' ');
                    currentWidth++;
                }

                currentWidth += currentWord.Length;
                sb.Append(currentWord);
            }

            return sb.ToString();
        }

        #endregion Methods
    }
}