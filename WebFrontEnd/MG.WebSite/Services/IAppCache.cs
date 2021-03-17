using System;

namespace MG.WebSite.Services
{
    public interface IAppCache
    {
        string GetString(string key);

        void SetString(string key, string value, TimeSpan expiry);
    }
}
