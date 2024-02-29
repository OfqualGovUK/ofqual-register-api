using Ofqual.Common.RegisterAPI.Models.Private;
using Ofqual.Common.RegisterAPI.Models.Public;

namespace Ofqual.Common.RegisterAPI.UseCase.Interfaces
{
    public interface IGetOrganisationsUseCase
    {
        Task<List<OrganisationPublic>> GetOrganisations(string search);
    }
}
