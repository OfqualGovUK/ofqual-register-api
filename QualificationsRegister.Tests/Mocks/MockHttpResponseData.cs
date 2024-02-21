using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker;
using System.Net;

namespace QualificationsRegister.Tests.Mocks
{
    public sealed class MockHttpResponseData : HttpResponseData
    {
        public MockHttpResponseData(FunctionContext context) : base(context)
        {
            Headers = new HttpHeadersCollection();
        }

        public override HttpStatusCode StatusCode { get; set; }
        public override HttpHeadersCollection Headers { get; set; } //= new HttpHeadersCollection();
        public override Stream Body { get; set; } = new MemoryStream();
        public override HttpCookies Cookies { get; }
    }
}
