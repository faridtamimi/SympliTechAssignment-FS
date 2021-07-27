using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using SeoEvaluation.Entities;
using SeoEvaluation.Infrastructure;
using SeoEvaluation.Tests.Helpers;
using Xunit;

namespace SeoEvaluation.Tests.BingSearchEngineTests
{
    public class BingSearchEngineTest
    {
        [Fact]
        public async Task GetBingSearchResults_MockResult_CheckSearchResult()
        {
            var fileStream = await File.ReadAllBytesAsync("BingSearchEngineTests\\Bing.html");
            var mockRepository = new Mock<IHttpClientFactory>();
            mockRepository.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(new HttpClient(new HttpMessageHandlerMock(fileStream)));
            var httpClientFactory = mockRepository.Object;

            var bingSearchEngine = new BingSearchEngine(httpClientFactory);
            var res = await bingSearchEngine.GetSearchResults(new SearchInput() { Keywords = string.Empty});
            Assert.NotNull(res);
            Assert.Empty(res.FindAll(x => string.IsNullOrWhiteSpace(x.Url)));
            Assert.Equal("https://www.sympli.com.au/", res[1].Url);
        }



    }
}
