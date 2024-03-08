using Ofqual.Common.RegisterAPI.Models.DB;

namespace Ofqual.Common.RegisterAPI.UseCase.Interfaces
{
    public interface IGetQualificationByNumberUseCase
    {
        Task<QualificationPublic?> GetQualificationByNumberPublic(string number);
        Task<Qualification?> GetQualificationByNumber(string number);
    }
}
