using AutoFixture;
using FluentAssertions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Ofqual.Common.RegisterAPI.Functions.Private;
using Ofqual.Common.RegisterAPI.Functions.Public;
using Ofqual.Common.RegisterAPI.Models;
using Ofqual.Common.RegisterAPI.Models.DB;
using Ofqual.Common.RegisterAPI.Tests.Mocks;
using Ofqual.Common.RegisterAPI.UseCase.Interfaces;
using System.Collections.Generic;

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
            _byNumberUseCaseMock.Setup(m => m.GetQualificationPublicByNumber(It.IsAny<string>())).Returns(stub);

            var res = await httpFunc.GetQualification(requestData, _fixture.Create<string>());
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
            _byNumberUseCaseMock.Setup(m => m.GetQualificationPublicByNumber(It.IsAny<string>())).Returns(stub);

            var res = await httpFunc.GetQualification(requestData, "");
            Console.WriteLine(res.StatusCode);
            Assert.That(res.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.BadRequest));
            res.Should().NotBeNull();
        }

        [Test]
        public async Task GetQualificationByTitlePublicReturnsOkResponse()
        {
            var stubbedList = _fixture.Create<ListResponse<QualificationPublic>>();

            _listUseCaseMock.Setup(m => m.ListQualificationsPublic(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).Returns(stubbedList);
            var httpFunc = new QualificationsPublic(new NullLoggerFactory(), _listUseCaseMock.Object,
                _byNumberUseCaseMock.Object);
            MockHttpRequestData requestData = new MockHttpRequestData(_functionContext.Object);
            var res = await httpFunc.ListQualifications(requestData, 1, 15, "edexcel");

            Assert.That(res.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
            res.Should().NotBeNull();
        }
    }
}
