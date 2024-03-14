using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Ofqual.Common.RegisterAPI.Models
{
    public static class Utilities
    {
        #region Qualifications
        public static string[]? GetAssessmentMethods(string? assessmentMethods)
        {
            if (assessmentMethods == null) { return null; }
            else
            {
                //remove [], quotes and spaces from the db value
                var str = Regex.Replace(assessmentMethods, @"[\[\\""\]]+", "");

                var arr = str.Replace(", ", ",").Split(",");
                return arr;
            }
        }
        #endregion
    }
}
