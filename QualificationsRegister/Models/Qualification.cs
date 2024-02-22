namespace Ofqual.Common.RegisterAPI.Models
{
    public record class Qualification
    {
        public int Id { get; init; }
        public string QualificationNumber { get; init; }
        public string QualificationNumberNoObliques { get; init; }
        public string Title { get; init; }
        public string Status { get; init; }
        public string OrganisationName { get; init; }
        public string OrganisationAcronym { get; init; }
        public string OrganisationRecognitionNumber { get; init; }
        public string Type { get; init; }
        public string SSA { get; init; }
        public string Level { get; init; }
        public string SubLevel { get; init; }
        public string EQFLevel { get; init; }
        public string GradingType { get; init; }
        public string GradingScale { get; init; }
        public int? TotalCredits { get; init; }
        public int? TQT { get; init; }
        public int? GLH { get; init; }
        public int? MinimumGLH { get; init; }
        public int? MaximumGLH { get; init; }
        public DateTime RegulationStartDate { get; init; }
        public DateTime OperationalStartDate { get; init; }
        public DateTime? OperationalEndDate { get; init; }
        public DateTime? CertificationEndDate { get; init; }
        public DateTime? ReviewDate { get; init; }
        public bool? OfferedInEngland { get; init; }
        public bool? OfferedInNorthernIreland { get; init; }
        public bool? OfferedInternationally { get; init; }
        public string Specialism { get; init; }
        public string Pathways { get; init; }
        public string AssessmentMethods { get; init; }
        public bool? ApprovedForDELFundedProgramme { get; init; }
        public string LinkToSpecification { get; init; }
        public string ApprenticeshipStandardReferenceNumber { get; init; }
        public string ApprenticeshipStandardTitle { get; init; }
        public bool? RegulatedByNorthernIreland { get; init; }
        public string NIDiscountCode { get; init; }
        public decimal? GCESizeEquivalence { get; init; }
        public decimal? GCSESizeEquivalence { get; init; }
        public string EntitlementFrameworkDesignation { get; init; }
        public DateTime? LastUpdatedDate { get; init; }
        public int OrganisationId { get; init; }
        public int? LevelId { get; init; }
        public int? TypeId { get; init; }
        public int? SSAId { get; init; }
        public int? GradingTypeId { get; init; }
        public int? GradingScaleId { get; init; }
    }

}
