using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
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
        private readonly IRegisterRepository _registerRepository;

        public RedisCache(ILoggerFactory loggerFactory, IDistributedCache redis, IRegisterRepository registerRepository)
        {
            _logger = loggerFactory.CreateLogger<RedisCache>();
            _redis = redis;
            _registerRepository = registerRepository;
        }

        public async Task<List<T>> GetCacheAsync<T>(string key)
        {
            _logger.LogInformation($"Getting cache value for key: {key}");
            var compressed = await _redis.GetAsync(key);

            if (compressed != null) //Cache is available
            {
                _logger.LogInformation($"Got cache value for key: {key}");
                string value = Decompress(compressed);
                _logger.LogInformation($"Decompressed cache value for key: {key}");
                return JsonSerializer.Deserialize<List<T>>(value)!;
            }
            else //Cache is not available
            {
                var data = await SetCacheAsync<T>(key);
                return data;
            }
        }

        private async Task<List<T>> SetCacheAsync<T>(string key)
        {
            var data = await _registerRepository.GetDataAsync(key);

            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddHours(1)
            };

            _logger.LogInformation("Setting Cache for key: {}", key);
            _logger.LogInformation("Starting Compression for key: {}", key);

            var compressed = Compress(JsonSerializer.Serialize(data));
            _logger.LogInformation("Compressed value for key: {}", key);

            await _redis.SetAsync(key, compressed, options);

            _logger.LogInformation("Set Cache for key: {}", key);

            return (List<T>) data;
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
