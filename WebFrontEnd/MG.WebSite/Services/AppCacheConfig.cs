using System;

namespace MG.WebSite.Services
{
    public class AppCacheConfig
    {
        public string CacheServerUrl { get; set; }

        public TimeSpan DefaultExpiry { get; set; }
    }
}