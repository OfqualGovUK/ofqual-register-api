using AutoFixture;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Ofqual.Common.RegisterAPI.Database;
using Ofqual.Common.RegisterAPI.Models;
using Ofqual.Common.RegisterAPI.Models.DB;
using Ofqual.Common.RegisterAPI.UseCase.Organisations;

namespace Ofqual.Common.RegisterAPI.Tests.UseCase
{
    [TestFixture]
    public class GetOrganisationsUseCaseTests
    {
        private Mock<IRegisterDb> _mockDB;
        private GetOrganisationsListUseCase _classUnderTest;
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _mockDB = new Mock<IRegisterDb>();
            _classUnderTest = new GetOrganisationsListUseCase(new NullLoggerFactory(), _mockDB.Object);
            _fixture = new Fixture();
        }

        [Test]
        [Ignore("WIP")]
        public async Task ReturnsListOfOrganisations()
        {
            var stubbedList = _fixture.Create<List<Organisation>>();
            //_mockDB.Setup(r => r.GetOrganisations());
            //var result = await _classUnderTest.GetOrganisations(It.IsAny<string>());

            //Assert.That(result, Is.Not.Null);
            //Assert.That(result, Has.Count.EqualTo(stubbedList.Count));
            //Assert.That(result[0].ContactEmail, Is.EqualTo(stubbedList[0].ContactEmail));
        }

        /*[Test]
        public async Task ReturnsListOfOrganisationsFromTheRepositoryWhenThereIsACacheMiss()
        {
            var stubbedList = _fixture.Create<List<OrganisationPrivate>>();
            List<OrganisationPrivate>? stub = null;
            _mockRedisCache.Setup(r => r.GetCacheAsync<Organisation>(It.IsAny<string>())).ReturnsAsync(stub);
            _mockRegisterRepository.Setup(r => r.GetOrganisations()).ReturnsAsync(stubbedList);
            var result = await _classUnderTest.GetOrganisations(It.IsAny<string>());
            Assert.IsNotNull(result);
            Assert.That(result, Has.Count.EqualTo(stubbedList.Count));
            Assert.That(result[0].ContactEmail, Is.EqualTo(stubbedList[0].ContactEmail));
        }*/
    }
}
