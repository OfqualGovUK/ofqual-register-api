using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ofqual.Common.RegisterAPI.Database;
using Ofqual.Common.RegisterAPI.Models;
using Ofqual.Common.RegisterAPI.Models.DB;
using Ofqual.Common.RegisterAPI.Services.Database;
using Ofqual.Common.RegisterAPI.UseCase.Interfaces;

namespace Ofqual.Common.RegisterAPI.UseCase
{
    public class GetQualificationsUseCase : IGetQualificationsListUseCase
    {
        private readonly ILogger _logger;
        private readonly IRegisterDb _registerDb;

        public GetQualificationsUseCase(ILoggerFactory loggerFactory, IRegisterDb registerdb)
        {
            _logger = loggerFactory.CreateLogger<GetQualificationsUseCase>();
            _registerDb = registerdb;
        }

        public List<QualificationPublic> ListQualificationsPublic(string? title)
        {
            _logger.LogInformation("Getting list of public qualifications");

            return  _registerDb.GetQualificationsPublicByName(title!);
        }

        public List<Qualification> ListQualificationsPrivate(string? title)
        {
            _logger.LogInformation("Getting list of qualifications");

            return _registerDb.GetQualificationsByName(title!);
        }

    }
}
