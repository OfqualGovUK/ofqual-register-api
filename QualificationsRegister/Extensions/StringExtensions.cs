using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Ofqual.Common.RegisterAPI.Extensions
{
    public static partial class StringExtensions
    {
        [GeneratedRegex(@"[\[\\""\]]+")]
        private static partial Regex SubStringRegex();

        public static string[]? GetSubStrings(this string value)
        {
            if (value == null) return null;

            //remove [], quotes and spaces from the db value
            var str = SubStringRegex().Replace(value, "");

            var arr = str.Replace(", ", ",").Split(",");
            return arr;
        }

    }
}
