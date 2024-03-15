using AutoFixture;
using FluentAssertions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
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
        private Mock<IGetQualificationsListUseCase> _searchUseCaseMock;
        private Mock<IGetQualificationByNumberUseCase> _byNumberUseCaseMock;
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _functionContext = new Mock<FunctionContext>();
            _searchUseCaseMock = new Mock<IGetQualificationsListUseCase>();
            _byNumberUseCaseMock = new Mock<IGetQualificationByNumberUseCase>();
            _fixture = new Fixture();
        }
        [Test]
        public async Task GetQualificationByNumberPublicReturnsOkResponse()
        {
            var stub = _fixture.Create<QualificationPublic>();
            var httpFunc = new QualificationsPublic(new NullLoggerFactory(), _searchUseCaseMock.Object, _byNumberUseCaseMock.Object);
            MockHttpRequestData requestData = new MockHttpRequestData(_functionContext.Object);
            _byNumberUseCaseMock.Setup(m => m.GetQualificationPublicByNumber(It.IsAny<string>())).Returns(stub);

            var res = await httpFunc.GetQualification(requestData, _fixture.Create<string>());
            Console.WriteLine(res.StatusCode);
            Assert.That(res.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
            res.Should().NotBeNull();
        }

        [Test]
        public async Task GetQualificationsByNumberPublicReturnsBadRequest()
        {
            var stub = _fixture.Create<QualificationPublic>();
            var httpFunc = new QualificationsPublic(new NullLoggerFactory(), _searchUseCaseMock.Object, _byNumberUseCaseMock.Object);
            MockHttpRequestData requestData = new MockHttpRequestData(_functionContext.Object);
            _byNumberUseCaseMock.Setup(m => m.GetQualificationPublicByNumber(It.IsAny<string>())).Returns(stub);

            var res = await httpFunc.GetQualification(requestData, "");
            Console.WriteLine(res.StatusCode);
            Assert.That(res.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.BadRequest));
            res.Should().NotBeNull();   
        }

        [Test]
        public async Task GetQualificationsListPublicResturnsOkResponse()
        {
            var stubbedList = _fixture.Create<List<QualificationPublic>>();

            var httpFunc = new QualificationsPublic(new NullLoggerFactory(), _searchUseCaseMock.Object, _byNumberUseCaseMock.Object);
            MockHttpRequestData requestData = new MockHttpRequestData(_functionContext.Object);
            _searchUseCaseMock.Setup(m => m.ListQualificationsPublic(It.IsAny<string>())).Returns(stubbedList);

            var res = await httpFunc.GetQualification(requestData, It.IsAny<string>());

            Assert.That(res.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
            res.Should().NotBeNull();
        }

        [Test]
        public async Task GetQualificationsListPublicThrowsInternalServerError()
        {
            var stubbedListPublic = _fixture.Create<List<QualificationPublic>>();
            var httpFunc = new QualificationsPublic(new NullLoggerFactory(), _searchUseCaseMock.Object, _byNumberUseCaseMock.Object);
            MockHttpRequestData requestData = new MockHttpRequestData(_functionContext.Object);
            _searchUseCaseMock.Setup(m => m.ListQualificationsPublic(It.IsAny<string>())).Throws<Exception>();
            var res = await httpFunc.GetQualification(requestData, "");

            Assert.That(res.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.InternalServerError));
            res.Should().NotBeNull();
        }
    }
}
