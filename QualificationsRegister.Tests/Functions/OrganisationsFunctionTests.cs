using AutoFixture;
using FluentAssertions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Ofqual.Common.RegisterAPI.Functions.Public;
using Ofqual.Common.RegisterAPI.Models;
using Ofqual.Common.RegisterAPI.Models.Exceptions;
using Ofqual.Common.RegisterAPI.Tests.Mocks;
using Ofqual.Common.RegisterAPI.UseCase.Interfaces;
using System.Net;
using System.Text.Json;

namespace Ofqual.Common.RegisterAPI.Tests.Functions
{
    [TestFixture]
    public class OrganisationFunctionTests
    {
        private Mock<FunctionContext> _functionContext;
        private Mock<IGetOrganisationsListUseCase> _searchUseCaseMock;
        private Mock<IGetOrganisationByNumberUseCase> _getOrganisationBybyNumberUseCaseMock;
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _functionContext = new Mock<FunctionContext>();
            _searchUseCaseMock = new Mock<IGetOrganisationsListUseCase>();
            _getOrganisationBybyNumberUseCaseMock = new Mock<IGetOrganisationByNumberUseCase>();
            _fixture = new Fixture();
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

            _getOrganisationBybyNumberUseCaseMock.Setup(m => m.GetOrganisationByNumber(It.IsAny<string>())).Returns(stub);
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
            _getOrganisationBybyNumberUseCaseMock.Setup(m => m.GetOrganisationByNumber(It.IsAny<string>()))
                .Throws(new BadRequestException("Please provide a valid organisation number"));
            var httpFunc = new OrganisationsPublic(new NullLoggerFactory(), _searchUseCaseMock.Object,
                _getOrganisationBybyNumberUseCaseMock.Object);
            MockHttpRequestData requestData = new MockHttpRequestData(_functionContext.Object);
            var res = await httpFunc.GetOrganisation(requestData, "error");

            res.StatusCode.Equals(HttpStatusCode.BadRequest);
            res.Should().NotBeNull();
        }
        [Test]
        public async Task GetOrganisationsListPublicResturnsOkResponse()
        {
            var stubbedList = _fixture.Create<List<Organisation>>();
            var response = new ListResponse<Organisation>
            {
                Results = stubbedList,

            };

            _searchUseCaseMock.Setup(m => m.ListOrganisations(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns(response);
            var httpFunc = new OrganisationsPublic(new NullLoggerFactory(), _searchUseCaseMock.Object,
                _getOrganisationBybyNumberUseCaseMock.Object);
            MockHttpRequestData requestData = new MockHttpRequestData(_functionContext.Object);
            var res = await httpFunc.GetOrganisationsList(requestData, "edexcel", 10);

            ListResponse<Organisation> body;
            using (var reader = new StreamReader(res.Body))
            {
                res.Body.Seek(0, SeekOrigin.Begin);
                body = JsonSerializer.Deserialize<ListResponse<Organisation>>(reader.ReadToEnd() ?? "", Utilities.JsonSerializerOptions)!;
            }

            res.StatusCode.Equals(HttpStatusCode.OK);
            res.Should().NotBeNull();
            body.Results.Should().HaveCount(response.Results.Count);
        }

        [Test]
        public async Task GetOrganisationsListPublicCatchesBadRequestException()
        {
            _searchUseCaseMock.Setup(m => m.ListOrganisations(It.IsAny<string>(), 1, 16))
                .Throws(new BadRequestException("Please use a limit size between 1 to 15 inclusive " +
                    "and page size greater than 0"));
            var httpFunc = new OrganisationsPublic(new NullLoggerFactory(), _searchUseCaseMock.Object,
                _getOrganisationBybyNumberUseCaseMock.Object);
            MockHttpRequestData requestData = new MockHttpRequestData(_functionContext.Object);
            var res = await httpFunc.GetOrganisationsList(requestData, "error", 10);

            res.StatusCode.Equals(HttpStatusCode.BadRequest);
            res.Should().NotBeNull();
        }
    }
}
