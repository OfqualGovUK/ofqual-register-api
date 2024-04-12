using Ofqual.Common.RegisterAPI.Extensions;
using Ofqual.Common.RegisterAPI.Models;
using Ofqual.Common.RegisterAPI.Models.DB;

namespace Ofqual.Common.RegisterAPI.Mappers
{
    public static class QualificationPublicEntityMapper
    {
        public static QualificationPublic ToDomain(this DbQualificationPublic dbQualificationPublic)
        {
            return new QualificationPublic
            {
                QualificationNumber = dbQualificationPublic.QualificationNumber,
                QualificationNumberNoObliques = dbQualificationPublic.QualificationNumberNoObliques,
                Title = dbQualificationPublic.Title,
                Status = dbQualificationPublic.Status,
                OrganisationName = dbQualificationPublic.OrganisationName,
                OrganisationAcronym = dbQualificationPublic.OrganisationAcronym,
                OrganisationRecognitionNumber = dbQualificationPublic.OrganisationRecognitionNumber,
                Type = dbQualificationPublic.Type,
                SSA = dbQualificationPublic.SSA,
                Level = dbQualificationPublic.Level,
                SubLevel = dbQualificationPublic.SubLevel,
                EQFLevel = dbQualificationPublic.EQFLevel,
                GradingType = dbQualificationPublic.GradingType,
                GradingScale = dbQualificationPublic.GradingScale,
                TotalCredits = dbQualificationPublic.TotalCredits,
                TQT = dbQualificationPublic.TQT,
                GLH = dbQualificationPublic.GLH,
                MinimumGLH = dbQualificationPublic.MinimumGLH,
                MaximumGLH = dbQualificationPublic.MaximumGLH,
                OfferedInEngland = dbQualificationPublic.OfferedInEngland,
                OfferedInNorthernIreland = dbQualificationPublic.OfferedInNorthernIreland,
                OfferedInternationally = dbQualificationPublic.OfferedInternationally,
                Specialism = dbQualificationPublic.Specialism,
                Pathways = dbQualificationPublic.Pathways,
                AssessmentMethods = dbQualificationPublic.AssessmentMethods?.GetSubStrings(),
                ApprovedForDELFundedProgramme = dbQualificationPublic.ApprovedForDELFundedProgramme,
                LinkToSpecification = dbQualificationPublic.LinkToSpecification,
                ApprenticeshipStandardReferenceNumber = dbQualificationPublic.ApprenticeshipStandardReferenceNumber,
                ApprenticeshipStandardTitle = dbQualificationPublic.ApprenticeshipStandardTitle,
                RegulatedByNorthernIreland = dbQualificationPublic.RegulatedByNorthernIreland,
                NIDiscountCode = dbQualificationPublic.NIDiscountCode,
                GCESizeEquivalence = dbQualificationPublic.GCESizeEquivalence,
                GCSESizeEquivalence = dbQualificationPublic.GCSESizeEquivalence,
                EntitlementFrameworkDesignation = dbQualificationPublic.EntitlementFrameworkDesignation,
                RegulationStartDate = dbQualificationPublic.RegulationStartDate.ToFormattedUniversalTime(),
                OperationalStartDate = dbQualificationPublic.OperationalStartDate.ToFormattedUniversalTime(),
                OperationalEndDate = dbQualificationPublic.OperationalEndDate?.ToFormattedUniversalTime(),
                CertificationEndDate = dbQualificationPublic.CertificationEndDate?.ToFormattedUniversalTime(),
                ReviewDate = dbQualificationPublic.ReviewDate?.ToFormattedUniversalTime(),
                LastUpdatedDate = dbQualificationPublic.LastUpdatedDate?.ToFormattedUniversalTime()
            };
        }

        public static List<QualificationPublic> ToDomain(this IEnumerable<DbQualificationPublic> dbQualifications)
        {
            return dbQualifications.Select(q => q.ToDomain()).ToList();
        }
    }
}
