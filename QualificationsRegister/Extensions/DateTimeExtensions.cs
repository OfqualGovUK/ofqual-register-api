using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Ofqual.Common.RegisterAPI.Extensions
{
    public static partial class DateTimeExtensions
    {
        public static DateTime ToFormattedUniversalTime(this DateTime value)
        {           
            var thisVal = value.ToUniversalTime();

            return new DateTime(thisVal.Year, thisVal.Month, thisVal.Day, thisVal.Hour, thisVal.Minute, thisVal.Second);
        }

    }
}
