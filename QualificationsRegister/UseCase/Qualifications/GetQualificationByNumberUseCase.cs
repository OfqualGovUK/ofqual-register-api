using Ofqual.Common.RegisterAPI.Models;
using Ofqual.Common.RegisterAPI.UseCase.Interfaces;

namespace Ofqual.Common.RegisterAPI.UseCase
{
    public class GetQualificationByNumberUseCase : IGetQualificationByNumberUseCase
    {
        public Task<Qualification> GetQualificationByNumber(string qualificationRef)
        {
            throw new NotImplementedException();
        }
    }
}
