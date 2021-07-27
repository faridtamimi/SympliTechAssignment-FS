using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using SeoEvaluation.Entities;
using SeoEvaluation.Infrastructure;
using SeoEvaluation.Tests.Helpers;
using Xunit;

namespace SeoEvaluation.Tests.GoogleSearchEngineTests
{
    public class GoogleSearchEngineTest
    {
        [Fact]
        public async Task GetGoogleSearchResults_MockResult_CheckSearchResult()
        {
            var fileStream = await File.ReadAllBytesAsync("GoogleSearchEngineTests\\Google.html");
            var mockRepository = new Mock<IHttpClientFactory>();
            mockRepository.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(new HttpClient(new HttpMessageHandlerMock(fileStream)));
            var httpClientFactory = mockRepository.Object;

            var googleSearchEngine = new GoogleSearchEngine(httpClientFactory);
            var res = await googleSearchEngine.GetSearchResults(new SearchInput() { Keywords = string.Empty});
            Assert.NotNull(res);
            Assert.Equal(101, res.Count);
            Assert.Empty(res.FindAll(x => string.IsNullOrWhiteSpace(x.Url)));
            Assert.Equal("https://www.sympli.com.au/", res[0].Url);
        }



    }
}
