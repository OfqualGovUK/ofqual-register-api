using Ofqual.Common.RegisterAPI.Models.DB;
using System.Text.RegularExpressions;

namespace Ofqual.Common.RegisterAPI.Models.Private
{
    public class QualificationPrivate
    {
        public int Id { get; set; }
        public string QualificationNumber { get; set; }
        public string? QualificationNumberNoObliques { get; set; }
        public string Title { get; set; }
        public string? Status { get; set; }
        public string OrganisationName { get; set; }
        public string OrganisationAcronym { get; set; }
        public string OrganisationRecognitionNumber { get; set; }
        public string? Type { get; set; }
        public string? SSACode { get; set; }
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
        public string[]? AssessmentMethods { get; set; }
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
        public bool AppearsOnPublicRegister { get; set; }
        public int OrganisationId { get; set; }
        public int? LevelId { get; set; }
        public int? TypeId { get; set; }
        public int? SSAId { get; set; }
        public int? GradingTypeId { get; set; }
        public int? GradingScaleId { get; set; }
        public bool? PreSixteen { get; set; }
        public bool? SixteenToEighteen { get; set; }
        public bool? EighteenPlus { get; set; }
        public bool? NineteenPlus { get; set; }

        public QualificationPrivate(Qualification qualification)
        {
            Id = qualification.Id;
            QualificationNumber = qualification.QualificationNumber;
            QualificationNumberNoObliques = qualification.QualifiationNumberNoObliques;
            Title = qualification.Title;
            Status = qualification.Status;
            OrganisationName = qualification.OrganisationName;
            OrganisationAcronym = qualification.OrganisationAcronym;
            OrganisationRecognitionNumber = qualification.OrganisationRecognitionNumber;
            Type = qualification.Type;
            SSACode = qualification.SSACode;
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
            EmbargoDate = qualification.EmbargoDate;
            OfferedInEngland = qualification.OfferedInEngland;
            OfferedInNorthernIreland = qualification.OfferedInNorthernIreland;
            OfferedInternationally = qualification.OfferedInternationally;
            Specialism = qualification.Specialism;
            Pathways = qualification.Pathways;
            AssessmentMethods = qualification.GetAssessmentMethodArray();
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
            UILastUpdatedDate = qualification.UILastUpdatedDate;
            InsertedDate = qualification.InsertedDate;
            Version = qualification.Version;
            AppearsOnPublicRegister = qualification.AppearsOnPublicRegister;
            OrganisationId = qualification.OrganisationId;
            LevelId = qualification.LevelId;
            TypeId = qualification.TypeId;
            SSAId = qualification.SSAId;
            GradingTypeId = qualification.GradingTypeId;
            GradingScaleId = qualification.GradingScaleId;
            PreSixteen = qualification.PreSixteen;
            SixteenToEighteen = qualification.SixteenToEighteen;
            EighteenPlus = qualification.EighteenPlus;
            NineteenPlus = qualification.NineteenPlus;
        }
    }
}
