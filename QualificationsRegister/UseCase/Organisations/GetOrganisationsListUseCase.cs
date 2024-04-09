using Microsoft.Extensions.Logging;
using Ofqual.Common.RegisterAPI.Database;
using Ofqual.Common.RegisterAPI.Mappers;
using Ofqual.Common.RegisterAPI.Models;
using Ofqual.Common.RegisterAPI.Models.Exceptions;
using Ofqual.Common.RegisterAPI.UseCase.Interfaces;

namespace Ofqual.Common.RegisterAPI.UseCase.Organisations
{
    public class GetOrganisationsListUseCase : IGetOrganisationsListUseCase
    {
        private readonly ILogger _logger;
        private readonly IRegisterDb _registerDb;

        public GetOrganisationsListUseCase(ILoggerFactory loggerFactory, IRegisterDb register)
        {
            _logger = loggerFactory.CreateLogger<GetOrganisationsListUseCase>();
            _registerDb = register;
        }

        public ListResponse<Organisation>? ListOrganisations(string? search, int page, int limit)
        {
            _logger.LogInformation("Getting list of organisations");

            if (limit < 1 || page < 1)
            {
                throw new BadRequestException($"Invalid parameter values. Page should be > 0 and Limit should be > 0");
            }

            var dbResponse = _registerDb.GetOrganisationsList(page - 1, limit, search!);

            return new ListResponse<Organisation>()
            {
                Count = dbResponse != null ? dbResponse.Count : 0,
                Results = dbResponse != null ? dbResponse?.Results?.ToDomain() : null,
                Limit = limit,
                CurrentPage = page
            };
        }

    }
}
