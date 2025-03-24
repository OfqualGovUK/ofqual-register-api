using Ofqual.Common.RegisterAPI.Extensions;
using System.Collections.Specialized;

namespace Ofqual.Common.RegisterAPI.Models
{
    public class QualificationFilter
    {
        public string[]? AssessmentMethods { get; set; }
        public string[]? GradingTypes { get; set; }
        public string[]? AwardingOrganisations { get; set; }
        public string[]? Availability { get; set; }
        public string[]? QualificationTypes { get; set; }
        public string[]? QualificationLevels { get; set; }
        public string[]? QualificationSubLevels { get; set; }
        public string[]? NationalAvailability { get; set; }
        public int? MinTotalQualificationTime { get; set; }
        public int? MaxTotalQualificationTime { get; set; }
        public int? MinGuidedLearninghours { get; set; }
        public int? MaxGuidedLearninghours { get; set; }
        public string[]? SectorSubjectAreas { get; set; }

        public bool? IntentionToSeekFundingInEngland { get; set; }
    }
}
