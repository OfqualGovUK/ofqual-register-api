using AutoFixture;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Ofqual.Common.RegisterAPI.Models.DB;
using Ofqual.Common.RegisterAPI.Models.Public;
using Ofqual.Common.RegisterAPI.Repository;
using Ofqual.Common.RegisterAPI.Services;
using Ofqual.Common.RegisterAPI.UseCase.Organisations;
using FluentAssertions;

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
            _classUnderTest = new GetOrganisationsUseCase(new NullLoggerFactory(), _mockRedisCache.Object,
                _mockRegisterRepository.Object);
            _fixture = new Fixture();
        }

        [Test]
        [Ignore("not complete")]
        public async Task ReturnsListOfOrganisationsFromTheCache()
        {
            var stubbedListPublic = _fixture.Create<List<OrganisationPublic>>();
            var stubbedList = _fixture.Create<IEnumerable<Organisation>>();


            _mockRegisterRepository.Setup(r => r.GetOrganisations()).ReturnsAsync(stubbedList);
            var mockFunc = new Mock<Func<Task<IEnumerable<Organisation>>>>();
            //mockFunc.Setup(m => m()).ReturnsAsync(stubbedList);

            _mockRedisCache.Setup(r => r.GetAndSetCacheAsync(It.IsAny<string>(), mockFunc.Object, 1));
            
            var result = await _classUnderTest.GetOrganisations(It.IsAny<string>());

            result.Should().NotBeNull();
            //    Assert.That(result, Has.Count.EqualTo(stubbedList.Count));
            //    Assert.That(result[0].ContactEmail, Is.EqualTo(stubbedList[0].ContactEmail));
        }


        [Test]
        [Ignore("not complete")]
        public async Task ReturnsListOfOrganisationsFromTheRepositoryWhenThereIsACacheMiss()
        {
            var stubbedList = _fixture.Create<IEnumerable<Organisation>>();
            List<OrganisationPublic>? stub = null;

            _mockRedisCache.Setup(r => r.GetCacheAsync<OrganisationPublic>(It.IsAny<string>())).ReturnsAsync(stub);
            _mockRegisterRepository.Setup(r => r.GetOrganisations()).ReturnsAsync(stubbedList);
            var result = await _classUnderTest.GetOrganisations(It.IsAny<string>());
            result.Should().NotBeNull();
            result.ToList().Should().BeEquivalentTo(stubbedList.ToList());
        }
    }
}
