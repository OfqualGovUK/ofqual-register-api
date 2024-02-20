using QualificationsRegister.UseCase.Interfaces;

namespace QualificationsRegister.UseCase
{
    public class GetOrganisationsSearchUseCase : IGetOrganisationsSearchUseCase
    {
        public Task<List<string>> GetSearchedOrganisations(string organisationName)
        {
            return Task.FromResult(new List<string>());
        }
    }
}
