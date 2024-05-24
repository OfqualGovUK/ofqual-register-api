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

            var filter = new QualificationFilter();

            filter.AssessmentMethods = nvc.Get(ASSESSMENT_METHODS_FILTER)?.GetSubStrings();
            filter.GradingTypes = nvc.Get(GRADING_TYPE_FILTER)?.GetSubStrings();
            filter.AwardingOrganisations = nvc.Get(AWARDING_ORGANISATIONS_FILTER)?.GetSubStrings();
            filter.Availability = nvc.Get(AVAILABILITY_FILTER)?.GetSubStrings();
            filter.QualificationTypes = nvc.Get(QUALIFICATION_TYPES_FILTER)?.GetSubStrings();
            filter.QualificationLevels = nvc.Get(QUALIFICATION_LEVELS_FILTER)?.GetSubStrings();
            filter.QualificationSubLevels = nvc.Get(QUALIFICATION_SUB_LEVELS_FILTER)?.GetSubStrings();
            filter.NationalAvailability = nvc.Get(NATIONAL_AVAILABILITY_FILTER)?.GetSubStrings();
            filter.SectorSubjectAreas = nvc.Get(SSA_FILTER)?.GetSubStrings();

            filter.MinTotalQualificationTime = ParseInt(nvc.Get(MIN_TQT_FILTER), "minTotalQualificationTime");
            filter.MaxTotalQualificationTime = ParseInt(nvc.Get(MAX_TQT_FILTER), "maxTotalQualificationTime");
            filter.MinGuidedLearninghours = ParseInt(nvc.Get(MIN_GLH_FILTER), "minGuidedLearninghours");
            filter.MaxGuidedLearninghours = ParseInt(nvc.Get(MAX_GLH_FILTER), "maxGuidedLearninghours");

            return filter;
        }

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

