using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Ofqual.Common.RegisterAPI.Models.MetaData
{
    public class QualificationMetaData
    {
        public Dictionary<string, int> AssessmentMethod { set; get; } = new Dictionary<string, int>();
        public Dictionary<string, int> AssessmentType { set; get; } = new Dictionary<string, int>();
        public Dictionary<string, int> AwardingOrganision { set; get; } = new Dictionary<string, int>();
        public Dictionary<string, int> Availability { set; get; } = new Dictionary<string, int>();
        public Dictionary<string, int> QualificationType { set; get; } = new Dictionary<string, int>();
        public Dictionary<string, int> QualificationLevel { set; get; } = new Dictionary<string, int>();
        public Dictionary<string, int> QualificationSubLevel { set; get; } = new Dictionary<string, int>();
        public Dictionary<string, int> GradingType { set; get; } = new Dictionary<string, int>();
        public Dictionary<string, int> NationalAvailability { set; get; } = new Dictionary<string, int>();
        public Dictionary<string, int> MinTotalQualificationTime { set; get; } = new Dictionary<string, int>();
        public Dictionary<string, int> MaxTotalQualificationTime { set; get; } = new Dictionary<string, int>();
        public Dictionary<string, int> MinGuidedLearningHours { set; get; } = new Dictionary<string, int>();
        public Dictionary<string, int> MaxGuidedLearningHours { set; get; } = new Dictionary<string, int>();
        public Dictionary<string, int> SectorSubjectArea { set; get; } = new Dictionary<string, int>();
    }

    public static class QualificationMetaDataHelper
    {
        public static Dictionary<string, int> GroupWithCount(this IEnumerable<string> list)
        => list.Where(c => c.IsNullOrEmpty())
               .GroupBy(i => i)
               .Select(g => new KeyValuePair<string, int>(g.Key, g.Count()))
               .ToDictionary();
    }
}
