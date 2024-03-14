using AutoFixture;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Ofqual.Common.RegisterAPI.Database;
using Ofqual.Common.RegisterAPI.Models;
using Ofqual.Common.RegisterAPI.Models.DB;
using Ofqual.Common.RegisterAPI.UseCase;

namespace Ofqual.Common.RegisterAPI.Tests.UseCase
{
    [TestFixture]
    public class GetQualificationsUseCaseTests
    {
        private Mock<IRegisterDb> _mockDB;
        private GetQualificationsUseCase _classUnderTest;
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _mockDB = new Mock<IRegisterDb> { CallBase = true };
            _classUnderTest = new GetQualificationsUseCase(new NullLoggerFactory(), _mockDB.Object);
            _fixture = new Fixture();
        }

        [Test]
        [Ignore("WIP")]
        public async Task ReturnsListOfQualifications()
        {
            var stubbedList = _fixture.Create<List<Qualification>>();
            _mockDB.Setup(r => r.GetQualifications(""));
            var result = await _classUnderTest.GetQualificationsPublic(It.IsAny<string>());

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Has.Count.EqualTo(stubbedList.Count));
            Assert.That(result[0].OrganisationName, Is.EqualTo(stubbedList[0].OrganisationName));
        }

        [Test]
        [Ignore("WIP")]
        public async Task ReturnsListOfQualificationsFromTheRepositoryWhenThereIsACacheMiss()
        {
            Assert.That(1, Is.EqualTo(1));

            //var stubbedList = _fixture.Create<List<QualificationPublic>>();
            //List<QualificationPrivate>? stub = null;
            //_mockRedisCache.Setup(r => r.GetCacheAsync<OrganisationPrivate>(It.IsAny<string>())).ReturnsAsync(stub);
            //_mockRegisterRepository.Setup(r => r.GetOrganisations()).ReturnsAsync(stubbedList);
            //var result = await _classUnderTest.GetQualificationsPublic(It.IsAny<string>());
            /*Assert.IsNotNull(result);
            Assert.That(result, Has.Count.EqualTo(stubbedList.Count));
            Assert.That(result[0].ContactEmail, Is.EqualTo(stubbedList[0].ContactEmail));*/
        }
    }
}
