using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ofqual.Common.RegisterAPI.Database;
using Ofqual.Common.RegisterAPI.Models;
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

        public List<Organisation>? ListOrganisations(string search)
        {
            _logger.LogInformation("Getting list of orgs");
            return _registerDb.GetOrganisationsList(search);
        }

    }
}
