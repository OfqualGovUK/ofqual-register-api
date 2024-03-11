using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ofqual.Common.RegisterAPI.Database;
using Ofqual.Common.RegisterAPI.Models;
using Ofqual.Common.RegisterAPI.UseCase.Interfaces;
using System.Text.RegularExpressions;

namespace Ofqual.Common.RegisterAPI.UseCase.Organisations
{
    public class GetOrganisationsUseCase : IGetOrganisationsUseCase
    {
        private readonly ILogger _logger;
        private readonly IRegisterDb _registerDb;

        public GetOrganisationsUseCase(ILoggerFactory loggerFactory, IRegisterDb register)
        {
            _logger = loggerFactory.CreateLogger<GetOrganisationsUseCase>();
            _registerDb = register;
        }

        public async Task<List<Organisation>> ListOrganisations(string search)
        {
            string RNnumber, number = string.Empty;
            string nameoracronym = string.Empty;

            if(Regex.IsMatch(search, @"^\d+$") )
            {
                number = search;
                RNnumber = $"RN{search}";
            }
            else if(search.Substring(0,2).ToLower().Equals("rn") &&
                Regex.IsMatch(search.Substring(2), @"^\d+$"))
            {
                number = search.Substring(2);
                RNnumber = search;
            }
            else
                nameoracronym = search;
                

            _logger.LogInformation("Getting list of orgs");
            return _registerDb.GetOrganisationsList(number, nameoracronym);
        }

    }
}
