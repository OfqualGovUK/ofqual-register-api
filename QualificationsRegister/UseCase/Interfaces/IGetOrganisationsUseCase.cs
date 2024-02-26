using Ofqual.Common.RegisterAPI.Models.Private;

namespace Ofqual.Common.RegisterAPI.UseCase.Interfaces
{
    public interface IGetOrganisationsUseCase
    {
        Task<List<OrganisationPrivate>> GetOrganisations(string search);
    }
}
