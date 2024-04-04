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
        private readonly string _apiUrl;

        public GetOrganisationsListUseCase(ILoggerFactory loggerFactory, IRegisterDb register, ApiOptions options)
        {
            _logger = loggerFactory.CreateLogger<GetOrganisationsListUseCase>();
            _registerDb = register;
            _apiUrl = options.ApiUrl;
        }

        public ListResponse<Organisation>? ListOrganisations(string? search, int offSet, int limit)
        {
            _logger.LogInformation("Getting list of organisations");

            var _offSet = (offSet - 1) * limit;    
            if (Math.Clamp(limit, 1, 15) != limit || _offSet < 0)
            {
                throw new BadRequestException("Please use a limit size between 1 to 15 inclusive " +
                    "and page size greater than 0");
            }
               
            var (organisations, count) = _registerDb.GetOrganisationsList(limit, _offSet, search!);

            var organisationsUrl = organisations?.Select(o => o.ToResponse(_apiUrl)).ToList();

            return new ListResponse<Organisation>
            {
                Count = count,
                CurrentPage = offSet,
                Limit = limit,
                Results = organisationsUrl ?? ([])
            };
        }

    }
}
