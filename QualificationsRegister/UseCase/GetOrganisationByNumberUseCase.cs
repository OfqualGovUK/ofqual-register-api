using QualificationsRegister.UseCase.Interfaces;

namespace QualificationsRegister.UseCase

{
    public class GetOrganisationByNumberUseCase : IGetOrganisationByNumberUseCase
    {
        public Task<string> GetOrganisationByNumber(string organisationNum)
        {
            return Task.FromResult(organisationNum);
        }
    }
}
