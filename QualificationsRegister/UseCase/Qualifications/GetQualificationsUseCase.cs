using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ofqual.Common.RegisterAPI.Database;
using Ofqual.Common.RegisterAPI.Models.DB;
using Ofqual.Common.RegisterAPI.UseCase.Interfaces;

namespace Ofqual.Common.RegisterAPI.UseCase
{
    public class GetQualificationsUseCase : IGetQualificationsUseCase
    {
        private readonly ILogger _logger;
        private readonly IRegisterDb _registerDb;

        public GetQualificationsUseCase(ILoggerFactory loggerFactory, IRegisterDb registerdb)
        {
            _logger = loggerFactory.CreateLogger<GetQualificationsUseCase>();
            _registerDb = registerdb;
        }

        public async Task<List<QualificationPublic>> GetQualificationsPublic(string? search)
        {
            _logger.LogInformation("Getting list of qualifications public");

            return  await _registerDb.GetQualificationsPublic(search!);
        }

        public async Task<List<Qualification>> GetQualificationsPrivate(string? search)
        {
            _logger.LogInformation("Getting list of qualifications");

            return await _registerDb.GetQualifications(search!);
        }

    }
}
