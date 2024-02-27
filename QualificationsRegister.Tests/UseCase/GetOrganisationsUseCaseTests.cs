using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Ofqual.Common.RegisterAPI.Services.Cache;
using Ofqual.Common.RegisterAPI.Services.Repository;
using Ofqual.Common.RegisterAPI.UseCase.Organisations;
using AutoFixture;
using Ofqual.Common.RegisterAPI.Models.Private;

namespace Ofqual.Common.RegisterAPI.Tests.UseCase
{
    [TestFixture]
    public class GetOrganisationsUseCaseTests 
    {
        private Mock<IRegisterRepository> _mockRegisterRepository;
        private Mock<IRedisCacheService> _mockRedisCache;
        private GetOrganisationsUseCase _classUnderTest;
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _mockRegisterRepository = new Mock<IRegisterRepository>();
            _mockRedisCache = new Mock<IRedisCacheService>();
            _classUnderTest = new GetOrganisationsUseCase(new NullLoggerFactory(), _mockRegisterRepository.Object,
                _mockRedisCache.Object);
            _fixture = new Fixture();
        }

        [Test]
        public async Task ReturnsListOfOrganisationsFromTheCache()
        {
            var stubbedList = _fixture.Create<List<OrganisationPrivate>>();
            _mockRedisCache.Setup(r => r.GetCacheAsync<OrganisationPrivate>(It.IsAny<string>())).ReturnsAsync(stubbedList);
            var result = await _classUnderTest.GetOrganisations(It.IsAny<string>());
            Assert.IsNotNull(result);
            Assert.That(result, Has.Count.EqualTo(stubbedList.Count));
            Assert.That(result[0].ContactEmail, Is.EqualTo(stubbedList[0].ContactEmail));
        }

        [Test]
        public async Task ReturnsListOfOrganisationsFromTheRepositoryWhenThereIsACacheMiss()
        {
            var stubbedList = _fixture.Create<List<OrganisationPrivate>>();
            List<OrganisationPrivate>? stub = null;
            _mockRedisCache.Setup(r => r.GetCacheAsync<OrganisationPrivate>(It.IsAny<string>())).ReturnsAsync(stub);
            _mockRegisterRepository.Setup(r => r.GetOrganisations()).ReturnsAsync(stubbedList);
            var result = await _classUnderTest.GetOrganisations(It.IsAny<string>());
            Assert.IsNotNull(result);
            Assert.That(result, Has.Count.EqualTo(stubbedList.Count));
            Assert.That(result[0].ContactEmail, Is.EqualTo(stubbedList[0].ContactEmail));
        }
    }
}
