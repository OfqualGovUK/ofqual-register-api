using Ofqual.Common.RegisterAPI.Models;
using Ofqual.Common.RegisterAPI.Models.DB;

namespace Ofqual.Common.RegisterAPI.UseCase.Interfaces
{
    public interface IGetQualificationsListUseCase
    {
        ListResponse<QualificationPublic> ListQualificationsPublic(int page, int limit, string? title);
        ListResponse<Qualification> ListQualificationsPrivate(int page, int limit, string? title);
    }
}
