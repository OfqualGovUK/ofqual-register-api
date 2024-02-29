using Ofqual.Common.RegisterAPI.Models.DB;
using Ofqual.Common.RegisterAPI.Models.Private;
using Ofqual.Common.RegisterAPI.Models.Public;

namespace Ofqual.Common.RegisterAPI.UseCase.Interfaces
{
    public interface IGetQualificationsUseCase
    {
        Task<List<QualificationPublic>> GetQualificationsPublic(string? search);
        Task<List<QualificationPrivate>> GetQualificationsPrivate(string? search);
    }
}
