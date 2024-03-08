using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ofqual.Common.RegisterAPI.Models.DB;
using Ofqual.Common.RegisterAPI.Services.Database;
using Ofqual.Common.RegisterAPI.UseCase.Interfaces;

namespace Ofqual.Common.RegisterAPI.UseCase.Organisations

{
    public class GetOrganisationByNumberUseCase : IGetOrganisationByNumberUseCase
    {
        private readonly RegisterDbContext _registerDbContext;
        private readonly ILogger _logger;

        public GetOrganisationByNumberUseCase(ILoggerFactory loggerFactory, RegisterDbContext registerDbContext)
        {
            _logger = loggerFactory.CreateLogger<GetOrganisationByNumberUseCase>();
            _registerDbContext = registerDbContext;
        }

        public async Task<Organisation?> GetOrganisationByNumber(string number)
        {
            return await _registerDbContext.Organisations.FirstOrDefaultAsync();
        }

    }
}
