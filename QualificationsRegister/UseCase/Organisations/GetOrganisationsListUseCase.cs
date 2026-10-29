using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration _configuration;

        public GetOrganisationsListUseCase(ILoggerFactory loggerFactory, IRegisterDb register, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<GetOrganisationsListUseCase>();
            _registerDb = register;
            _configuration = configuration;
        }

        public ListResponse<Organisation>? ListOrganisations(string? search, int? limit, int page)
        {
            _logger.LogInformation("Getting list of organisations");

            if (page < 1)
            {
                throw new BadRequestException($"Invalid parameter values. Page should be > 0");
            }

            var dbResponse = _registerDb.GetOrganisationsList(page - 1, limit, search!);

            return new ListResponse<Organisation>()
            {
                Count = dbResponse != null ? dbResponse.Count : 0,
                Results = dbResponse?.Results?
                    .ToDomain()
                    .Select(PopulateCanonicalUrl)
                    .ToList(),
                Limit = limit,
                CurrentPage = page
            };
        }

        private Organisation PopulateCanonicalUrl(Organisation org)
        {
            string apiUrl = _configuration.GetValue<string>("APIMgmtURL")!;

            org.CanonicalUrl =
                $"{apiUrl}/api/organisations/{org.RecognitionNumber}";

            return org;
        }
    }
}
