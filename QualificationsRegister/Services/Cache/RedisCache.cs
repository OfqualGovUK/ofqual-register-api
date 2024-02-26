using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Ofqual.Common.RegisterAPI.Services.Repository;
using System.IO.Compression;
using System.Text;
using System.Text.Json;

namespace Ofqual.Common.RegisterAPI.Services.Cache
{
    public class RedisCache : IRedisCacheService
    {
        private readonly ILogger _logger;
        private readonly IDistributedCache _redis;

        public RedisCache(ILoggerFactory loggerFactory, IDistributedCache redis)
        {
            _logger = loggerFactory.CreateLogger<RedisCache>();
            _redis = redis;
        }

        public async Task<IEnumerable<T>?> GetCacheAsync<T>(string key)
        {
            _logger.LogInformation($"Getting value for key: {key}");
            var compressed = await _redis.GetAsync(key);
            if (compressed != null)
            {
                string value = Decompress(compressed);
                _logger.LogInformation($"Got and decompressed value for key: {key}");
                return JsonSerializer.Deserialize<IEnumerable<T>>(value);
            }

            return null;
        }


        public async Task SetCacheAsync<T>(string key, T data, int ttlDays = 1)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(5)
            };

            var compressed = Compress(JsonSerializer.Serialize(data));
            await _redis.SetAsync(key, compressed, options);
        }

        private byte[] Compress(string str)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(str);

            using var input = new MemoryStream(Encoding.UTF8.GetBytes(str));
            using var output = new MemoryStream();
            using var stream = new BrotliStream(output, CompressionLevel.Fastest);

            input.CopyTo(stream);
            stream.Flush();

            return output.ToArray();
        }

        private string Decompress(byte[] value)
        {
            using var input = new MemoryStream(value);
            using var output = new MemoryStream();
            using var stream = new BrotliStream(input, CompressionMode.Decompress);

            stream.CopyTo(output);
            stream.Flush();

            return Encoding.UTF8.GetString(output.ToArray());
        }
    }
}
