using Ofqual.Common.RegisterAPI.Models;
using Ofqual.Common.RegisterAPI.UseCase.Interfaces;

namespace Ofqual.Common.RegisterAPI.UseCase
{
    public class GetQualificationsUseCase : IGetQualificationsUseCase
    {

        public Task<List<Qualification>> GetQualifications(string qualificationName)
        {
            throw new NotImplementedException();
        }
    }
}
