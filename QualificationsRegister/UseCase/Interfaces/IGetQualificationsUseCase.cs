using Ofqual.Common.RegisterAPI.Models.DB;
using Ofqual.Common.RegisterAPI.Models.Private;
using Ofqual.Common.RegisterAPI.Models.Public;

namespace Ofqual.Common.RegisterAPI.UseCase.Interfaces
{
    public interface IGetQualificationsUseCase
    {
        Task<IEnumerable<Qualification>> GetQualifications(string? search);
        Task<IEnumerable<QualificationPrivate>> GetQualificationsPrivate(string? search);
    }
}