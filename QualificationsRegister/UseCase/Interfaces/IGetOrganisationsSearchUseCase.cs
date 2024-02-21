namespace QualificationsRegister.UseCase.Interfaces
{
    public interface IGetOrganisationsSearchUseCase
    {
        Task<List<string>> GetSearchedOrganisations(string organisationName);
    }
}
