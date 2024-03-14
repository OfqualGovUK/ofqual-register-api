using Ofqual.Common.RegisterAPI.Models;
using Ofqual.Common.RegisterAPI.Models.DB;

namespace Ofqual.Common.RegisterAPI.UseCase.Interfaces
{
    public interface IGetQualificationByNumberUseCase
    {
        QualificationPublic? GetQualificationPublicByNumber(string number);
        Qualification? GetQualificationByNumber(string number);
    }
}
