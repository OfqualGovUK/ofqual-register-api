using Ofqual.Common.RegisterAPI.UseCase.Interfaces;

namespace Ofqual.Common.RegisterAPI.UseCase

{
    public class GetOrganisationByNumberUseCase : IGetOrganisationByNumberUseCase
    {
        public Task<string> GetOrganisationByNumber(string organisationNum)
        {
            return Task.FromResult(organisationNum);
        }
    }
}
