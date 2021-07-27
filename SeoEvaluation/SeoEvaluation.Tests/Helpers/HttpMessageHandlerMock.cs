using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SeoEvaluation.Tests.Helpers
{
    internal class HttpMessageHandlerMock : HttpMessageHandler
    {
        private readonly byte[] _content;

        public HttpMessageHandlerMock(byte[] content)
        {
            _content = content;
        }
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK) { Content = new ByteArrayContent(_content) });
        }
    }
}