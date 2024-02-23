using Microsoft.Extensions.Logging;
using Ofqual.Common.RegisterAPI.Models;
using Ofqual.Common.RegisterAPI.Services.Cache;
using Ofqual.Common.RegisterAPI.UseCase.Interfaces;
using Ofqual.Common.RegisterAPI.UseCase.Organisations;

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

        public async Task<Qualification> GetQualificationByNumber(string number)
        {
            var qualifications = await _redisCacheService.GetCache<Qualification>("Qualifications");
            var qualificationByNumber = qualifications.FirstOrDefault(e=>e.QualificationNumberNoObliques  == number);

            return qualificationByNumber;
        }
    }
}
