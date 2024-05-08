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
    public class GetOrganisationByNumberUseCaseTests
    {
        private Mock<IRegisterDb> _mockDB;
        private GetOrganisationByNumberUseCase _classUnderTest;
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _mockDB = new Mock<IRegisterDb>();
            _classUnderTest = new GetOrganisationByNumberUseCase(new NullLoggerFactory(), _mockDB.Object);
            _fixture = new Fixture();
        }


        [Test]
        public void GetsAnOrganisationsUsingRecognitionNumber()
        {
            var dbOrg = _fixture.Create<DbOrganisation>();
            _mockDB.Setup(x => x.GetOrganisationByNumber("2323", "RN2323")).Returns(dbOrg);
            var domainOrg = _classUnderTest.GetOrganisationByNumber("2323");

            domainOrg.Should().NotBeNull();
            domainOrg.Should().BeEquivalentTo(dbOrg,
                            options => options
                                .Excluding(x => x.Id)
                                .Excluding(x => x.OfqualRecognisedOn)
                                .Excluding(x => x.OfqualRecognisedTo)
                                .Excluding(x => x.OfqualSurrenderedOn)
                                .Excluding(x => x.OfqualWithdrawnOn)
                                .Excluding(x => x.CceaRecognisedOn)
                                .Excluding(x => x.CceaRecognisedTo)
                                .Excluding(x => x.CceaSurrenderedOn)
                                .Excluding(x => x.CceaWithdrawnOn)
                                .Excluding(x => x.LastUpdatedDate)
                                );

            domainOrg?.OfqualRecognisedOn.Should().Be(dbOrg.OfqualRecognisedOn?.ToUniversalTime());
            domainOrg?.OfqualRecognisedTo.Should().Be(dbOrg.OfqualRecognisedTo?.ToUniversalTime());
            domainOrg?.OfqualSurrenderedOn.Should().Be(dbOrg.OfqualSurrenderedOn?.ToUniversalTime());
            domainOrg?.OfqualWithdrawnOn.Should().Be(dbOrg.OfqualWithdrawnOn?.ToUniversalTime());
            domainOrg?.CceaRecognisedTo.Should().Be(dbOrg.CceaRecognisedTo?.ToUniversalTime());
            domainOrg?.CceaRecognisedTo.Should().Be(dbOrg.CceaRecognisedTo?.ToUniversalTime());
            domainOrg?.CceaSurrenderedOn.Should().Be(dbOrg.CceaSurrenderedOn?.ToUniversalTime());
            domainOrg?.CceaWithdrawnOn.Should().Be(dbOrg.CceaWithdrawnOn?.ToUniversalTime());
            domainOrg?.LastUpdatedDate.Should().Be(dbOrg.LastUpdatedDate.ToUniversalTime());
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
