using Ofqual.Common.RegisterAPI.Models.DB;

namespace Ofqual.Common.RegisterAPI.UseCase.Interfaces
{
    public interface IGetOrganisationsUseCase
    {
        Task<List<Organisation>> GetOrganisations(string search);
    }
}
