namespace Ofqual.Common.RegisterAPI.Gateways
{
    public interface IQualificationGateway
    {
        Task<List<string>> GetQualifications();
    }
}
