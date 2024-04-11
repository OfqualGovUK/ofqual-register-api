using Azure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ofqual.Common.RegisterAPI.Database;
using Ofqual.Common.RegisterAPI.Mappers;
using Ofqual.Common.RegisterAPI.Models;
using Ofqual.Common.RegisterAPI.Models.DB;
using Ofqual.Common.RegisterAPI.Models.Exceptions;
using Ofqual.Common.RegisterAPI.Services.Database;
using Ofqual.Common.RegisterAPI.UseCase.Interfaces;
using System.Net;

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

            if (!int.TryParse(Environment.GetEnvironmentVariable("QualificationsPagingLimit"), out var pagingLimit))
            {
                pagingLimit = 100;
            }

            if (page < 1 || limit > pagingLimit || limit < 1)
            {
                throw new BadRequestException($"Invalid parameter values. Page should be > 0 and Limit should be > 0 and <= {pagingLimit}");
            }

            var dbResponse = _registerDb.GetQualificationsPublicByName(page - 1, limit, query, title!);

            return new ListResponse<QualificationPublic>
            {
                Count = dbResponse.Count,
                Results = dbResponse.Results?.ToDomain(),
                CurrentPage = page,
                Limit = limit
            };
        }

        public ListResponse<Qualification> ListQualificationsPrivate(int page, int limit, QualificationFilter? query, string? title)
        {
            _logger.LogInformation("Getting list of qualifications");

            if (!int.TryParse(Environment.GetEnvironmentVariable("QualificationsPagingLimit"), out var pagingLimit))
            {
                pagingLimit = 100;
            }

            if (page < 1 || limit > pagingLimit || limit < 1)
            {
                throw new BadRequestException($"Invalid parameter values. Page should be > 0 and Limit should be > 0 and <= {pagingLimit}");
            }

            var dbResponse = _registerDb.GetQualificationsByName(page-1, limit, query, title!);

            return new ListResponse<Qualification>
            {
                Count = dbResponse.Count,
                Results = dbResponse.Results?.ToDomain(),
                CurrentPage = page,
                Limit = limit
            };
        }

    }
}
