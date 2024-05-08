using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Ofqual.Common.RegisterAPI.Database;
using Ofqual.Common.RegisterAPI.Models;
using Ofqual.Common.RegisterAPI.Models.DB;
using Ofqual.Common.RegisterAPI.Models.Exceptions;
using Ofqual.Common.RegisterAPI.UseCase.Qualifications;

namespace Ofqual.Common.RegisterAPI.Tests.UseCase
{
    [TestFixture]
    public class ListQualificationsUseCaseTests
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

        #region Qualifications Private

        [Test]
        public void ReturnsListOfPrivateQualifications()
        {
            var mockList = _fixture.Create<List<DbQualification>>();

            var listResp = new DbListResponse<DbQualification>()
            {
                Count = mockList.Count,
                Results = mockList
            };

            _mockDB.Setup(x => x.GetQualificationsList(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<QualificationFilter>(), It.IsAny<string>())).Returns(listResp);
            var result = _classUnderTest.ListQualifications(1, 1, null, null);

            result.Should().NotBeNull();
            result?.Results.Should().HaveCount(mockList.Count);
        }


        [Test]
        public void ListOfQualificationsWithInvalidPagingParametersReturnsBadRequest()
        {
            var stubbedList = _fixture.Create<DbListResponse<DbQualification>>();
            _mockDB.Setup(x => x.GetQualificationsList(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<QualificationFilter>(), It.IsAny<string>())).Returns(stubbedList);

            Func<ListResponse<Qualification>> testDelegate = () => _classUnderTest.ListQualifications(1, 101, It.IsAny<QualificationFilter>(), It.IsAny<string>())!;
            testDelegate.Should().Throw<BadRequestException>().WithMessage($"Invalid parameter values. Page should be > 0 and Limit should be > 0 and <= 100");
        }
        #endregion

        #region Qualification Public


        [Test]
        public void ReturnsListOfPublicQualifications()
        {
            var mockList = _fixture.Create<List<DbQualificationPublic>>();

            var listResp = new DbListResponse<DbQualificationPublic>()
            {
                Count = mockList.Count,
                Results = mockList
            };

            _mockDB.Setup(x => x.GetQualificationsPublicList(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<QualificationFilter>(), It.IsAny<string>())).Returns(listResp);
            var result = _classUnderTest.ListQualificationsPublic(1, 1, null, null);

            result.Should().NotBeNull();
            result?.Results.Should().HaveCount(mockList.Count);
        }


        [Test]
        public void ListOfQualificationsPublicWithInvalidPagingParametersReturnsBadRequest()
        {
            var stubbedList = _fixture.Create<DbListResponse<DbQualificationPublic>>();
            _mockDB.Setup(x => x.GetQualificationsPublicList(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<QualificationFilter>(), It.IsAny<string>())).Returns(stubbedList);

            Func<ListResponse<QualificationPublic>> testDelegate = () => _classUnderTest.ListQualificationsPublic(1, 101, It.IsAny<QualificationFilter>(), It.IsAny<string>())!;
            testDelegate.Should().Throw<BadRequestException>().WithMessage($"Invalid parameter values. Page should be > 0 and Limit should be > 0 and <= 100");
        }

        #endregion
    }
}
