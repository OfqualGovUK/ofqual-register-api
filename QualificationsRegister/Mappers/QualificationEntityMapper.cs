using Ofqual.Common.RegisterAPI.Extensions;
using Ofqual.Common.RegisterAPI.Models;
using Ofqual.Common.RegisterAPI.Models.DB;

namespace Ofqual.Common.RegisterAPI.Mappers
{
    public static class QualificationEntityMapper
    {
        public static Qualification ToDomain(this DbQualification dbQualification)
        {
            return new Qualification
            {
                QualificationNumber = dbQualification.QualificationNumber,
                QualificationNumberNoObliques = dbQualification.QualificationNumberNoObliques,
                Title = dbQualification.Title,
                Status = dbQualification.Status,
                OrganisationName = dbQualification.OrganisationName,
                OrganisationAcronym = dbQualification.OrganisationAcronym,
                OrganisationRecognitionNumber = dbQualification.OrganisationRecognitionNumber,
                Type = dbQualification.Type,
                SSACode = dbQualification.SSA_Code,
                SSA = dbQualification.SSA,
                Level = dbQualification.Level,
                SubLevel = dbQualification.SubLevel,
                EQFLevel = dbQualification.EQFLevel,
                GradingType = dbQualification.GradingType,
                GradingScale = dbQualification.GradingScale,
                TotalCredits = dbQualification.TotalCredits,
                TQT = dbQualification.TQT,
                GLH = dbQualification.GLH,
                MinimumGLH = dbQualification.MinimumGLH,
                MaximumGLH = dbQualification.MaximumGLH,
                RegulationStartDate = dbQualification.RegulationStartDate,
                OperationalStartDate = dbQualification.OperationalStartDate,
                OperationalEndDate = dbQualification.OperationalEndDate,
                CertificationEndDate = dbQualification.CertificationEndDate,
                ReviewDate = dbQualification.ReviewDate,
                EmbargoDate = dbQualification.EmbargoDate,
                OfferedInEngland = dbQualification.OfferedInEngland,
                OfferedInNorthernIreland = dbQualification.OfferedInNorthernIreland,
                OfferedInternationally = dbQualification.OfferedInternationally,
                Specialism = dbQualification.Specialism,
                Pathways = dbQualification.Pathways,
                AssessmentMethods = dbQualification.AssessmentMethods?.GetSubStrings(),
                ApprovedForDELFundedProgramme = dbQualification.ApprovedForDELFundedProgramme,
                LinkToSpecification = dbQualification.LinkToSpecification,
                ApprenticeshipStandardReferenceNumber = dbQualification.ApprenticeshipStandardReferenceNumber,
                ApprenticeshipStandardTitle = dbQualification.ApprenticeshipStandardTitle,
                RegulatedByNorthernIreland = dbQualification.RegulatedByNorthernIreland,
                NIDiscountCode = dbQualification.NIDiscountCode,
                GCESizeEquivalence = dbQualification.GCESizeEquivalence,
                GCSESizeEquivalence = dbQualification.GCSESizeEquivalence,
                EntitlementFrameworkDesignation = dbQualification.EntitlementFrameworkDesignation,
                LastUpdatedDate = dbQualification.LastUpdatedDate,
                UILastUpdatedDate = dbQualification.UILastUpdatedDate,
                InsertedDate = dbQualification.InsertedDate,
                Version = dbQualification.Version,
                AppearsOnPublicRegister = dbQualification.AppearsOnPublicRegister,
                OrganisationId = dbQualification.OrganisationId,
                LevelId = dbQualification.LevelId,
                TypeId = dbQualification.TypeId,
                SSAId = dbQualification.SSAId,
                GradingTypeId = dbQualification.GradingTypeId,
                GradingScaleId = dbQualification.GradingScaleId,
                PreSixteen = dbQualification.PreSixteen,
                SixteenToEighteen = dbQualification.SixteenToEighteen,
                EighteenPlus = dbQualification.EighteenPlus,
                NineteenPlus = dbQualification.NineteenPlus,
            };
        }

        public static List<Qualification> ToDomain(this IEnumerable<DbQualification> dbQualifications)
        {
            return dbQualifications.Select(q => q.ToDomain()).ToList();
        }
    }
}
