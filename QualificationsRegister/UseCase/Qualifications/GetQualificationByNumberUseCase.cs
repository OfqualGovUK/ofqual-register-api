using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ofqual.Common.RegisterAPI.Models.DB;
using Ofqual.Common.RegisterAPI.Services.Database;
using Ofqual.Common.RegisterAPI.UseCase.Interfaces;

namespace Ofqual.Common.RegisterAPI.UseCase
{
    public class GetQualificationByNumberUseCase : IGetQualificationByNumberUseCase
    {
        private readonly IRegisterDb _registerDb;
        private readonly ILogger _logger;

        public GetQualificationByNumberUseCase(ILoggerFactory loggerFactory, IRegisterDb registerDb)
        {
            _logger = loggerFactory.CreateLogger<GetQualificationByNumberUseCase>();
            _registerDb = registerDb;
        }

        public async Task<QualificationPublic?> GetQualificationByNumberPublic(string number)
        {
            var results = await _registerDb.GetQualificationsPublic();

            return results.FirstOrDefault();
        }

        public async Task<Qualification?> GetQualificationByNumber(string number)
        {
            var results = await _registerDb.GetQualifications();

            return results.FirstOrDefault();
        }
    }
}
