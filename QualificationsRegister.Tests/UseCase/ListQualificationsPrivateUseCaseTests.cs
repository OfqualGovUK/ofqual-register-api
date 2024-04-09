using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Ofqual.Common.RegisterAPI.Database;
using Ofqual.Common.RegisterAPI.Models;
using Ofqual.Common.RegisterAPI.Models.DB;
using Ofqual.Common.RegisterAPI.UseCase.Qualifications;

namespace Ofqual.Common.RegisterAPI.Tests.UseCase
{
    [TestFixture]
    public class ListQualificationsPrivateUseCaseTests
    {
        private Mock<IRegisterDb> _mockDB;
        private GetQualificationsListUseCase _classUnderTest;
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _mockDB = new Mock<IRegisterDb>();
            _classUnderTest = new GetQualificationsListUseCase(new NullLoggerFactory(), _mockDB.Object);
            _fixture = new Fixture();
        }

        [Test]
        public void ReturnsListOfPrivateQualifications()
        {
            var mockList = _fixture.Create<List<DbQualification>>();

            var listResp = new DbListResponse<DbQualification>()
            {
                Count = mockList.Count,
                Results = mockList
            };

            _mockDB.Setup(x => x.GetQualificationsByName(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<QualificationFilter>(), It.IsAny<string>())).Returns(listResp);
            var result = _classUnderTest.ListQualificationsPrivate(1, 1, null, null);

            result.Should().NotBeNull();
            result?.Results.Should().HaveCount(mockList.Count);
        }
    }
}
