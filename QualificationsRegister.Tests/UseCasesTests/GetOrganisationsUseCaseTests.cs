using Castle.Core.Logging;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Ofqual.Common.RegisterAPI.Models.Private;
using Ofqual.Common.RegisterAPI.Services.Cache;
using Ofqual.Common.RegisterAPI.Services.Repository;
using Ofqual.Common.RegisterAPI.UseCase;
using Ofqual.Common.RegisterAPI.UseCase.Interfaces;
using Ofqual.Common.RegisterAPI.UseCase.Organisations;

namespace Ofqual.Common.RegisterAPI.Tests.UseCase
{
    [TestFixture]
    public class GetQualificationsUseCaseTests
    {
        private Mock<IRegisterRepository> _mockRegisterRepository;
        private Mock<IRedisCacheService> _mockRedisCache;
        private GetQualificationsUseCase _classUnderTest;

        [SetUp]
        public void Setup()
        {
            _mockRegisterRepository = new Mock<IRegisterRepository>();
            _mockRedisCache = new Mock<IRedisCacheService>();
            _classUnderTest = new GetQualificationsUseCase(new NullLoggerFactory(), _mockRedisCache.Object);
        }

        [Test]
        public async Task ReturnsListOfQualifications()
        {
   
        }
    }
}
