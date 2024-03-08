using Ofqual.Common.RegisterAPI.Models.DB;

namespace Ofqual.Common.RegisterAPI.UseCase.Interfaces
{
    public interface IGetOrganisationByNumberUseCase
    {
        Task<Organisation?> GetOrganisationByNumber(string reference);
    }
}
