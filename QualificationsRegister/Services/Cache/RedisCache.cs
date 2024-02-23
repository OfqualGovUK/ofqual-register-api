using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Ofqual.Common.RegisterAPI.Services.Repository;
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

                retrievedData = await SetCache<T>(key);

            }

            return retrievedData == null ? default : retrievedData;
        }

        public async Task<List<T>> GetSetCache<T>(string key, Func<Task<List<T>>> onMiss, DateTime expiry)
        {
            var existingData = await GetCacheAsync<List<T>>(key);

            if (existingData != null)
            {
                return existingData;
            }
            else
            {
                // _logger.LogInformation(key + " Cache Miss");
                var data = await onMiss();
                await SetCacheAsync(key, await onMiss(), expiry);
                return data;
            }
        }

        public async Task<List<T>> SetCache<T>(string key)
        {
            var data = new List<T>();

            if (key.ToLower() == "organisations")
            {
                data = (List<T>) await _registerRepository.GetAllOrganisationsAsync();
            }

            if (data.Count > 0)
            {
                var options = new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddMinutes(5)
                };

                await _redis.SetStringAsync(key, JsonSerializer.Serialize(data), options);
            }

            return data;
        }

        private async Task<T> GetCacheAsync<T>(string key)
        {
            var value = await _redis.GetStringAsync(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }
        private async Task SetCacheAsync<T>(string key, T value, DateTime expiry)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = expiry
            };
            await _redis.SetStringAsync(key, JsonSerializer.Serialize(value), options);
        }

    }
}
