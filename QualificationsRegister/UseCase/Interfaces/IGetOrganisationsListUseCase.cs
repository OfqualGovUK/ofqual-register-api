using Ofqual.Common.RegisterAPI.Models;
using Ofqual.Common.RegisterAPI.Models.Response;

namespace Ofqual.Common.RegisterAPI.UseCase.Interfaces
{
    public interface IGetOrganisationsListUseCase
    {
        ListResponse<Organisation>? ListOrganisations(string? search, int offSet, int limit);
    }
}
