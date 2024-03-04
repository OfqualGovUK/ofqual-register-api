namespace Ofqual.Common.RegisterAPI.Services
{
    public interface IRedisCacheService
    {
        public Task<List<T>> GetCacheAsync<T>(string key);
        Task ResetCacheAsync();
    }
}
