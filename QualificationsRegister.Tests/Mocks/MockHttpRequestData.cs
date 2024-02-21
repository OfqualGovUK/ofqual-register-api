using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace QualificationsRegister.Tests.Mocks
{
    public sealed class MockHttpRequestData : HttpRequestData
    {
        private readonly FunctionContext _context;
        public MockHttpRequestData(FunctionContext context) : base(context)
        {
            this._context = context;
        }

        public override HttpResponseData CreateResponse()
        {
            return new MockHttpResponseData(_context);
        }

        public override Stream Body { get; }
        public override HttpHeadersCollection Headers { get; }
        public override IReadOnlyCollection<IHttpCookie> Cookies { get; }
        public override Uri Url { get; }
        public override IEnumerable<ClaimsIdentity> Identities { get; }
        public override string Method { get; }
    }
}
