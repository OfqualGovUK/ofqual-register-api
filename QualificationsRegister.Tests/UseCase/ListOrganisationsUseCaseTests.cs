using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Ofqual.Common.RegisterAPI.Database;
using Ofqual.Common.RegisterAPI.Models;
using Ofqual.Common.RegisterAPI.Models.Exceptions;
using Ofqual.Common.RegisterAPI.UseCase.Organisations;

namespace Ofqual.Common.RegisterAPI.Tests.UseCase
{
    [TestFixture]
    public class ListOrganisationsUseCaseTests
    {
        private Mock<IRegisterDb> _mockDB;
        private GetOrganisationsListUseCase _classUnderTest;
        private Fixture _fixture;
        private readonly string _apiUrl = "http://apiUrl";

        [SetUp]
        public void Setup()
        {
            _mockDB = new Mock<IRegisterDb>();
            _classUnderTest = new GetOrganisationsListUseCase(new NullLoggerFactory(), _mockDB.Object,
                new ApiOptions { ApiUrl = _apiUrl });
            _fixture = new Fixture();
        }

        [Test]
        public void ReturnsListOfOrganisationsWithValidPagingParameters()
        {
            var stubbedList = _fixture.CreateMany<Organisation>().ToList();
            _mockDB.Setup(x => x.GetOrganisationsList(15, 0, It.IsAny<string>())).Returns((stubbedList, stubbedList.Count));
            var result = _classUnderTest.ListOrganisations(It.IsAny<string>(), 1, 15);

            result.Should().NotBeNull();
            result.Results.Should().HaveCount(stubbedList.Count);
            result.Results.Should().BeEquivalentTo(stubbedList);
            result.Results[0].CanonicalUrl.Should().Be($"{_apiUrl}/api/organisations/{result.Results[0].RecognitionNumber}");
        }

        [Test]
        public void ListOfOrganisationsWithInvalidPagingParametersReturnsBadRequest()
        {
            var stubbedList = _fixture.CreateMany<Organisation>().ToList();
            _mockDB.Setup(x => x.GetOrganisationsList(17, 0, It.IsAny<string>())).Returns((stubbedList, stubbedList.Count));

            Func<ListResponse<Organisation>> testDelegate = () => _classUnderTest.ListOrganisations(It.IsAny<string>(), 0, 17);
            testDelegate.Should().Throw<BadRequestException>().WithMessage("Please use a limit size between 1 to 15 inclusive " +
                    "and page size greater than 0");
        }
    }
}
