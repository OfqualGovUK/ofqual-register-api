using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Ofqual.Common.RegisterAPI.Functions.Public;
using Ofqual.Common.RegisterAPI.Tests.Mocks;
using Ofqual.Common.RegisterAPI.UseCase.Interfaces;

namespace Ofqual.Common.RegisterAPI.Tests.Functions
{
    [TestFixture]
    public class Function1FunctionTests
    {
        private Mock<FunctionContext> _functionContext;
        private Mock<IGetOrganisationsUseCase> _searchUseCaseMock;
        private Mock<IGetOrganisationByReferenceUseCase> _byNumberUseCaseMock;

        

        [SetUp]
        public void Setup()
        {
            _functionContext = new Mock<FunctionContext>();
            _searchUseCaseMock = new Mock<IGetOrganisationsUseCase>();
            _byNumberUseCaseMock = new Mock<IGetOrganisationByReferenceUseCase>();
        }
        [Test]
        public async Task FunctionReturnsOkResponse()
        {
            var httpFunc = new OrganisationsPublic(new NullLoggerFactory(), _searchUseCaseMock.Object, _byNumberUseCaseMock.Object);
            MockHttpRequestData requestData = new MockHttpRequestData(_functionContext.Object);
            var res = await httpFunc.GetOrganisation(requestData, "");
            Console.WriteLine(res.StatusCode);
            Assert.That(res.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
        }
    }
}
