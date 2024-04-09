using AutoFixture;
using FluentAssertions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Ofqual.Common.RegisterAPI.Database;
using Ofqual.Common.RegisterAPI.Models;
using Ofqual.Common.RegisterAPI.Models.DB;
using Ofqual.Common.RegisterAPI.Models.Exceptions;
using Ofqual.Common.RegisterAPI.UseCase.Organisations;
using System.Diagnostics;

namespace Ofqual.Common.RegisterAPI.Tests.UseCase
{
    [TestFixture]
    public class ListOrganisationsUseCaseTests
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
        public void ReturnsListOfOrganisationsWithValidPagingParameters()
        {
            var stubbedList = _fixture.Create<List<DbOrganisation>>();

            var listResp = new DbListResponse<DbOrganisation>()
            {
                Results = stubbedList,
                Count = 1
            };

            _mockDB.Setup(x => x.GetOrganisationsList(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).Returns(listResp);
            var result = _classUnderTest.ListOrganisations(It.IsAny<string>(), 1, 1);

            result.Should().NotBeNull();
            result?.Results.Should().HaveCount(stubbedList.Count);
        }

        [Test]
        public void ListOfOrganisationsWithInvalidPagingParametersReturnsBadRequest()
        {
            var stubbedList = _fixture.Create<DbListResponse<DbOrganisation>>();
            _mockDB.Setup(x => x.GetOrganisationsList(15, 0, It.IsAny<string>())).Returns(stubbedList);

            Func<ListResponse<Organisation>> testDelegate = () => _classUnderTest.ListOrganisations(It.IsAny<string>(), 0, 17)!;
            testDelegate.Should().Throw<BadRequestException>().WithMessage("Invalid Paging params - Please use a limit greater than 0 and page size greater than 0");
        }
    }
}
