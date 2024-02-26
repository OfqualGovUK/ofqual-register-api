using Google.Protobuf.WellKnownTypes;
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
        private readonly IRegisterRepository _registerRepository;

        public RedisCache(ILoggerFactory loggerFactory, IDistributedCache redis, IRegisterRepository registerRepository)
        {
            _logger = loggerFactory.CreateLogger<RedisCache>();
            _redis = redis;
            _registerRepository = registerRepository;
        }

        public async Task<List<T>> GetCache<T>(string key)
        {
            var retrievedData = await GetCacheAsync<List<T>>(key);

            if (retrievedData == null)
            {
                _logger.LogInformation("{} Cache Miss", key);

                retrievedData = SetCacheAsync<T>(key);
            }
            else
            {
                _logger.LogInformation("{} Cache retreived", key);
            }

            return retrievedData;
        }

        private List<T> SetCacheAsync<T>(string key)
        {
            var data = _registerRepository.GetDataAsync();

            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(5)
            };

            var cacheupdateTasks = new List<Task>();

            foreach (var dictKey in data.Keys)
            {
                _logger.LogInformation("Setting {} Cache", dictKey);

                var compressed = Compress(JsonSerializer.Serialize(data[dictKey]));
                Task updateCache = new Task(() => _redis.SetAsync(dictKey, compressed, options));

                cacheupdateTasks.Add(updateCache);

                updateCache.Start();
            }

            Task.WaitAll(cacheupdateTasks.ToArray());

            _logger.LogInformation("Set Cache");

            return (List<T>)data[key];
        }

        private async Task<T?> GetCacheAsync<T>(string key)
        {
            _logger.LogInformation("Checking {} cache", key);

            var compressed = await _redis.GetAsync(key);

            if (compressed == null)
            {
                return default;
            }
            else
            {
                return JsonSerializer.Deserialize<T>(Decompress(compressed));
            }

        }
        public byte[] Compress(string str)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(str);

            using var input = new MemoryStream(Encoding.UTF8.GetBytes(str));
            using var output = new MemoryStream();
            using var stream = new BrotliStream(output, CompressionLevel.Fastest);

            input.CopyTo(stream);
            stream.Flush();

            return output.ToArray();
        }

        public string Decompress(byte[] value)
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
