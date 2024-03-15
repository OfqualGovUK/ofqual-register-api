using Ofqual.Common.RegisterAPI.Models;
using Ofqual.Common.RegisterAPI.Models.Response;

namespace Ofqual.Common.RegisterAPI.UseCase.Interfaces
{
    public interface IGetOrganisationsListUseCase
    {
        ListOrganisationsResponse ListOrganisations(string? search, int offSet = 0, int limit = 15);
    }
}
