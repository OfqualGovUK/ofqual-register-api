using Ofqual.Common.RegisterAPI.Models;

namespace Ofqual.Common.RegisterAPI.UseCase.Interfaces
{
    public interface IGetOrganisationByNumberUseCase
    {
        Task<Organisation?> GetOrganisationByNumber(string reference);
    }
}
