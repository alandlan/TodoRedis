using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoRedis.Services.Caching
{
    public interface ICachingService
    {
        Task SetAsync(string key, string value);

        Task<T> GetAsync<T>(string key);

        Task<IEnumerable<T>> GetListAsync<T>(string key);

        Task SetAsync<T>(string key, T value);
    }
}