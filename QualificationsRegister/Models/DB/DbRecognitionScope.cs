using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Ofqual.Common.RegisterAPI.Models.DB
{
    public class DbRecognitionScope
    {
        public required int Id { get; set; }
        public required string RecognitionNumber { get; set; }
        public required string OrganisationAcronym { get; set; }
        public required string OrganisationName { get; set; }
        public required bool InclusionExclusion { get; set; }
        public required string InclusionExclusionType { get; set; }
        public string? RecognitionPermissionType { get; set; }
        public string? Type { get; set; }
        public string? Level { get; set; }
        public string? SubLevel { get; set; }
        public string? LevelDescription { get; set; }
        public string? SSA { get; set; }
        public string? ApprenticeshipStandardReferenceNumber { get; set; }
        public string? ApprenticeshipStandardTitle { get; set; }
        public string? TechnicalQualificationSubject { get; set; }
        public required string QualificationDescription { get; set; }
        public required int OrganisationId { get; set; }
        public int? RecognitionPermissionTypeId { get; set; }
        public int? LevelId { get; set; }
        public int? SSAId { get; set; }
        public int? QualificationTypeId { get; set; }
        public int? ApprenticeshipStandardId { get; set; }
        public int? TechnicalQualificationSubjectId { get; set; }

    }
}
