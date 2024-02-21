namespace Ofqual.Common.RegisterAPI.UseCase.Interfaces
{
    public interface IGetOrganisationByNumberUseCase
    {
        Task<string> GetOrganisationByNumber(string organisationNum);
    }
}
