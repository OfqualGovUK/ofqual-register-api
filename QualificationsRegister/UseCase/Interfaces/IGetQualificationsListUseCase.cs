using Ofqual.Common.RegisterAPI.Models;
using Ofqual.Common.RegisterAPI.Models.DB;

namespace Ofqual.Common.RegisterAPI.UseCase.Interfaces
{
    public interface IGetQualificationsListUseCase
    {
        ListResponse<Qualification> ListQualifications(int page, int? limit, QualificationFilter? query, string? title);
        ListResponse<QualificationPublic> ListQualificationsPublic(int page, int? limit, QualificationFilter? query, string? title);
    }
}
