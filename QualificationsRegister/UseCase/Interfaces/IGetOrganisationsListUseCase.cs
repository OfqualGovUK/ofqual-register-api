using Ofqual.Common.RegisterAPI.Models;

namespace Ofqual.Common.RegisterAPI.UseCase.Interfaces
{
    public interface IGetOrganisationsListUseCase
    {
        List<Organisation>? ListOrganisations(string search);
    }
}
