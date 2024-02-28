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
        // Allow only one thread to access at a time    
        private readonly SemaphoreSlim _lock = new SemaphoreSlim(1, 1);
        private readonly ILogger _logger;
        private readonly IDistributedCache _redis;
        private readonly IRegisterRepository _registerRepository;

        public RedisCache(ILoggerFactory loggerFactory, IDistributedCache redis, IRegisterRepository registerRepository)
        {
            _logger = loggerFactory.CreateLogger<RedisCache>();
            _redis = redis;
            _registerRepository = registerRepository;
        }

        public async Task<List<T>> GetCache<T>(string key)
        {
            var retrievedData = RetrieveCache<List<T>>(key);

            if (retrievedData == null)
            {

                await _lock.WaitAsync();
                _logger.LogInformation("{} Cache Miss. Waiting for update", key);
                _logger.LogDebug("{} Cache Miss. Waiting for update - {}", key, Environment.CurrentManagedThreadId);

                try
                {
                    if (RetrieveCache<List<T>>(key) == null)
                    {
                        retrievedData = await SetCacheAsync<T>(key);
                    }
                }
                finally
                {
                    _lock.Release();
                }
            }
            else
            {
                _logger.LogInformation("{} Cache retreived", key);
                _logger.LogDebug("{} Cache retreived - {}", key, Environment.CurrentManagedThreadId);
            }

            //retrieve data for the pending threads if data is null
            return retrievedData ?? RetrieveCache<List<T>>(key)!;
        }

        private async Task<List<T>> SetCacheAsync<T>(string key, int ttlDays = 1)
        {
            var data = await _registerRepository.GetDataAsync(key);

            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(2)
            };

            _logger.LogInformation("Setting Cache with key: {} ", key);

            var compressed = Compress(JsonSerializer.Serialize(data));
            _logger.LogInformation("Got and decompressed value for key: {}", key);

            await _redis.SetAsync(key, compressed, options);

            _logger.LogInformation("Set Cache");

            return (List<T>) data;
        }

        private T? RetrieveCache<T>(string key)
        {
            var compressed = _redis.Get(key);

            if (compressed == null)
            {
                return default;
            }

            return JsonSerializer.Deserialize<T>(Decompress(compressed));
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
