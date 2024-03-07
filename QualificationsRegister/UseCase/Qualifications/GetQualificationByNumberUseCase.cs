using Microsoft.Extensions.Logging;
using Ofqual.Common.RegisterAPI.Models.DB;
using Ofqual.Common.RegisterAPI.Models.Private;
using Ofqual.Common.RegisterAPI.Models.Public;
using Ofqual.Common.RegisterAPI.Repository;
using Ofqual.Common.RegisterAPI.Services;
using Ofqual.Common.RegisterAPI.UseCase.Interfaces;

namespace Ofqual.Common.RegisterAPI.UseCase
{
    public class GetQualificationByNumberUseCase : IGetQualificationByNumberUseCase
    {
        private readonly IRedisCacheService _redisCacheService;
        private readonly ILogger _logger;
        private readonly IRegisterRepository _registerRepository;

        public GetQualificationByNumberUseCase(ILoggerFactory loggerFactory, IRedisCacheService redisCacheService, IRegisterRepository repository)
        {
            _logger = loggerFactory.CreateLogger<GetQualificationByNumberUseCase>();
            _redisCacheService = redisCacheService;
            _registerRepository = repository;
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
            var qualifications = await _redisCacheService.GetAndSetCacheAsync(RedisCache.Qualifications, async () =>
            {
                return await _registerRepository.GetQualifications();
            });
            return qualifications.FirstOrDefault(e => e.GetQualificationNumber().Equals(number.Replace("/", ""), StringComparison.CurrentCultureIgnoreCase));
        }
    }
}
