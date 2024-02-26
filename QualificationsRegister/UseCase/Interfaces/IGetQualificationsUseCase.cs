using Ofqual.Common.RegisterAPI.Models.Private;

namespace Ofqual.Common.RegisterAPI.UseCase.Interfaces
{
    public interface IGetQualificationsUseCase
    {
        Task<List<QualificationPrivate>> GetQualifications(string search);
    }
}
