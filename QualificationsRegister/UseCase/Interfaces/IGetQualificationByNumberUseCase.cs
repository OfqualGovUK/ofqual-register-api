using Ofqual.Common.RegisterAPI.Models;
using Ofqual.Common.RegisterAPI.Models.DB;

namespace Ofqual.Common.RegisterAPI.UseCase.Interfaces
{
    public interface IGetQualificationByNumberUseCase
    {
        QualificationPublic? GetQualificationPublicByNumber(string number, string? number2, string? number3);
        Qualification? GetQualificationByNumber(string number, string? number2, string? number3);
    }
}
