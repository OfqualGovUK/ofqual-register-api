using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ofqual.Common.RegisterAPI.Database;
using Ofqual.Common.RegisterAPI.Models;
using Ofqual.Common.RegisterAPI.Models.Response;
using Ofqual.Common.RegisterAPI.UseCase.Interfaces;
using System.Text.RegularExpressions;

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

        public ListOrganisationsResponse ListOrganisations(string? search, int offSet = 1, int limit = 15)
        {
            _logger.LogInformation("Getting list of organisations");
            var _offSet = (offSet - 1) * limit;

            var organisations = _registerDb.GetOrganisationsList(limit, _offSet, search!);

            return new ListOrganisationsResponse
            {
                Organisations = organisations,
                NextPage = organisations?.Count == limit ? Convert.ToString(offSet + 1) : null
            };
        }

    }
}
