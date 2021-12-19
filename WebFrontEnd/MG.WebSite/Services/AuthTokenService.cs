using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using MG.WebSite.Services;
using Microsoft.Extensions.Options;

namespace MG.WebSite.Services
{
    public interface IAuthTokenService
    {
        Task<string> GetTokenAsync();
    }

    public class AuthTokenService : IAuthTokenService
    {
        private readonly HttpClient client;
        private readonly IAppCache appCache;
        private readonly IOptions<AppConfig> appConfig;

        public AuthTokenService(HttpClient client, IAppCache appCache, IOptions<AppConfig> appConfig)
        {
            this.client = client;
            this.appCache = appCache;
            this.appConfig = appConfig;
        }

        public virtual async Task<string> GetTokenAsync()
        {

            var token = string.Empty;
            var cacheToken = appCache.GetString(AppConstants.TokenCacheKey);
            if (!string.IsNullOrWhiteSpace(cacheToken))
            {
                token = cacheToken;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Token is retrieved from cache");
                Console.ResetColor();
                return token;
            }

            var idServer = appConfig.Value.IdServerUrl;

            var discDoc = await client.GetDiscoveryDocumentAsync(idServer);

            if (discDoc.IsError)
            {
                var error = "Could not get the ID-Server information.";
                if (discDoc.Exception != null)
                {
                    error += $" Error{discDoc.Exception.Message}";
                }

                throw new Exception(error);
            }

            var scopes = new Dictionary<string, string>
            {
                { "scope", appConfig.Value.CatalogServiceScopes }
            };

            var request = new TokenRequest
            {
                Address = discDoc.TokenEndpoint,
                GrantType = AppConstants.GrantTpye,
                Parameters = scopes,
                ClientId = appConfig.Value.ClientId,
                ClientSecret = appConfig.Value.ClientSecret
            };

            var tokenResult = await client.RequestTokenAsync(request);

            if (tokenResult.IsError)
            {
                var error = "Could not get the auth token from ID-Server.";
                if (tokenResult.Exception != null)
                {
                    error += $" Error{tokenResult.Exception.Message}";
                }

                throw new Exception(error);
            }

            token = tokenResult.AccessToken;
            appCache.SetString(AppConstants.TokenCacheKey, tokenResult.AccessToken,
                TimeSpan.FromSeconds(tokenResult.ExpiresIn - 10));

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Token is retrieved from ID-Server");
            Console.ResetColor();

            return token;
        }

    }
}