using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ofqual.Common.RegisterAPI.Models.DB;
using Ofqual.Common.RegisterAPI.Services.Database;
using Ofqual.Common.RegisterAPI.UseCase.Interfaces;

namespace Ofqual.Common.RegisterAPI.UseCase
{
    public class GetQualificationsUseCase : IGetQualificationsUseCase
    {
        private readonly ILogger _logger;
        private readonly RegisterDbContext _registerDbContext;

        public GetQualificationsUseCase(ILoggerFactory loggerFactory, RegisterDbContext registerContext)
        {
            _logger = loggerFactory.CreateLogger<GetQualificationsUseCase>();
            _registerDbContext = registerContext;
        }

        public async Task<List<QualificationPublic>> GetQualificationsPublic(string? search)
        {
            _logger.LogInformation("Getting list of qualifications public");

            return  await _registerDbContext.QualificationsPublic.ToListAsync();
        }

        public async Task<List<Qualification>> GetQualificationsPrivate(string? search)
        {
            _logger.LogInformation("Getting list of qualifications");

            return await _registerDbContext.Qualifications.ToListAsync();
        }

    }
}
