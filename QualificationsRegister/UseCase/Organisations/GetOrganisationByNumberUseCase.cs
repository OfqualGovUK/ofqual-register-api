using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ofqual.Common.RegisterAPI.Models.DB;
using Ofqual.Common.RegisterAPI.Services.Database;
using Ofqual.Common.RegisterAPI.UseCase.Interfaces;

namespace Ofqual.Common.RegisterAPI.UseCase.Organisations

{
    public class GetOrganisationByNumberUseCase : IGetOrganisationByNumberUseCase
    {
        private readonly IRegisterDb _registerDb;
        private readonly ILogger _logger;

        public GetOrganisationByNumberUseCase(ILoggerFactory loggerFactory, IRegisterDb registerDb)
        {
            _logger = loggerFactory.CreateLogger<GetOrganisationByNumberUseCase>();
            _registerDb = registerDb;
        }

        public async Task<Organisation?> GetOrganisationByNumber(string number)
        {
            var results = await _registerDb.GetOrganisations();

            return results.FirstOrDefault();
        }

    }
}
