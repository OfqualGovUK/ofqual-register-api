using AutoFixture;
using FluentAssertions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Ofqual.Common.RegisterAPI.Functions.Public;
using Ofqual.Common.RegisterAPI.Models.DB;
using Ofqual.Common.RegisterAPI.Tests.Mocks;
using Ofqual.Common.RegisterAPI.UseCase.Interfaces;

namespace Ofqual.Common.RegisterAPI.Tests.Functions
{
    [TestFixture]
    public class OrganisationFunctionTests
    {
        private Mock<FunctionContext> _functionContext;
        private Mock<IGetOrganisationsUseCase> _searchUseCaseMock;
        private Mock<IGetOrganisationByNumberUseCase> _getOrganisationBybyNumberUseCaseMock;
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _functionContext = new Mock<FunctionContext>();
            _searchUseCaseMock = new Mock<IGetOrganisationsUseCase>();
            _getOrganisationBybyNumberUseCaseMock = new Mock<IGetOrganisationByNumberUseCase>();
            _fixture = new Fixture();
        }
        [Test]
        public async Task GetOrganisationPublicReturnsBadRequest()
        {
            var httpFunc = new OrganisationsPublic(new NullLoggerFactory(), _searchUseCaseMock.Object,
                _getOrganisationBybyNumberUseCaseMock.Object);
            MockHttpRequestData requestData = new MockHttpRequestData(_functionContext.Object);
            var res = await httpFunc.GetOrganisation(requestData, "");
            Assert.That(res.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.BadRequest));
            res.Should().NotBeNull();
        }

        [Test]
        public async Task GetOrganisationPublicReturnsNotFoundResponse()
        {
            var httpFunc = new OrganisationsPublic(new NullLoggerFactory(), _searchUseCaseMock.Object,
                _getOrganisationBybyNumberUseCaseMock.Object);
            MockHttpRequestData requestData = new MockHttpRequestData(_functionContext.Object);
            var res = await httpFunc.GetOrganisation(requestData, "MIT");
            Assert.That(res.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.NotFound));
            res.Should().NotBeNull();
        }


        [Test]
        public async Task GetOrganisationPublicReturnsOkResponse()
        {
            var stub = _fixture.Create<Organisation>();

            _getOrganisationBybyNumberUseCaseMock.Setup(m => m.GetOrganisationByNumber(It.IsAny<string>())).ReturnsAsync(stub);
            var httpFunc = new OrganisationsPublic(new NullLoggerFactory(), _searchUseCaseMock.Object,
                _getOrganisationBybyNumberUseCaseMock.Object);
            MockHttpRequestData requestData = new MockHttpRequestData(_functionContext.Object);
            var res = await httpFunc.GetOrganisation(requestData, "edexcel");
            Assert.That(res.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
            res.Should().NotBeNull();
        }

        [Test]
        public async Task GetOrganisationPublicThrowsException()
        {
            var stub = _fixture.Create<Organisation>();

            _getOrganisationBybyNumberUseCaseMock.Setup(m => m.GetOrganisationByNumber(It.IsAny<string>())).Throws<Exception>();
            var httpFunc = new OrganisationsPublic(new NullLoggerFactory(), _searchUseCaseMock.Object,
                _getOrganisationBybyNumberUseCaseMock.Object);
            MockHttpRequestData requestData = new MockHttpRequestData(_functionContext.Object);
            var res = await httpFunc.GetOrganisation(requestData, "error");

            Assert.That(res.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.InternalServerError));
            
            res.Should().NotBeNull();
        }

        [Test]
        public async Task GetOrganisationsListPublicResturnsOkResponse()
        {
            var stubbedList = _fixture.Create<List<Organisation>>();

            _searchUseCaseMock.Setup(m => m.GetOrganisations(It.IsAny<string>())).ReturnsAsync(stubbedList);
            var httpFunc = new OrganisationsPublic(new NullLoggerFactory(), _searchUseCaseMock.Object,
                _getOrganisationBybyNumberUseCaseMock.Object);
            MockHttpRequestData requestData = new MockHttpRequestData(_functionContext.Object);
            var res = await httpFunc.GetOrganisationsList(requestData, "edexcel");

            Assert.That(res.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
            res.Should().NotBeNull();
        }

        [Test]
        public async Task GetOrganisationsListPublicThrowsInternalServerError()
        {
            _searchUseCaseMock.Setup(m => m.GetOrganisations(It.IsAny<string>())).Throws<Exception>();
            var httpFunc = new OrganisationsPublic(new NullLoggerFactory(), _searchUseCaseMock.Object,
                _getOrganisationBybyNumberUseCaseMock.Object);
            MockHttpRequestData requestData = new MockHttpRequestData(_functionContext.Object);
            var res = await httpFunc.GetOrganisationsList(requestData, "error");

            Assert.That(res.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.InternalServerError));
            res.Should().NotBeNull();
        }
    }
}
