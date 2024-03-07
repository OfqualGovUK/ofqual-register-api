using Ofqual.Common.RegisterAPI.Models.DB;

namespace Ofqual.Common.RegisterAPI.UseCase.Interfaces
{
    public interface IGetQualificationsUseCase
    {
        Task<List<QualificationPublic>> GetQualificationsPublic(string? search);
        Task<List<Qualification>> GetQualificationsPrivate(string? search);
    }
}
