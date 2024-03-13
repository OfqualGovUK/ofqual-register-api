using Ofqual.Common.RegisterAPI.Models;
using Ofqual.Common.RegisterAPI.Models.DB;

namespace Ofqual.Common.RegisterAPI.Mappers
{
    public static class OrganisationMapper
    {

        public static List<Organisation> ToDomain(this List<DbOrganisation> organisations)
        {
            return organisations.Select(o => o.ToDomain()).ToList();    
        }
    }
}
