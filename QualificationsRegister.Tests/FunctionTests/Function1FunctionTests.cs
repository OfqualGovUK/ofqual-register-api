using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Moq;
using QualificationsRegister.Functions;
using QualificationsRegister.UseCase.Interfaces;
namespace QualificationsRegister.Tests.Functions
{
    [TestFixture]
    public class Function1FunctionTests
    {
        private Mock<HttpRequestData> _reqMock;
        private Mock<HttpResponseData> _respMock;
        private Mock<ILogger> _logMock;
        private Mock<ILoggerFactory> _loggerFactoryMock;
        private Mock<IGetOrganisationsSearchUseCase> _searchUseCaseMock;
        private Mock<IGetOrganisationByNumberUseCase> _byNumberUseCaseMock;

        [SetUp]
        public void Setup()
        {
            var _m = new Mock<FunctionContext>();
            _reqMock = new Mock<HttpRequestData>(_m.Object);
            _respMock = new Mock<HttpResponseData>();
            _respMock.SetupAllProperties();

            _loggerFactoryMock = new Mock<ILoggerFactory>();
            _logMock = new Mock<ILogger>();
            _loggerFactoryMock.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(_logMock.Object);

            _searchUseCaseMock = new Mock<IGetOrganisationsSearchUseCase>();
            _byNumberUseCaseMock = new Mock<IGetOrganisationByNumberUseCase>();
        }
        [Test]
        public async Task FunctionReturnsOkResponse()
        {
            var httpFunc = new Function1(_loggerFactoryMock.Object, _searchUseCaseMock.Object, _byNumberUseCaseMock.Object);            
            var res = await httpFunc.Run(_reqMock.Object);
            Console.WriteLine(res.StatusCode);
            Assert.That(res, Is.Not.Null);
        }
    }
}
