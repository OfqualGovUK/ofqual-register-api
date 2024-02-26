using Ofqual.Common.RegisterAPI.Models.Private;

namespace Ofqual.Common.RegisterAPI.UseCase.Interfaces
{
    public interface IGetOrganisationsUseCase
    {
        Task<IEnumerable<OrganisationPrivate>> GetOrganisations(string search);
    }
}
