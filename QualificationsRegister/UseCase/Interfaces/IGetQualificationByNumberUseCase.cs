using Ofqual.Common.RegisterAPI.Models.Private;
using Ofqual.Common.RegisterAPI.Models.Public;

namespace Ofqual.Common.RegisterAPI.UseCase.Interfaces
{
    public interface IGetQualificationByNumberUseCase
    {
        Task<QualificationPublic?> GetQualificationByNumber(string number);
        Task<QualificationPrivate?> GetQualificationByNumberPrivate(string number);
    }
}