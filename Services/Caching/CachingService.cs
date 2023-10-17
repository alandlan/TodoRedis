using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace TodoRedis.Services.Caching
{
    public class CachingService : ICachingService
    {
        private readonly IDistributedCache _cache;
        private readonly DistributedCacheEntryOptions _options;
        public CachingService(IDistributedCache cache)
        {
            _cache = cache;
            _options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15),
                SlidingExpiration = TimeSpan.FromMinutes(10)
            };
        }

        public async Task<T> GetAsync<T>(string key)
        {
            var result = await _cache.GetStringAsync(key);

            if (result is null)
            {
                return default;
            }

            return JsonSerializer.Deserialize<T>(result);
        }

        public async Task<IEnumerable<T>> GetListAsync<T>(string key)
        {
            var result = await _cache.GetStringAsync(key);
            if (result is null)
            {
                return default;
            }

            return JsonSerializer.Deserialize<IEnumerable<T>>(result);
        }

        public async Task SetAsync(string key, string value)
        {
            await _cache.SetStringAsync(key, value, _options);
        }

        public async Task SetAsync<T>(string id, T value)
        {
            await _cache.SetStringAsync(id, JsonSerializer.Serialize(value), _options);
        }
    }
}