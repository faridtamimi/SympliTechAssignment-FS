using System.Collections.Generic;
using System.Threading.Tasks;
using SeoEvaluation.Entities;

namespace SeoEvaluation.Application.Contracts
{
    public interface ISearchService
    {
        Task<Dictionary<string, RankingResult>> GetRankings(SearchInput input);
        List<string> GetSearchEngines();
    }
}
