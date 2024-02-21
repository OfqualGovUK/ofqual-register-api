using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using QualificationsRegister.Functions;
using QualificationsRegister.Tests.Mocks;
using QualificationsRegister.UseCase.Interfaces;
namespace QualificationsRegister.Tests.Functions
{
    [TestFixture]
    public class Function1FunctionTests
    {
        private Mock<FunctionContext> _functionContext;
        private Mock<IGetOrganisationsSearchUseCase> _searchUseCaseMock;
        private Mock<IGetOrganisationByNumberUseCase> _byNumberUseCaseMock;

        

        [SetUp]
        public void Setup()
        {
            _functionContext = new Mock<FunctionContext>();
            _searchUseCaseMock = new Mock<IGetOrganisationsSearchUseCase>();
            _byNumberUseCaseMock = new Mock<IGetOrganisationByNumberUseCase>();
        }
        [Test]
        public async Task FunctionReturnsOkResponse()
        {
            var httpFunc = new Function1(new NullLoggerFactory(), _searchUseCaseMock.Object, _byNumberUseCaseMock.Object);
            MockHttpRequestData requestData = new MockHttpRequestData(_functionContext.Object);
            var res = await httpFunc.Run(requestData);
            Console.WriteLine(res.StatusCode);
            Assert.That(res.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
        }
    }
}
