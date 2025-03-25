using AutoFixture;
using FluentAssertions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Ofqual.Common.RegisterAPI.Functions.Private;
using Ofqual.Common.RegisterAPI.Functions.Public;
using Ofqual.Common.RegisterAPI.Mappers;
using Ofqual.Common.RegisterAPI.Models;
using Ofqual.Common.RegisterAPI.Models.Exceptions;
using Ofqual.Common.RegisterAPI.Tests.Mocks;
using Ofqual.Common.RegisterAPI.UseCase.Interfaces;
using System.Collections.Specialized;
using System.Diagnostics.Eventing.Reader;
using System.Net;

namespace Ofqual.Common.RegisterAPI.Tests.Functions
{
    [TestFixture]
    public class QualificationsPrivateFunctionTests
    {
        private Mock<FunctionContext> _functionContext;
        private Mock<IGetQualificationsListUseCase> _listUseCaseMock;
        private Mock<IGetQualificationByNumberUseCase> _byNumberUseCaseMock;
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _functionContext = new Mock<FunctionContext>();
            _listUseCaseMock = new Mock<IGetQualificationsListUseCase>();
            _byNumberUseCaseMock = new Mock<IGetQualificationByNumberUseCase>();
            _fixture = new Fixture();
        }

        [Test]
        public async Task GetQualificationByNumberReturnsOkResponse()
        {
            var stub = _fixture.Create<Qualification>();
            var httpFunc = new QualificationsPrivate(new NullLoggerFactory(), _listUseCaseMock.Object, _byNumberUseCaseMock.Object);
            MockHttpRequestData requestData = new MockHttpRequestData(_functionContext.Object);
            _byNumberUseCaseMock.Setup(m => m.GetQualificationByNumber(It.IsAny<string>(), null, null)).Returns(stub);

            var res = await httpFunc.GetQualification(requestData, _fixture.Create<string>(), null, null);
            Console.WriteLine(res.StatusCode);
            Assert.That(res.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            res.Should().NotBeNull();
        }

        [Test]
        public async Task GetQualificationByNumberReturnsBadRequest()
        {
            var stub = _fixture.Create<Qualification>();
            var httpFunc = new QualificationsPrivate(new NullLoggerFactory(), _listUseCaseMock.Object, _byNumberUseCaseMock.Object);

            MockHttpRequestData requestData = new MockHttpRequestData(_functionContext.Object);

            _byNumberUseCaseMock.Setup(m => m.GetQualificationByNumber(It.IsAny<string>(), null, null)).Throws(new BadRequestException("Invalid Qualification number format. Permitted format: 500/1522/9 or 50015229"));

            var res = await httpFunc.GetQualification(requestData, "", null, null);
            Console.WriteLine(res.StatusCode);
            Assert.That(res.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            res.Should().NotBeNull();
        }

        [Test]
        public async Task GetQualificationByTitleReturnsOkResponse()
        {
            var stubbedList = _fixture.Create<ListResponse<Qualification>>();

            _listUseCaseMock.Setup(m => m.ListQualifications(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<QualificationFilter>(), It.IsAny<string>())).Returns(stubbedList);
            var httpFunc = new QualificationsPrivate(new NullLoggerFactory(), _listUseCaseMock.Object,
                _byNumberUseCaseMock.Object);
            MockHttpRequestData requestData = new MockHttpRequestData(_functionContext.Object);
            var res = await httpFunc.ListQualifications(requestData, 1, 15, "edexcel");

            Assert.That(res.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            res.Should().NotBeNull();
        }

        [Test]
        public async Task GetQualificationByTitleReturnsBadRequest()
        {
            var stubbedList = _fixture.Create<ListResponse<Qualification>>();

            _listUseCaseMock.Setup(m => m.ListQualifications(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<QualificationFilter>(), It.IsAny<string>())).Throws(new BadRequestException(""));
            var httpFunc = new QualificationsPrivate(new NullLoggerFactory(), _listUseCaseMock.Object,
                _byNumberUseCaseMock.Object);

            MockHttpRequestData requestData = new MockHttpRequestData(_functionContext.Object);

            var res = await httpFunc.ListQualifications(requestData, 1, 15, "edexcel");

            Assert.That(res.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            res.Should().NotBeNull();
        }

        [DatapointSource]
        public object[] IntentionToSeekFundingInEnglandData = {
            new { request = "intentionToSeekFundingInEngland=true",  expected = true  },
            new { request = "intentionToSeekFundingInEngland=1",     expected = true  },
            new { request = "intentionToSeekFundingInEngland=false", expected = false },
            new { request = "intentionToSeekFundingInEngland=0",     expected = false },
            new { request = "intentionToSeekFundingInEngland=asdf",  expected = (bool?) null },
            new { request = "intentionToSeekFundingInEngland=null",  expected = (bool?) null },
            new { request = "intentionToSeekFundingInEngland=2",     expected = (bool?) null },
            new { request = "dummyParameter=true",                   expected = (bool?) null },
            new { request = "dummyParameter=false",                  expected = (bool?) null },
            new { request = "",                                      expected = (bool?) null }
        };  

        [Theory]
        public async Task FilterQualificationsByIntentionToSeekFundingInEngland(string request, bool? expected)
        {
            //Arrange
            var stubbedQualList = new Qualification[]
            {
                new()
                {
                    QualificationNumber = "1",
                    Title = "funded qual 1",
                    OrganisationName = "1quals",
                    OrganisationAcronym = "oq",
                    OrganisationRecognitionNumber = "1",
                    IntentionToSeekFundingInEngland = true
                },
                new()                                                                                                                                                 
                {
                    QualificationNumber = "2",
                    Title =   "funded qual 2",
                    OrganisationName = "1quals",
                    OrganisationAcronym = "oq",
                    OrganisationRecognitionNumber = "1",
                    IntentionToSeekFundingInEngland = true
                },
                new()                                                                                                                                                 
                {
                    QualificationNumber = "3",
                    Title = "unfunded qual 1",
                    OrganisationName = "1quals",
                    OrganisationAcronym = "oq",
                    OrganisationRecognitionNumber = "1",
                    IntentionToSeekFundingInEngland = false
                },
                new()                                                                                                                                                 
                {
                    QualificationNumber = "4",
                    Title = "unfunded qual 2",
                    OrganisationName = "1quals",
                    OrganisationAcronym = "oq",
                    OrganisationRecognitionNumber = "1",
                    IntentionToSeekFundingInEngland = false
                }
            };


            var qParams = request.Split('"').Select(x => x.Trim()).ToArray();

            //Act
            QualificationFilterNvcMapper.GetQualificationFilterQuery(new NameValueCollection { { qParams[0], qParams[1] } });




            //Assert
            //TODO:Complete Test
            Assert.That(stubbedQualList, Is.Not.Empty);

        }

    }
}
