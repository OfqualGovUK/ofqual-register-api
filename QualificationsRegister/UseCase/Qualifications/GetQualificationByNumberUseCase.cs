using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ofqual.Common.RegisterAPI.Models.DB;
using Ofqual.Common.RegisterAPI.Services.Database;
using Ofqual.Common.RegisterAPI.UseCase.Interfaces;

namespace Ofqual.Common.RegisterAPI.UseCase
{
    public class GetQualificationByNumberUseCase : IGetQualificationByNumberUseCase
    {
        private readonly RegisterDbContext _registerDbContext;
        private readonly ILogger _logger;

        public GetQualificationByNumberUseCase(ILoggerFactory loggerFactory, RegisterDbContext registerDbContext)
        {
            _logger = loggerFactory.CreateLogger<GetQualificationByNumberUseCase>();
            _registerDbContext = registerDbContext;
        }

        public async Task<QualificationPublic?> GetQualificationByNumberPublic(string number)
        {
            return await _registerDbContext
                .QualificationsPublic
                .FirstOrDefaultAsync();
        }

        public async Task<Qualification?> GetQualificationByNumber(string number)
        {
            return await _registerDbContext
                .Qualifications.FirstOrDefaultAsync();
        }
    }
}
