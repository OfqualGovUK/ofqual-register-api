namespace Ofqual.Common.RegisterAPI.UseCase.Interfaces
{
    public interface IGetOrganisationsSearchUseCase
    {
        Task<List<string>> GetSearchedOrganisations(string organisationName);
    }
}
