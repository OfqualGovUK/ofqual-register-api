using Microsoft.Extensions.Logging;
using Ofqual.Common.RegisterAPI.Models.DB;
using Ofqual.Common.RegisterAPI.Models.Private;
using Ofqual.Common.RegisterAPI.Models.Public;
using Ofqual.Common.RegisterAPI.Services.Cache;
using Ofqual.Common.RegisterAPI.UseCase.Interfaces;

namespace Ofqual.Common.RegisterAPI.UseCase
{
    public class GetQualificationByNumberUseCase : IGetQualificationByNumberUseCase
    {
        private readonly IRedisCacheService _redisCacheService;
        private readonly ILogger _logger;

        public GetQualificationByNumberUseCase(ILoggerFactory loggerFactory, IRedisCacheService redisCacheService)
        {
            _logger = loggerFactory.CreateLogger<GetQualificationByNumberUseCase>();
            _redisCacheService = redisCacheService;
        }

        public async Task<QualificationPublic?> GetQualificationByNumberPublic(string number)
        {
            var qualification = await GetQualificationByNumber(number);

            return qualification == null || qualification?.AppearsOnPublicRegister == false ? null : new QualificationPublic(qualification!);
        }

        public async Task<QualificationPrivate?> GetQualificationByNumberPrivate(string number)
        {
            var qualification = await GetQualificationByNumber(number);

            return qualification == null ? null : new QualificationPrivate(qualification!);
        }

        private async Task<Qualification?> GetQualificationByNumber(string number)
        {
            var qualifications = await _redisCacheService.GetCache<Qualification>("Qualifications");

            return qualifications.Where(e=>e.GetQualificationNumber().Equals(number.Replace("/",""), StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
        }
    }
}
