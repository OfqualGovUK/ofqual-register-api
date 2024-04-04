using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Ofqual.Common.RegisterAPI.Extensions
{
    public static class StringExtensions
    {
        public static string[]? GetSubStrings(this string value)
        {
            if (value == null) return null;

            //remove [], quotes and spaces from the db value
            var str = Regex.Replace(value, @"[\[\\""\]]+", "");

            var arr = str.Replace(", ", ",").Split(",");
            return arr;
        }
    }
}
