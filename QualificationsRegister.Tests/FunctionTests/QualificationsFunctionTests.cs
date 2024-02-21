using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Ofqual.Common.RegisterAPI.Functions;
using Ofqual.Common.RegisterAPI.Tests.Mocks;
using Ofqual.Common.RegisterAPI.UseCase.Interfaces;

namespace Ofqual.Common.RegisterAPI.Tests.Functions
{
    [TestFixture]
    public class QualificationsFunctionTests
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
            var httpFunc = new Qualifications(new NullLoggerFactory(), _searchUseCaseMock.Object, _byNumberUseCaseMock.Object);
            MockHttpRequestData requestData = new MockHttpRequestData(_functionContext.Object);
            var res = await httpFunc.ListQualifications(requestData);
            Console.WriteLine(res.StatusCode);
            Assert.That(res.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
        }
    }
}
