using Castle.Core.Logging;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Ofqual.Common.RegisterAPI.Models.Private;
using Ofqual.Common.RegisterAPI.Services.Cache;
using Ofqual.Common.RegisterAPI.Services.Repository;
using Ofqual.Common.RegisterAPI.UseCase.Interfaces;
using Ofqual.Common.RegisterAPI.UseCase.Organisations;

namespace Ofqual.Common.RegisterAPI.Tests.UseCase
{
    [TestFixture]
    public class GetOrganisationsUseCaseTests 
    {
        private Mock<IRegisterRepository> _mockRegisterRepository;
        private Mock<IRedisCacheService> _mockRedisCache;
        private GetOrganisationsUseCase _classUnderTest;

        [SetUp]
        public void Setup()
        {
            _mockRegisterRepository = new Mock<IRegisterRepository>();
            _mockRedisCache = new Mock<IRedisCacheService>();
            _classUnderTest = new GetOrganisationsUseCase(new NullLoggerFactory(), _mockRegisterRepository.Object,
                _mockRedisCache.Object);
        }

        [Test]
        public async Task ReturnsListOfOrganisations()
        {
   
        }
    }
}
