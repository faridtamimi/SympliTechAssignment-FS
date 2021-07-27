using System.Collections.Generic;
using System.Threading.Tasks;
using SeoEvaluation.Entities;

namespace SeoEvaluation.Application.Contracts
{
    public interface ISearchEngineDataProvider
    {
        Task<List<SearchResult>> GetSearchResults(SearchInput input);
        string EngineName { get; }
    }
}
