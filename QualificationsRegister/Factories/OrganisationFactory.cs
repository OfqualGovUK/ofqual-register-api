using Ofqual.Common.RegisterAPI.Models;
using Ofqual.Common.RegisterAPI.Models.DB;

namespace Ofqual.Common.RegisterAPI.Factories
{
    public static class OrganisationFactory
    {

        public static List<Organisation> ToDomain(this List<MDDBOrganisation> organisations)
        {
            return organisations.Select(o => o.ToDomain()).ToList();    
        }
    }
}
