using Ofqual.Common.RegisterAPI.UseCase.Interfaces;

namespace Ofqual.Common.RegisterAPI.UseCase
{
    public class GetOrganisationsSearchUseCase : IGetOrganisationsSearchUseCase
    {
        public Task<List<string>> GetSearchedOrganisations(string organisationName)
        {
            return Task.FromResult(new List<string>());
        }
    }
}
