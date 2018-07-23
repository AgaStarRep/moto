using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace Motohusaria.Services
{
    [InjectableService(typeof(ICacheService))]
    public class MemoryCacheService : ICacheService
    {
        public object this[string key] => Get<object>(key);

        public void DeleteKey(string key)
        {
        }

        public T Get<T>(string key)
        {
            return default(T);
        }

        public void Store<T>(string key, T data, int cacheTime)
        {
        }

        public void DeleteKey(CacheKey key)
        {
        }

        public void DeleteKeysStartingWith(string key)
        {
        }

    }
}
