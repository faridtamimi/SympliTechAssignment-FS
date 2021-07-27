using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using SeoEvaluation.Application.Contracts;
using SeoEvaluation.Entities;

namespace SeoEvaluation.Application.Services
{
    public class SearchService : ISearchService
    {
        private readonly IEnumerable<ISearchEngineDataProvider> _searchEngineDataProvider;
        private readonly IMemoryCache _memoryCache;
        public SearchService(IEnumerable<ISearchEngineDataProvider> searchEngineDataProvider, IMemoryCache memoryCache)
        {
            _searchEngineDataProvider = searchEngineDataProvider;
            _memoryCache = memoryCache;
        }

        public List<string> GetSearchEngines()
        {
            return  _searchEngineDataProvider.Select(x => x.EngineName).ToList();
        }

        public async Task<Dictionary<string, RankingResult>> GetRankings(SearchInput input)
        {
            var searchEngineDataProvider = _searchEngineDataProvider;
            if (input.SearchEngines != null && input.SearchEngines.Any())
            {
                searchEngineDataProvider = searchEngineDataProvider.Where(x => input.SearchEngines.Contains(x.EngineName));
            }

            // The engine can fetch data in parallel
            var tasks = searchEngineDataProvider.Select(item => GetSearchResult(input, item));

            var results = await Task.WhenAll(tasks);
            return results.ToDictionary(k => k.SearchEngineName, v => v);
        }

        private async Task<RankingResult> GetSearchResult(SearchInput input, ISearchEngineDataProvider searchEngine)
        {
            var searchCacheKey = $"search_cache_key_{searchEngine.EngineName}_{input.Keywords}_{input.Url}_{input.NumberOfResults}";
            return await _memoryCache.GetOrCreateAsync(searchCacheKey, async cacheEntry =>
            {
                cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);
                var results = await searchEngine.GetSearchResults(input);

                RankingResult rankingResult = new();
                rankingResult.Rankings = results.Select((x, i) => new { i, isContain = x.Url.Contains(input.Url) })
                    .Where(x => x.isContain).Select(x => x.i + 1).ToList();
                rankingResult.SearchEngineName = searchEngine.EngineName;
                return rankingResult;
            });
        }


    }
}
