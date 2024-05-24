using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ofqual.Common.RegisterAPI.Constants
{
    //to format the names of the filters as sent through in the HTTP Request for the Quals
    public class QualificationFilterConstants
    {
        public const string ASSESSMENT_METHODS_FILTER = "AssessmentMethods";

      public const string GRADING_TYPE_FILTER =  "GradingTypes";
      public const string AWARDING_ORGANISATIONS_FILTER =   "AwardingOrganisations";
      public const string AVAILABILITY_FILTER =  "Availability";
      public const string QUALIFICATION_TYPES_FILTER =   "QualificationTypes";
      public const string QUALIFICATION_LEVELS_FILTER =   "QualificationLevels";
      public const string QUALIFICATION_SUB_LEVELS_FILTER = "QualificationSubLevels";
      public const string NATIONAL_AVAILABILITY_FILTER =  "NationalAvailability";
      public const string SSA_FILTER =  "SectorSubjectAreas";
      public const string MIN_TQT_FILTER =  "MinTotalQualificationTime";
      public const string MAX_TQT_FILTER =  "MaxTotalQualificationTime";
      public const string MIN_GLH_FILTER =  "MinGuidedLearninghours";
      public const string MAX_GLH_FILTER =  "MaxGuidedLearninghours";
    }
}
