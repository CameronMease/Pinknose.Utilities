using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pinknose.Utilities
{
    public class LatexUtility
    {
        private static readonly char[] SpecialCharacters = new char[]{'#', '$', '%', '&', '~', '_', '^', '\\', '{', '}'};

        public static string EncodeSpecialCharacters(string text)
        {
            StringBuilder sb = new StringBuilder();

            foreach (char c in text) 
            { 
                if (SpecialCharacters.Contains(c))
                {
                    sb.Append('\\');
                }

                sb.Append(c);
            }

            return sb.ToString();
        }
    }
}
