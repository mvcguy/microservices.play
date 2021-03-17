using System;
using System.Collections.Concurrent;

namespace MG.WebSite.Services
{
    public class InMemoryCache : IAppCache
    {
        private ConcurrentDictionary<string, CachedValue> _cache;

        public InMemoryCache()
        {
            _cache = new ConcurrentDictionary<string, CachedValue>();
        }

        public void SetString(string key, string value, TimeSpan expiry)
        {
            _cache[key] = new CachedValue
            {
                Value = value,
                Expiry = expiry,
                CreatedOn = DateTime.Now
            };
        }

        public string GetString(string key)
        {
            if (!_cache.TryGetValue(key, out var value)) return null;

            if (value.IsExpired)
            {
                if(_cache.TryRemove(key, out _))
                {
                    Console.WriteLine("Key is removed from cache. Reason: Expired");
                }
                return null;
            }

            return value.Value;
        }

        class CachedValue
        {
            public string Value { get; set; }

            public TimeSpan Expiry { get; set; }

            public DateTime CreatedOn { get; set; }

            public bool IsExpired => CreatedOn + Expiry < DateTime.Now;
        }
    }
}
