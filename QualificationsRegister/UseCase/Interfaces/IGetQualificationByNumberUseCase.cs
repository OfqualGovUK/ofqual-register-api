using Ofqual.Common.RegisterAPI.Models;

namespace Ofqual.Common.RegisterAPI.UseCase.Interfaces
{
    public interface IGetQualificationByNumberUseCase
    {
        Task<Qualification> GetQualificationByNumber(string qualificationNum);
    }
}
