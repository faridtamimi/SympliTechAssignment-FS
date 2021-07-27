using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using SeoEvaluation.Entities;

namespace SeoEvaluation.Infrastructure
{
    public class GoogleSearchEngine : AbstractSearchEngine
    {

        public override string EngineName => "Google";
        static readonly Regex RegexPattern = new ("<div class=\"kCrYT\"><a href=\"\\/url\\?q\\=(?<url>.*?)&amp;sa=U&amp;", RegexOptions.Compiled, TimeSpan.FromSeconds(10));
        protected override Regex UrlPattern => RegexPattern;
        public GoogleSearchEngine(IHttpClientFactory clientFactory) : base(clientFactory)
        {
        }

        protected override HttpRequestMessage CreateHttpRequest(SearchInput input)
        {
            return new (HttpMethod.Get, $"https://www.google.com.au/search?q={input.Keywords.Replace(' ', '+')}&num={input.NumberOfResults}");
        }




    }
}
