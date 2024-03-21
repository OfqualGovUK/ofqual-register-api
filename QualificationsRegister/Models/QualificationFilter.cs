using Ofqual.Common.RegisterAPI.Extensions;
using System.Collections.Specialized;

namespace Ofqual.Common.RegisterAPI.Models
{
    public class QualificationFilter
    {
        public string[]? AssessmentMethod { get; set; }
        public string[]? GradingType { get; set; }
        public string[]? AwardingOrganisation { get; set; }
        public string[]? Availability { get; set; }
        public string[]? QualificationType{get;set;}
        public string[]? QualificationLevel{get;set;}
        public string[]? QualificationSubLevel{get;set;}
        public string[]? NationalAvailability{get;set;}
        public int? MinTotalQualificationTime{get;set;}
        public int? MaxTotalQualificationTime{get;set;}
        public int? MinGuidedlearninghours{get;set;}
        public int? MaxGuidedlearninghours{get;set;}
        public string[]? SectorSubjectArea { get; set; }

        public QualificationFilter(NameValueCollection filtersQuery)
        {
            AssessmentMethod = filtersQuery.Get("assessmentMethod")?.GetSubStrings();
        }

    }
}
