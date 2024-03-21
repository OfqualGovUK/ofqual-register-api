using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ofqual.Common.RegisterAPI.Database;
using Ofqual.Common.RegisterAPI.Models;
using Ofqual.Common.RegisterAPI.Models.DB;
using Ofqual.Common.RegisterAPI.Services.Database;
using Ofqual.Common.RegisterAPI.UseCase.Interfaces;

namespace Ofqual.Common.RegisterAPI.UseCase
{
    public class GetQualificationsUseCase : IGetQualificationsListUseCase
    {
        private readonly ILogger _logger;
        private readonly IRegisterDb _registerDb;

        public GetQualificationsUseCase(ILoggerFactory loggerFactory, IRegisterDb registerdb)
        {
            _logger = loggerFactory.CreateLogger<GetQualificationsUseCase>();
            _registerDb = registerdb;
        }

        public ListResponse<QualificationPublic> ListQualificationsPublic(int page, int limit, QualificationFilter? query, string? title)
        {
            _logger.LogInformation("Getting list of public qualifications");

            return _registerDb.GetQualificationsPublicByName(page, limit, query, title!);
        }

        public ListResponse<Qualification> ListQualificationsPrivate(int page, int limit, string? title)
        {
            _logger.LogInformation("Getting list of qualifications");

            return _registerDb.GetQualificationsByName(page, limit, title!);
        }

    }
}
