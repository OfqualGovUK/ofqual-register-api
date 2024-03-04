using Ofqual.Common.RegisterAPI.Models.Public;

namespace Ofqual.Common.RegisterAPI.Services
{
    public interface IRedisCacheService
    {
        Task RemoveAsync(string key);
        Task<List<T>> GetAndSetCacheAsync<T>(string key, Func<Task<IEnumerable<T>>> onMiss, int ttlDays=1);
        Task<List<T>?> GetCacheAsync<T>(string key);
        Task SetCacheAsync<T>(string key, T data, int ttlDays = 1);
        //Task<List<QualificationPublic>> GetAndSetCacheAsync<T>(string v, Func<Task<IEnumerable<T>>> value);
    }
}
