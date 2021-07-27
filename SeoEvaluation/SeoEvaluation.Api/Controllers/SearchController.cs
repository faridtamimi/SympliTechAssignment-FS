using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SeoEvaluation.Api.DTOs;
using SeoEvaluation.Application.Contracts;
using SeoEvaluation.Entities;

namespace SeoEvaluation.Api.Controllers
{
    public class SearchController : BaseApiController
    {
        private readonly ILogger<SearchController> _logger;
        private readonly ISearchService _searchService;

        public SearchController(ILogger<SearchController> logger, ISearchService searchService)
        {
            _logger = logger;
            _searchService = searchService;
        }

        [HttpPost]
        public async Task<List<RankingResultDto>> GetSearchResults(SearchInputDto searchInputDto)
        {

            _logger.LogInformation("Search Request Received! {0}",
                System.Text.Json.JsonSerializer.Serialize(searchInputDto));

            var validSearchEngines = _searchService.GetSearchEngines();
            if (!searchInputDto.SearchEngines.All(x => validSearchEngines.Contains(x)))
            {
                throw new ValidationException("Search engines are not valid");
            }

            SearchInput searchInput = new()
            {
                Keywords = searchInputDto.Keywords,
                Url = searchInputDto.Url,
                NumberOfResults = searchInputDto.NumberOfResults,
                SearchEngines = searchInputDto.SearchEngines
            };

            Dictionary<string, RankingResult> res = await _searchService.GetRankings(searchInput);

            var resDto = res.Values.Select(
                kvp => new RankingResultDto()
                {
                    SearchEngineName = kvp.SearchEngineName,
                    Rankings = kvp.Rankings.Count == 0 ? "0" : string.Join(",", kvp.Rankings)
                }).ToList();

            return resDto;
        }

        [HttpGet("[action]")]
        public List<string> GetSearchEngineNames()
        {
            return _searchService.GetSearchEngines();
        }
    }
}
