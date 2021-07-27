using Microsoft.Extensions.Caching.Memory;
using Moq;

namespace SeoEvaluation.Tests.Helpers
{
    internal static class Helper
    {
        public static IMemoryCache NeutralMemoryCache { get;}
        static Helper()
        {
            Mock<IMemoryCache> memoryCacheMock = new Mock<IMemoryCache>();
            memoryCacheMock.Setup(x => x.CreateEntry(It.IsAny<object>())).Returns(Mock.Of<ICacheEntry>());
            NeutralMemoryCache = memoryCacheMock.Object;

        }
    }
}
