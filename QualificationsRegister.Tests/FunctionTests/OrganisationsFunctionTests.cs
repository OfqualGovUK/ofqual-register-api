using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Ofqual.Common.RegisterAPI.Functions.Public;
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

        [SetUp]
        public void Setup()
        {
            _functionContext = new Mock<FunctionContext>();
            _searchUseCaseMock = new Mock<IGetOrganisationsUseCase>();
            _getOrganisationBybyNumberUseCaseMock = new Mock<IGetOrganisationByNumberUseCase>();
        }
        [Test]
        public async Task GetOrganisationPublicReturnsBadRequest()
        {
            var httpFunc = new OrganisationsPublic(new NullLoggerFactory(), _searchUseCaseMock.Object, _getOrganisationBybyNumberUseCaseMock.Object);
            MockHttpRequestData requestData = new MockHttpRequestData(_functionContext.Object);
            var res = await httpFunc.GetOrganisation(requestData, "");
            Console.WriteLine(res.StatusCode);
            Assert.That(res.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task GetOrganisationsListPublicResturnsOkResponse()
        {
            var httpFunc = new OrganisationsPublic(new NullLoggerFactory(), _searchUseCaseMock.Object, _getOrganisationBybyNumberUseCaseMock.Object);
            MockHttpRequestData requestData = new MockHttpRequestData(_functionContext.Object);
            var res = await httpFunc.GetOrganisationsList(requestData, "");
            Console.WriteLine(res.StatusCode);
            Assert.That(res.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
        }
    }
}
