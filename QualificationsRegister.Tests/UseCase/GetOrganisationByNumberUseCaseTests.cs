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
    public class GetOrganisationByNumberUseCaseTests
    {
        private Mock<IRegisterDb> _mockDB;
        private GetOrganisationByNumberUseCase _classUnderTest;
        private Fixture _fixture;
        private readonly string _apiUrl = "http://apiUrl";

        [SetUp]
        public void Setup()
        {
            _mockDB = new Mock<IRegisterDb>();
            _classUnderTest = new GetOrganisationByNumberUseCase(new NullLoggerFactory(), _mockDB.Object,
                new ApiOptions { ApiUrl= _apiUrl });
            _fixture = new Fixture();
        }


        [Test]
        public void GetsAnOrganisationsUsingRecognitionNumber()
        {
            var stub = _fixture.Create<Organisation>();
            _mockDB.Setup(x => x.GetOrganisationByNumber("2323","RN2323")).Returns(stub);
            var result = _classUnderTest.GetOrganisationByNumber("2323");

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(stub);
            result.CanonicalUrl.Should().Be($"{_apiUrl}/api/organisations/{result.RecognitionNumber}");
        }

        [Test]
        public void GetOrganisationsByNumberWithEmptyParameterReturnsBadRequest()
        {
            Func<Organisation> testDelegate = () => _classUnderTest.GetOrganisationByNumber("")!;
            testDelegate.Should().Throw<BadRequestException>().WithMessage("Organisation number is null or empty");
        }


        [Test]
        [TestCase("number=rn23")]
        [TestCase("number")]
        [TestCase("23number")]
        public void GetOrganisationsByNumberWithInvalidParameterReturnsBadRequest(string number)
        {
            Func<Organisation> testDelegate = () => _classUnderTest.GetOrganisationByNumber(number)!;
            testDelegate.Should().Throw<BadRequestException>().WithMessage("Please provide a valid organisation number");
        }

    }
}
