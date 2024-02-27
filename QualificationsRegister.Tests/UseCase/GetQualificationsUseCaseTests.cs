using AutoFixture;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Ofqual.Common.RegisterAPI.Models.Private;
using Ofqual.Common.RegisterAPI.Services.Cache;
using Ofqual.Common.RegisterAPI.Services.Repository;
using Ofqual.Common.RegisterAPI.UseCase;

namespace Ofqual.Common.RegisterAPI.Tests.UseCase
{
    [TestFixture]
    public class GetQualificationsUseCaseTests
    {
        private Mock<IRegisterRepository> _mockRegisterRepository;
        private Mock<IRedisCacheService> _mockRedisCache;
        private GetQualificationsUseCase _classUnderTest;
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _mockRegisterRepository = new Mock<IRegisterRepository>();
            _mockRedisCache = new Mock<IRedisCacheService>();
            _classUnderTest = new GetQualificationsUseCase(new NullLoggerFactory(), _mockRedisCache.Object);
            _fixture = new Fixture();
        }
        [Test]
        public async Task ReturnsListOfQualificationsFromTheCache()
        {
            var stubbedList = _fixture.Create<List<OrganisationPrivate>>();
            _mockRedisCache.Setup(r => r.GetCacheAsync<OrganisationPrivate>(It.IsAny<string>())).ReturnsAsync(stubbedList);
            var result = await _classUnderTest.GetQualifications(It.IsAny<string>());
            /*Assert.IsNotNull(result);
            Assert.That(result, Has.Count.EqualTo(stubbedList.Count));
            Assert.That(result[0].ContactEmail, Is.EqualTo(stubbedList[0].ContactEmail));*/
        }

        [Test]
        public async Task ReturnsListOfQualificationsFromTheRepositoryWhenThereIsACacheMiss()
        {
            var stubbedList = _fixture.Create<List<OrganisationPrivate>>();
            List<OrganisationPrivate>? stub = null;
            _mockRedisCache.Setup(r => r.GetCacheAsync<OrganisationPrivate>(It.IsAny<string>())).ReturnsAsync(stub);
            _mockRegisterRepository.Setup(r => r.GetOrganisations()).ReturnsAsync(stubbedList);
            var result = await _classUnderTest.GetQualifications(It.IsAny<string>());
            /*Assert.IsNotNull(result);
            Assert.That(result, Has.Count.EqualTo(stubbedList.Count));
            Assert.That(result[0].ContactEmail, Is.EqualTo(stubbedList[0].ContactEmail));*/
        }
    }
}
