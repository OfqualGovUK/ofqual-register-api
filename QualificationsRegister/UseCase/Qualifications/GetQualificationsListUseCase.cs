using Microsoft.Extensions.Logging;
using Ofqual.Common.RegisterAPI.Database;
using Ofqual.Common.RegisterAPI.Mappers;
using Ofqual.Common.RegisterAPI.Models;
using Ofqual.Common.RegisterAPI.UseCase.Interfaces;

namespace Ofqual.Common.RegisterAPI.UseCase.Qualifications
{
    public class GetQualificationsListUseCase : IGetQualificationsListUseCase
    {
        private readonly ILogger _logger;
        private readonly IRegisterDb _registerDb;

        public GetQualificationsListUseCase(ILoggerFactory loggerFactory, IRegisterDb registerdb)
        {
            _logger = loggerFactory.CreateLogger<GetQualificationsListUseCase>();
            _registerDb = registerdb;
        }

        public ListResponse<QualificationPublic> ListQualificationsPublic(int page, int limit, QualificationFilter? query, string? title)
        {
            _logger.LogInformation("Getting list of public qualifications");

            var dbResponse = _registerDb.GetQualificationsPublicByName(page - 1, limit, query, title!);

            return new ListResponse<QualificationPublic> { Count = dbResponse.Count, CurrentPage = page, Limit = limit, Results = dbResponse.Results?.ToDomain() };
        }

        public ListResponse<Qualification> ListQualificationsPrivate(int page, int limit, QualificationFilter? query, string? title)
        {
            _logger.LogInformation("Getting list of qualifications");

            var dbResponse = _registerDb.GetQualificationsByName(page - 1, limit, query, title!);

            return new ListResponse<Qualification> { Count = dbResponse.Count, CurrentPage = page, Limit = limit, Results = dbResponse.Results?.ToDomain() };
        }

    }
}
