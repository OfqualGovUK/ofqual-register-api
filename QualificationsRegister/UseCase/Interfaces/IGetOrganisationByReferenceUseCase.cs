using Ofqual.Common.RegisterAPI.Models.Private;
using Ofqual.Common.RegisterAPI.Models.Public;

namespace Ofqual.Common.RegisterAPI.UseCase.Interfaces
{
    public interface IGetOrganisationByReferenceUseCase
    {
        Task<OrganisationPublic?> GetOrganisationByReference(string reference);
    }
}
