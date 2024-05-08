using Ofqual.Common.RegisterAPI.Models;

namespace Ofqual.Common.RegisterAPI.UseCase.Interfaces
{
    public interface IGetScopesByOrganisationNumberUseCase
    {
        Task<RecognitionScope> GetScopesByOrganisationNumber(string? recognitionNumber);
    }
}
