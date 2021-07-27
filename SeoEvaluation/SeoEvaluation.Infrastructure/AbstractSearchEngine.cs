using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SeoEvaluation.Application.Contracts;
using SeoEvaluation.Entities;
using SeoEvaluation.Entities.Exceptions;

namespace SeoEvaluation.Infrastructure
{
    public abstract class AbstractSearchEngine : ISearchEngineDataProvider
    {
        private readonly IHttpClientFactory _clientFactory;
        public abstract string EngineName { get; }

        protected AbstractSearchEngine(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        protected abstract HttpRequestMessage CreateHttpRequest(SearchInput input);
        protected abstract Regex UrlPattern { get; }

        protected virtual string SanitizeUrl(string url) => url;


        public async Task<List<SearchResult>> GetSearchResults(SearchInput input)
        {
            var request = CreateHttpRequest(input);

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            string searchResultHtml;
            if (response.IsSuccessStatusCode)
                searchResultHtml = await response.Content.ReadAsStringAsync();
            else
                throw new SearchEngineException(EngineName);

            List<SearchResult> results = new();
            foreach (Match m in UrlPattern.Matches(searchResultHtml))
            {
                var extractedUrl = m.Groups["url"].Value;
                extractedUrl = SanitizeUrl(extractedUrl);
                if (!string.IsNullOrWhiteSpace(extractedUrl))
                {
                    results.Add(new SearchResult() { Url = extractedUrl });
                }
            }
            return results;
        }

    }
}
