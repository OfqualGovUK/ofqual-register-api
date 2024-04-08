using Microsoft.Extensions.Logging;
using Ofqual.Common.RegisterAPI.Database;
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

        public ListResponse<Organisation>? ListOrganisations(string? search, int offSet, int limit)
        {
            _logger.LogInformation("Getting list of organisations");

            var _offSet = (offSet - 1) * limit;    
            if (Math.Clamp(limit, 1, 15) != limit || _offSet < 0)
            {
                throw new BadRequestException("Please use a limit size between 1 to 15 inclusive " +
                    "and page size greater than 0");
            }
               
            return _registerDb.GetOrganisationsList(limit, offSet, search!);
        }

    }
}
