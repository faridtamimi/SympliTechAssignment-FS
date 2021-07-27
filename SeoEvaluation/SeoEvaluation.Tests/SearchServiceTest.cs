using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using SeoEvaluation.Application.Contracts;
using SeoEvaluation.Application.Services;
using SeoEvaluation.Entities;
using SeoEvaluation.Tests.Helpers;
using Xunit;

namespace SeoEvaluation.Tests
{
    public class SearchServiceTest
    {

        private readonly Mock<ISearchEngineDataProvider> _mockRepository = new();
        [Fact]
        public async Task GetRankings_EmptySearchEngineList_ReturnsEmptyRankings()
        {
            // For Testing Environment Setup
            //Arrange
            var searchService = new SearchService(new ISearchEngineDataProvider[0], Helper.NeutralMemoryCache );

            //Act
            var res = await searchService.GetRankings(new SearchInput());
            //Assert
            Assert.NotNull(res);
            Assert.Empty(res);
        }

        [Fact]
        public async Task GetRankings_EmptyResult_ReturnsEmptyRankings()
        {
            _mockRepository.Setup(x => x.GetSearchResults(It.IsAny<SearchInput>()))
                .ReturnsAsync(new List<SearchResult>());
            const string engineName = "Mock";
            _mockRepository.SetupGet(x => x.EngineName).Returns(engineName);
            
            ISearchEngineDataProvider mockSearchEngineDataProvider = _mockRepository.Object;
            
            var searchService = new SearchService(new[] { mockSearchEngineDataProvider }, Helper.NeutralMemoryCache);
            
            IDictionary<string, RankingResult> res = await searchService.GetRankings(new SearchInput());
            
            Assert.NotNull(res);
            Assert.Single(res);
            Assert.Contains(engineName, res);
            Assert.Empty(res[engineName].Rankings);
        }


        [Fact]
        public async Task GetRankings_SingleResult_ReturnsSingleRankings()
        {
            _mockRepository.Setup(x => x.GetSearchResults(It.IsAny<SearchInput>()))
                .ReturnsAsync(new List<SearchResult>(){new (){Url = "https://www.sympli.com.au"}});
            const string engineName = "Mock";
            _mockRepository.SetupGet(x => x.EngineName).Returns(engineName);
            ISearchEngineDataProvider mockSearchEngineDataProvider = _mockRepository.Object;
            
            var searchService = new SearchService(new[] { mockSearchEngineDataProvider }, Helper.NeutralMemoryCache);
            
            IDictionary<string, RankingResult> res = await searchService.GetRankings(new SearchInput() {Url = "www.sympli.com.au" });
            
            Assert.NotNull(res);
            Assert.Single(res);
            Assert.Contains(engineName, res);
            Assert.Single(res[engineName].Rankings);
            Assert.Equal(1, res[engineName].Rankings.First());
        }

        [Fact]
        public async Task GetRankings_SingleResultWithoutMatch_ReturnsEmptyRankings()
        {
            _mockRepository.Setup(x => x.GetSearchResults(It.IsAny<SearchInput>()))
                .ReturnsAsync(new List<SearchResult>() { new () { Url = "https://www.yahoo.com.au" } });
            const string engineName = "Mock";
            _mockRepository.SetupGet(x => x.EngineName).Returns(engineName);
            ISearchEngineDataProvider mockSearchEngineDataProvider = _mockRepository.Object;
            
            var searchService = new SearchService(new[] { mockSearchEngineDataProvider }, Helper.NeutralMemoryCache);
            
            IDictionary<string, RankingResult> res = await searchService.GetRankings(new SearchInput() { Url = "www.sympli.com.au" });
            
            Assert.NotNull(res);
            Assert.Single(res);
            Assert.Contains(engineName, res);
            Assert.Empty(res[engineName].Rankings);
        }
    }
}
