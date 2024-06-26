using AutoFixture;
using FluentAssertions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Ofqual.Common.RegisterAPI.Functions.Private;
using Ofqual.Common.RegisterAPI.Functions.Public;
using Ofqual.Common.RegisterAPI.Models;
using Ofqual.Common.RegisterAPI.Models.Exceptions;
using Ofqual.Common.RegisterAPI.Tests.Mocks;
using Ofqual.Common.RegisterAPI.UseCase.Interfaces;
using System.Net;

namespace Ofqual.Common.RegisterAPI.Tests.Functions
{
    [TestFixture]
    public class QualificationsPublicFunctionTests
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
        public async Task GetQualificationByNumberPublicReturnsOkResponse()
        {
            var stub = _fixture.Create<QualificationPublic>();
            var httpFunc = new QualificationsPublic(new NullLoggerFactory(), _listUseCaseMock.Object, _byNumberUseCaseMock.Object);
            MockHttpRequestData requestData = new MockHttpRequestData(_functionContext.Object);
            _byNumberUseCaseMock.Setup(m => m.GetQualificationPublicByNumber(It.IsAny<string>(), null, null)).Returns(stub);

            var res = await httpFunc.GetQualification(requestData, _fixture.Create<string>(), null, null);
            Console.WriteLine(res.StatusCode);
            Assert.That(res.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
            res.Should().NotBeNull();
        }

        [Test]
        public async Task GetQualificationByNumberPublicReturnsBadRequest()
        {
            var stub = _fixture.Create<QualificationPublic>();
            var httpFunc = new QualificationsPublic(new NullLoggerFactory(), _listUseCaseMock.Object, _byNumberUseCaseMock.Object);

            MockHttpRequestData requestData = new MockHttpRequestData(_functionContext.Object);

            _byNumberUseCaseMock.Setup(m => m.GetQualificationPublicByNumber(It.IsAny<string>(), null, null)).Throws(new BadRequestException("Invalid Qualification number format. Permitted format: 500/1522/9 or 50015229"));

            var res = await httpFunc.GetQualification(requestData, "", null, null);
            Console.WriteLine(res.StatusCode);
            Assert.That(res.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.BadRequest));
            res.Should().NotBeNull();
        }

        [Test]
        public async Task GetQualificationByTitlePublicReturnsOkResponse()
        {
            var stubbedList = _fixture.Create<ListResponse<QualificationPublic>>();

            _listUseCaseMock.Setup(m => m.ListQualificationsPublic(It.IsAny<int>(), It.IsAny<int>(), null, It.IsAny<string>())).Returns(stubbedList);
            var httpFunc = new QualificationsPublic(new NullLoggerFactory(), _listUseCaseMock.Object,
                _byNumberUseCaseMock.Object);
            MockHttpRequestData requestData = new MockHttpRequestData(_functionContext.Object);

            var res = await httpFunc.ListQualifications(requestData, 1, 15, "edexcel");

            Assert.That(res.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
            res.Should().NotBeNull();
        }


        [Test]
        public async Task GetQualificationByTitlePublicReturnsBadRequest()
        {
            var stubbedList = _fixture.Create<ListResponse<QualificationPublic>>();

            _listUseCaseMock.Setup(m => m.ListQualificationsPublic(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<QualificationFilter>(), It.IsAny<string>())).Throws(new BadRequestException(""));
            var httpFunc = new QualificationsPublic(new NullLoggerFactory(), _listUseCaseMock.Object,
                _byNumberUseCaseMock.Object);

            MockHttpRequestData requestData = new MockHttpRequestData(_functionContext.Object);

            var res = await httpFunc.ListQualifications(requestData, 1, 15, "edexcel");

            Assert.That(res.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            res.Should().NotBeNull();
        }
    }
}
