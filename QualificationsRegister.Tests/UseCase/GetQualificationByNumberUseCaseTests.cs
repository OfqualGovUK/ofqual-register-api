using AutoFixture;
using FluentAssertions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Ofqual.Common.RegisterAPI.Database;
using Ofqual.Common.RegisterAPI.Extensions;
using Ofqual.Common.RegisterAPI.Models;
using Ofqual.Common.RegisterAPI.Models.DB;
using Ofqual.Common.RegisterAPI.Models.Exceptions;
using Ofqual.Common.RegisterAPI.UseCase.Qualifications;
using System.Diagnostics;

namespace Ofqual.Common.RegisterAPI.Tests.UseCase
{
    [TestFixture]
    public class GetQualificationByNumberUseCaseTests
    {
        private Mock<IRegisterDb> _mockDB;
        private GetQualificationByNumberUseCase _classUnderTest;
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _mockDB = new Mock<IRegisterDb>();
            _classUnderTest = new GetQualificationByNumberUseCase(new NullLoggerFactory(), _mockDB.Object);
            _fixture = new Fixture();
        }

        #region QualificationPublic
        [Test]
        public void GetsAQualificationPublicUsingRecognitionNumberNoObliques()
        {
            var dbQual = _fixture.Create<DbQualificationPublic>();
            _mockDB.Setup(x => x.GetQualificationPublicByNumber("", "10015784")).Returns(dbQual);
            var domainQual = _classUnderTest.GetQualificationPublicByNumber("10015784", null, null);

            AssertQualificationPublicProperties(dbQual, domainQual);
        }

        [Test]
        public void GetsAQualificationPublicUsingRecognitionNumberObliques()
        {
            var dbQual = _fixture.Create<DbQualificationPublic>();
            _mockDB.Setup(x => x.GetQualificationPublicByNumber("100/1578/4", "")).Returns(dbQual);
            var domainQual = _classUnderTest.GetQualificationPublicByNumber("100", "1578", "4");

            AssertQualificationPublicProperties(dbQual, domainQual);
        }


        //throws null with bad qual number format (no obliques)
        [Test]
        public void GetQualificationPublicByNumberNoObliquesWithBadParameterReturnsBadRequest()
        {
            Func<QualificationPublic> testDelegate = () => _classUnderTest.GetQualificationPublicByNumber("5353", null, null)!;
            testDelegate.Should().Throw<BadRequestException>().WithMessage("Invalid Qualification number format. Permitted format: 500/1522/9 or 50015229");
        }
        //throws null with bad qual number format (obliques)
        [Test]
        public void GetQualificationPublicByNumberObliquesWithBadParameterReturnsBadRequest()
        {
            Func<QualificationPublic> testDelegate = () => _classUnderTest.GetQualificationPublicByNumber("5353", "2", null)!;
            testDelegate.Should().Throw<BadRequestException>().WithMessage("Invalid Qualification number format. Permitted format: 500/1522/9 or 50015229");
        }

        [Test]
        public void GetQualificationPublicByNumberWithEmptyParameterReturnsBadRequest()
        {
            Func<QualificationPublic> testDelegate = () => _classUnderTest.GetQualificationPublicByNumber("", null, null)!;
            testDelegate.Should().Throw<BadRequestException>().WithMessage("Invalid Qualification number format. Permitted format: 500/1522/9 or 50015229");
        }

        private static void AssertQualificationPublicProperties(DbQualificationPublic dbQual, QualificationPublic? domainQual)
        {
            domainQual.Should().NotBeNull();
            domainQual.Should().BeEquivalentTo(dbQual,
                            options => options
                                .Excluding(x => x.Id)
                                .Excluding(x => x.RegulationStartDate)
                                .Excluding(x => x.OperationalStartDate)
                                .Excluding(x => x.OperationalEndDate)
                                .Excluding(x => x.CertificationEndDate)
                                .Excluding(x => x.ReviewDate)
                                .Excluding(x => x.LastUpdatedDate)
                                .Excluding(x => x.AssessmentMethods)
                                );

            domainQual?.AssessmentMethods?.Should().BeEquivalentTo(dbQual.AssessmentMethods?.GetSubStrings());

            domainQual?.RegulationStartDate.Should().Be(dbQual.RegulationStartDate.ToFormattedUniversalTime());
            domainQual?.OperationalStartDate.Should().Be(dbQual.OperationalStartDate.ToFormattedUniversalTime());
            domainQual?.OperationalEndDate.Should().Be(dbQual.OperationalEndDate?.ToFormattedUniversalTime());
            domainQual?.CertificationEndDate.Should().Be(dbQual.CertificationEndDate?.ToFormattedUniversalTime());
            domainQual?.ReviewDate.Should().Be(dbQual.ReviewDate?.ToFormattedUniversalTime());
            domainQual?.LastUpdatedDate.Should().Be(dbQual.LastUpdatedDate?.ToFormattedUniversalTime());
        }

        #endregion

        #region QualificationPrivate
        [Test]
        public void GetsAQualificationUsingRecognitionNumber()
        {
            var dbQual = _fixture.Create<DbQualification>();
            _mockDB.Setup(x => x.GetQualificationByNumber("100/1578/4", "")).Returns(dbQual);
            var domainQual = _classUnderTest.GetQualificationByNumber("100", "1578", "4");

            AssertQualificationProperties(dbQual, domainQual);
        }

        [Test]
        public void GetQualificationByNumberWithEmptyParameterReturnsBadRequest()
        {
            Func<Qualification> testDelegate = () => _classUnderTest.GetQualificationByNumber("", null, null)!;
            testDelegate.Should().Throw<BadRequestException>().WithMessage("Invalid Qualification number format. Permitted format: 500/1522/9 or 50015229");
        }


        private static void AssertQualificationProperties(DbQualification dbQual, Qualification? domainQual)
        {
            domainQual.Should().NotBeNull();
            domainQual.Should().BeEquivalentTo(dbQual,
                            options => options
                                .Excluding(x => x.Id)
                                .Excluding(x => x.RegulationStartDate)
                                .Excluding(x => x.OperationalStartDate)
                                .Excluding(x => x.OperationalEndDate)
                                .Excluding(x => x.CertificationEndDate)
                                .Excluding(x => x.ReviewDate)
                                .Excluding(x => x.EmbargoDate)
                                .Excluding(x => x.LastUpdatedDate)
                                .Excluding(x => x.AssessmentMethods)
                                .Excluding(x => x.UILastUpdatedDate)
                                .Excluding(x => x.SSA_Code)
                                );

            domainQual?.AssessmentMethods?.Should().BeEquivalentTo(dbQual.AssessmentMethods?.GetSubStrings());
            domainQual?.SSACode?.Should().BeEquivalentTo(dbQual.SSA_Code);

            domainQual?.RegulationStartDate.Should().Be(dbQual.RegulationStartDate.ToFormattedUniversalTime());
            domainQual?.OperationalStartDate.Should().Be(dbQual.OperationalStartDate.ToFormattedUniversalTime());
            domainQual?.OperationalEndDate.Should().Be(dbQual.OperationalEndDate?.ToFormattedUniversalTime());
            domainQual?.CertificationEndDate.Should().Be(dbQual.CertificationEndDate?.ToFormattedUniversalTime());
            domainQual?.ReviewDate.Should().Be(dbQual.ReviewDate?.ToFormattedUniversalTime());
            domainQual?.EmbargoDate.Should().Be(dbQual.EmbargoDate?.ToFormattedUniversalTime());
            domainQual?.UILastUpdatedDate.Should().Be(dbQual.UILastUpdatedDate.ToFormattedUniversalTime());
            domainQual?.LastUpdatedDate.Should().Be(dbQual.LastUpdatedDate?.ToFormattedUniversalTime());
        }

        #endregion
    }
}
