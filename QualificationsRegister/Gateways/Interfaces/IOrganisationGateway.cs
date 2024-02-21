namespace Ofqual.Common.RegisterAPI.Gateways

{
    public interface IOrganisationsGateway
    {
        Task<List<string>> GetOrganisations(string organisationName);
        Task<string> GetOrganisationByNumber(string organisationNum);
    }
}
