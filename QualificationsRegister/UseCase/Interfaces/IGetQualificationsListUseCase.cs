using Ofqual.Common.RegisterAPI.Models;
using Ofqual.Common.RegisterAPI.Models.DB;

namespace Ofqual.Common.RegisterAPI.UseCase.Interfaces
{
    public interface IGetQualificationsListUseCase
    {
        List<QualificationPublic> ListQualificationsPublic(string? title);
        List<Qualification> ListQualificationsPrivate(string? title);
    }
}
