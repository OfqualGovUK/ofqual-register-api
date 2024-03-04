using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.IO.Compression;
using System.Text;
using System.Text.Json;

namespace Ofqual.Common.RegisterAPI.Services
{
    public class RedisCache : IRedisCacheService
    {
        public static string Organisations = "Organisations";
        public static string Qualifications = "Qualifications";

        private readonly ILogger _logger;
        private readonly IDistributedCache _redis;

        public RedisCache(ILoggerFactory loggerFactory, IDistributedCache redis)
        {
            _logger = loggerFactory.CreateLogger<RedisCache>();
            _redis = redis;
        }

        public async Task<List<T>> GetAndSetCacheAsync<T>(string key, Func<Task<IEnumerable<T>>> onMiss, int ttlDays = 1)
        {
            _logger.LogInformation($"Getting cache value for key: {key}");
            var values = await GetCacheAsync<T>(key);
            if(values != null)
                return values;

            var data = await onMiss();
            _logger.LogInformation("Cache miss for key: {}", key);
            await SetCacheAsync(key, data.ToList());
            return data.ToList();
        }

        public async Task<List<T>?> GetCacheAsync<T>(string key)
        {
            var compressed = await _redis.GetAsync(key);
            if (compressed != null) //Cache is available
            {
                _logger.LogInformation($"Got cache value for key: {key}");
                var value = Decompress(compressed);
                _logger.LogInformation($"Decompressed cache value for key: {key}");
                return JsonSerializer.Deserialize<List<T>>(value)!;
            }
            return null;
        }

        public async Task SetCacheAsync<T>(string key, T data, int ttlDays = 1)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddHours(1)
            };

            _logger.LogInformation("Starting Compression for key: {}", key);
            var compressed = Compress(JsonSerializer.Serialize(data));
            await _redis.SetAsync(key, compressed, options);
        }

        public async Task RemoveAsync(string key)
        {
            _logger.LogInformation($"Removing Cache for key: {key}");
            await _redis.RemoveAsync(key);
        }

        private static byte[] Compress(string str)
        {
            using var input = new MemoryStream(Encoding.UTF8.GetBytes(str));
            using var output = new MemoryStream();
            using var stream = new BrotliStream(output, CompressionLevel.Fastest);

            input.CopyTo(stream);
            stream.Flush();

            return output.ToArray();
        }

        private static string Decompress(byte[] value)
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
