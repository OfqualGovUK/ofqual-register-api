using Ofqual.Common.RegisterAPI.Models;

namespace Ofqual.Common.RegisterAPI.UseCase.Interfaces
{
    public interface IGetOrganisationByReferenceUseCase
    {
        Task<Organisation> GetOrganisationByReference(string reference);
    }
}
