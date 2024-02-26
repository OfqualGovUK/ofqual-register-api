using Ofqual.Common.RegisterAPI.Models.Private;

namespace Ofqual.Common.RegisterAPI.Models.Public
{
    public class QualificationPublic
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

        public QualificationPublic(QualificationPrivate qualification)
        {
            Id = qualification.Id;
            QualificationNumber = qualification.QualificationNumber;
            QualificationNumberNoObliques = qualification.QualificationNumberNoObliques;
            Title = qualification.Title;
            Status = qualification.Status;
            OrganisationName = qualification.OrganisationName;
            OrganisationAcronym = qualification.OrganisationAcronym;
            OrganisationRecognitionNumber = qualification.OrganisationRecognitionNumber;
            Type = qualification.Type;
            SSA = qualification.SSA;
            Level = qualification.Level;
            SubLevel = qualification.SubLevel;
            EQFLevel = qualification.EQFLevel;
            GradingType = qualification.GradingType;
            GradingScale = qualification.GradingScale;
            TotalCredits = qualification.TotalCredits;
            TQT = qualification.TQT;
            GLH = qualification.GLH;
            MinimumGLH = qualification.MinimumGLH;
            MaximumGLH = qualification.MaximumGLH;
            RegulationStartDate = qualification.RegulationStartDate;
            OperationalStartDate = qualification.OperationalStartDate;
            OperationalEndDate = qualification.OperationalEndDate;
            CertificationEndDate = qualification.CertificationEndDate;
            ReviewDate = qualification.ReviewDate;
            OfferedInEngland = qualification.OfferedInEngland;
            OfferedInNorthernIreland = qualification.OfferedInNorthernIreland;
            OfferedInternationally = qualification.OfferedInternationally;
            Specialism = qualification.Specialism;
            Pathways = qualification.Pathways;
            AssessmentMethods = qualification.AssessmentMethods;
            ApprovedForDELFundedProgramme = qualification.ApprovedForDELFundedProgramme;
            LinkToSpecification = qualification.LinkToSpecification;
            ApprenticeshipStandardReferenceNumber = qualification.ApprenticeshipStandardReferenceNumber;
            ApprenticeshipStandardTitle = qualification.ApprenticeshipStandardTitle;
            RegulatedByNorthernIreland = qualification.RegulatedByNorthernIreland;
            NIDiscountCode = qualification.NIDiscountCode;
            GCESizeEquivalence = qualification.GCESizeEquivalence;
            GCSESizeEquivalence = qualification.GCSESizeEquivalence;
            EntitlementFrameworkDesignation = qualification.EntitlementFrameworkDesignation;
            LastUpdatedDate = qualification.LastUpdatedDate;
            OrganisationId = qualification.OrganisationId;
            LevelId = qualification.LevelId;
            TypeId = qualification.TypeId;
            SSAId = qualification.SSAId;
            GradingTypeId = qualification.GradingTypeId;
            GradingScaleId = qualification.GradingScaleId;
        }
    }
}


