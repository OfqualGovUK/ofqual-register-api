using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Ofqual.Common.RegisterAPI.Models.DB
{
    public class QualificationPublic
    {
        public required int Id { get; set; }
        public required string QualificationNumber { get; set; }
        [Column("QualifiationNumberNoObliques")]
        public string? QualificationNumberNoObliques { get; set; }
        public required string Title { get; set; }
        public string? Status { get; set; }
        public required string OrganisationName { get; set; }
        public required string OrganisationAcronym { get; set; }
        public required string OrganisationRecognitionNumber { get; set; }
        public string? Type { get; set; }
        public string? SSA { get; set; }
        public string? Level { get; set; }
        public string? SubLevel { get; set; }
        public string? EQFLevel { get; set; }
        public string? GradingType { get; set; }
        public string? GradingScale { get; set; }
        public int? TotalCredits { get; set; }
        public int? TQT { get; set; }
        public int? GLH { get; set; }
        public int? MinimumGLH { get; set; }
        public int? MaximumGLH { get; set; }
        public DateTime RegulationStartDate { get; set; }
        public DateTime OperationalStartDate { get; set; }
        public DateTime? OperationalEndDate { get; set; }
        public DateTime? CertificationEndDate { get; set; }
        public DateTime? ReviewDate { get; set; }
        public DateTime? EmbargoDate { get; set; }
        public bool? OfferedInEngland { get; set; }
        public bool? OfferedInNorthernIreland { get; set; }
        public bool? OfferedInternationally { get; set; }
        public string? Specialism { get; set; }
        public string? Pathways { get; set; }
        [Column("AssessmentMethods")]
        [JsonIgnore]
        public string? AssessmentMethodsString { get; set; }
        [NotMapped]
        public string[]? AssessmentMethods
        {
            get
            {
                if (AssessmentMethodsString == null) { return null; }
                else
                {
                    //remove [], quotes and spaces from the db value
                    var str = Regex.Replace(AssessmentMethodsString, @"[\[\\""\]]+", "");

                    var arr = str.Replace(", ", ",").Split(",");
                    return arr;
                }
            }
            set { }
        }
        public bool? ApprovedForDELFundedProgramme { get; set; }
        public string? LinkToSpecification { get; set; }
        public string? ApprenticeshipStandardReferenceNumber { get; set; }
        public string? ApprenticeshipStandardTitle { get; set; }
        public bool? RegulatedByNorthernIreland { get; set; }
        public string? NIDiscountCode { get; set; }
        public decimal? GCESizeEquivalence { get; set; }
        public decimal? GCSESizeEquivalence { get; set; }
        public string? EntitlementFrameworkDesignation { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
        public DateTime UILastUpdatedDate { get; set; }
        public DateTime? InsertedDate { get; set; }
        public int? Version { get; set; }
        public int OrganisationId { get; set; }
        public int? LevelId { get; set; }
        public int? TypeId { get; set; }
        public int? SSAId { get; set; }
        public int? GradingTypeId { get; set; }
        public int? GradingScaleId { get; set; }

        public string[]? GetAssessmentMethodArray()
        {
            if (AssessmentMethodsString == null) { return null; }
            else
            {
                //remove [], quotes and spaces from the db value
                var str = Regex.Replace(AssessmentMethodsString, @"[\[\\""\]]+", "");

                var arr = str.Replace(", ", ",").Split(",");
                return arr;
            }
        }

        public string GetQualificationNumber()
        {
            return QualificationNumberNoObliques ?? QualificationNumber.Replace("/", "");
        }
    }
}
