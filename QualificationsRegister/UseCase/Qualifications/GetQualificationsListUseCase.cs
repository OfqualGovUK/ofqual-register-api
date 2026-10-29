
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Ofqual.Common.RegisterAPI.Database;
using Ofqual.Common.RegisterAPI.Mappers;
using Ofqual.Common.RegisterAPI.Models;
using Ofqual.Common.RegisterAPI.Models.Exceptions;
using Ofqual.Common.RegisterAPI.UseCase.Interfaces;

namespace Ofqual.Common.RegisterAPI.UseCase.Qualifications
{
    public class GetQualificationsListUseCase : IGetQualificationsListUseCase
    {
        private readonly ILogger _logger;
        private readonly IRegisterDb _registerDb;
        private readonly IConfiguration _configuration;

        public GetQualificationsListUseCase(
            ILoggerFactory loggerFactory,
            IRegisterDb registerDb,
            IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<GetQualificationsListUseCase>();
            _registerDb = registerDb;
            _configuration = configuration;
        }

        private int PagingLimit =>
            _configuration.GetValue<int?>("QualificationsPagingLimit") ?? 8000;

        public ListResponse<QualificationPublic> ListQualificationsPublic(
            int page,
            int? limit,
            QualificationFilter? query,
            string? title)
        {
            _logger.LogInformation("Getting list of public qualifications");

            ValidatePaging(page, limit);

            var dbResponse = _registerDb.GetQualificationsPublicList(
                page - 1,
                limit,
                query,
                title!);

            return new ListResponse<QualificationPublic>
            {
                Count = dbResponse.Count,
                Results = dbResponse.Results?.ToDomain(),
                CurrentPage = page,
                Limit = limit
            };
        }

        public ListResponse<Qualification> ListQualifications(
            int page,
            int? limit,
            QualificationFilter? query,
            string? title)
        {
            _logger.LogInformation("Getting list of qualifications");

            ValidatePaging(page, limit);

            var dbResponse = _registerDb.GetQualificationsList(
                page - 1,
                limit,
                query,
                title!);

            return new ListResponse<Qualification>
            {
                Count = dbResponse.Count,
                Results = dbResponse.Results?.ToDomain(),
                CurrentPage = page,
                Limit = limit
            };
        }

        private void ValidatePaging(int page, int? limit)
        {
            if (page < 1 || limit > PagingLimit || limit < 1)
            {
                throw new BadRequestException(
                    $"Invalid parameter values. Page should be > 0 and Limit should be > 0 and <= {PagingLimit}");
            }
        }
    }
}
