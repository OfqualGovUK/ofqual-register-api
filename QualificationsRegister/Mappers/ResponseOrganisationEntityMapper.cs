using Ofqual.Common.RegisterAPI.Models;
using Ofqual.Common.RegisterAPI.Models.DB;

namespace Ofqual.Common.RegisterAPI.Mappers
{
    public static class ResponseOrganisationEntityMapper
    {

        public static Organisation ToResponse(this Organisation organisation, string apiUrl)
        {
            organisation.CanonicalUrl = $"{apiUrl}/api/organisations/{organisation.RecognitionNumber}";
            return organisation;
        }

    }
}
