using System.Net.Http;
using System.Text.RegularExpressions;
using SeoEvaluation.Entities;

namespace SeoEvaluation.Infrastructure
{
    public class BingSearchEngine : AbstractSearchEngine
    {
        public override string EngineName => "Bing";
        private static readonly Regex RegexPattern = new ("<li class=\"b_algo\".*?<h2>.*?href=\\\"(?<url>.*?)\\\"");

        protected override HttpRequestMessage CreateHttpRequest(SearchInput input)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"https://www.bing.com/search?q={input.Keywords.Replace(' ', '+')}&count={input.NumberOfResults}");
            httpRequestMessage.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.107 Safari/537.36");
            return httpRequestMessage;
        }
        protected override Regex UrlPattern => RegexPattern;

        public BingSearchEngine(IHttpClientFactory clientFactory) : base(clientFactory)
        {
        }
    }
}
