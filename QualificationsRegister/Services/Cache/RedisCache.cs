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
                //cache missed
                _logger.LogInformation(key + " Cache Miss");

                retrievedData = await SetCacheAsync<T>(key);

            }

            return retrievedData == null ? default : retrievedData;
        }

        private async Task<List<T>> SetCacheAsync<T>(string key)
        {
            var data = _registerRepository.GetDataAsync().Result;

            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(5)
            };

            var cacheupdateTasks = new List<Task>();

            foreach (var dictKey in data.Keys)
            {
                var compressed = CompressAsync(JsonSerializer.Serialize(data[dictKey]));
                Task updateCache = new Task(() => _redis.SetAsync(dictKey, compressed, options));

                cacheupdateTasks.Add(updateCache);

                updateCache.Start();
            }

            Task.WaitAll(cacheupdateTasks.ToArray());

            return (List<T>) data[key];
        }

        private async Task<T> GetCacheAsync<T>(string key)
        {
            var compressed = await _redis.GetAsync(key);

            string value = null;

            if (compressed != null)
            {
                value = DecompressAsync(compressed);
            }

            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }


        public byte[] CompressAsync(string str)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(str);

            using var input = new MemoryStream(Encoding.UTF8.GetBytes(str));
            using var output = new MemoryStream();
            using var stream = new BrotliStream(output, CompressionLevel.Fastest);

            input.CopyTo(stream);
            stream.Flush();

            return output.ToArray();
        }

        public string DecompressAsync(byte[] value)
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
