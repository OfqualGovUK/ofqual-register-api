using Ofqual.Common.RegisterAPI.Models.Private;
using Ofqual.Common.RegisterAPI.Models.Public;

namespace Ofqual.Common.RegisterAPI.UseCase.Interfaces
{
    public interface IGetOrganisationByNumberUseCase
    {
        Task<OrganisationPublic?> GetOrganisationByNumber(string reference);
    }
}
