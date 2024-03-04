using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Ofqual.Common.RegisterAPI.Services;
using Microsoft.Extensions.Logging.Abstractions;
using AutoFixture;
using System.Text;
using System.Text.Json;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using System.Diagnostics;
using FluentAssertions;

namespace OfqualCommon.RegisterAPI.Tests.Services
{
    [TestFixture]
    public class RedisCacheServiceTests
    {
        private RedisCache _redisCache;
        IDistributedCache _inMemoryCache;
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            var opts = Options.Create(new MemoryDistributedCacheOptions());
            _inMemoryCache = new MemoryDistributedCache(opts);
            _redisCache = new RedisCache(new NullLoggerFactory(), _inMemoryCache);
            _fixture = new Fixture();
        }

        [Test]
        [TestCase("testkey")]
        public async Task GetsValueWithCorrecteKeyFromCache(string key)
        {
            var stubbedList = _fixture.Create<List<string>>();

            await _redisCache.SetCacheAsync(key, stubbedList);
            var output = await _redisCache.GetCacheAsync<string>(key);

            output.Should().NotBeNullOrEmpty();
            output.Should().BeEquivalentTo(stubbedList);
        }

        [Test]
        [TestCase("testkey")]
        public async Task DoesnotGetValueWithCorrecteKeyFromCache(string key)
        {
            var stubbedList = _fixture.Create<List<string>>();

            await _redisCache.SetCacheAsync(key, stubbedList);
            var output = await _redisCache.GetCacheAsync<string>("incorrectKey");

            output.Should().BeNull();
        }


        [Test]
        public async Task GetsAndSetValueFromCache()
        {
            var stubbedList = _fixture.Create<IEnumerable<string>>();

            var mockFunc = new Mock<Func<Task<IEnumerable<string>>>>();
            mockFunc.Setup(m => m()).ReturnsAsync(stubbedList);
            var output = await _redisCache.GetAndSetCacheAsync<string>("key", mockFunc.Object);
            output.Should().NotBeNullOrEmpty();
            output.Should().BeEquivalentTo(stubbedList);
        }
    }
}
