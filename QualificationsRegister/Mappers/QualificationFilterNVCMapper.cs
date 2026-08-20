using Ofqual.Common.RegisterAPI.Extensions;
using Ofqual.Common.RegisterAPI.Models;
using Ofqual.Common.RegisterAPI.Models.Exceptions;
using System.Collections.Specialized;
using static Ofqual.Common.RegisterAPI.Constants.QualificationFilterConstants;

namespace Ofqual.Common.RegisterAPI.Mappers
{
    //maps the HTTP params to a model for easier processing / filtering
    public static class QualificationFilterNvcMapper
    {
        public static QualificationFilter? GetQualificationFilterQuery(this NameValueCollection nvc)
        {
            if (nvc.Count == 0)
            {
                return null;
            }

            var filter = new QualificationFilter
            {
                AssessmentMethods = nvc.Get(ASSESSMENT_METHODS_FILTER)?.GetSubStrings(),
                GradingTypes = nvc.Get(GRADING_TYPE_FILTER)?.GetSubStrings(),
                AwardingOrganisations = nvc.GetValues(AWARDING_ORGANISATIONS_FILTER),
                AORecognitionNumbers = nvc.Get(AO_RECOGNITION_NUMBERS_FILTER)?.GetSubStrings(),
                Availability = nvc.Get(AVAILABILITY_FILTER)?.GetSubStrings(),
                QualificationTypes = nvc.Get(QUALIFICATION_TYPES_FILTER)?.GetSubStrings(),
                QualificationLevels = nvc.Get(QUALIFICATION_LEVELS_FILTER)?.GetSubStrings(),
                QualificationSubLevels = nvc.Get(QUALIFICATION_SUB_LEVELS_FILTER)?.GetSubStrings(),
                NationalAvailability = nvc.Get(NATIONAL_AVAILABILITY_FILTER)?.GetSubStrings(),
                SectorSubjectAreas = nvc.GetValues(SSA_FILTER),

                MinTotalQualificationTime = ParseInt(nvc.Get(MIN_TQT_FILTER), "minTotalQualificationTime"),
                MaxTotalQualificationTime = ParseInt(nvc.Get(MAX_TQT_FILTER), "maxTotalQualificationTime"),
                MinGuidedLearninghours = ParseInt(nvc.Get(MIN_GLH_FILTER), "minGuidedLearninghours"),
                MaxGuidedLearninghours = ParseInt(nvc.Get(MAX_GLH_FILTER), "maxGuidedLearninghours"),

                IntentionToSeekFundingInEngland = ParseBoolean(nvc.Get(FUNDING_INTENTION_ENGLAND))
            };
            return filter;
        }

        private static bool? ParseBoolean(string? value) =>
            value?.Trim() switch
            {
                null => null,
                "1" => true,
                "0" => false,
                _ => bool.TryParse(value, out bool result)
                    ? result
                    : null
            };
        
        private static int? ParseInt(string? value, string field)
        {
            try
            {
                if (value == null) return null;
                if (value == "null") return null;
                if (value.Length == 0) return null;

                return value != null ? int.Parse(value!) : null;
            }
            catch (Exception)
            {
                throw new BadRequestException($"Error parsing value '{value}' for {field}.");
            }
        }
    }
}

