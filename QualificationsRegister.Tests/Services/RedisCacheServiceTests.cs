using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Ofqual.Common.RegisterAPI.Services;

namespace OfqualCommon.RegisterAPI.Tests.Services
{
    [TestFixture]
    public class RedisCacheServiceTests
    {
        private Mock<IRedisCacheService> _mockRedisCache;
        IDistributedCache _inMemoryCache;

        [SetUp]
        public void Setup()
        {
            var opts = Options.Create(new MemoryDistributedCacheOptions());
            _inMemoryCache = new MemoryDistributedCache(opts);
        }
    }
}
