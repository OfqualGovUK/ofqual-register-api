namespace QualificationsRegister.UseCase.Interfaces
{
    public interface IGetOrganisationByNumberUseCase
    {
        Task<string> GetOrganisationByNumber(string organisationNum);
    }
}
