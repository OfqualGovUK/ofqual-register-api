using Ofqual.Common.RegisterAPI.Extensions;
using Ofqual.Common.RegisterAPI.Models;
using Ofqual.Common.RegisterAPI.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ofqual.Common.RegisterAPI.Mappers
{
    public static class QualificationFilterNvcMapper
    {
        public static QualificationFilter? GetQualificationFilterQuery(this NameValueCollection nvc)
        {
            if (nvc.Count == 0)
            {
                return null;
            }

            var filter = new QualificationFilter();

            filter.AssessmentMethods = nvc.Get("AssessmentMethods")?.GetSubStrings();
            filter.GradingTypes = nvc.Get("GradingTypes")?.GetSubStrings();
            filter.AwardingOrganisations = nvc.Get("AwardingOrganisations")?.GetSubStrings();
            filter.Availability = nvc.Get("Availability")?.GetSubStrings();
            filter.QualificationTypes = nvc.Get("QualificationTypes")?.GetSubStrings();
            filter.QualificationLevels = nvc.Get("QualificationLevels")?.GetSubStrings();
            filter.QualificationSubLevels = nvc.Get("QualificationSubLevels")?.GetSubStrings();
            filter.NationalAvailability = nvc.Get("NationalAvailability")?.GetSubStrings();
            filter.SectorSubjectAreas = nvc.Get("SectorSubjectAreas")?.GetSubStrings();

            filter.MinTotalQualificationTime = ParseInt(nvc.Get("MinTotalQualificationTime"), "minTotalQualificationTime");
            filter.MaxTotalQualificationTime = ParseInt(nvc.Get("MaxTotalQualificationTime"), "maxTotalQualificationTime");
            filter.MinGuidedLearninghours = ParseInt(nvc.Get("MinGuidedLearninghours"), "minGuidedLearninghours");
            filter.MaxGuidedLearninghours = ParseInt(nvc.Get("MaxGuidedLearninghours"), "maxGuidedLearninghours");

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

