using IdentityModel.Client;
using MG.WebSite.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace MG.WebSite.WebClients
{
    public class ProductCategoriesClient
    {
        private readonly ProductCategoriesProxy proxy;
        private readonly HttpClient client;
        private readonly IAppCache appCache;
        private string token;

        private string catalogServiceUrl = "https://localhost:5020";
        private string idServerUrl = "https://localhost:5010/";
        private string tokenKey = "mg.website.token";
        private string metaDataKey = "mg.website.categories.metadata";

        public ProductCategoriesClient(HttpClient client, IAppCache appCache)
        {
            this.client = client;
            this.appCache = appCache;
            proxy = new ProductCategoriesProxy(catalogServiceUrl, this.client);
        }


        private async Task InitializeToken()
        {

            if (!string.IsNullOrWhiteSpace(token)) return;

            var cacheToken = appCache.GetString(tokenKey);
            if (!string.IsNullOrWhiteSpace(cacheToken))
            {
                token = cacheToken;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Token is retrieved from cache");
                Console.ResetColor();
                return;
            }

            var idServer = idServerUrl;

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
                { "scope", "catalog.fullaccess" }
            };

            var request = new TokenRequest
            {
                Address = discDoc.TokenEndpoint,
                GrantType = "client_credentials",
                Parameters = scopes,
                ClientId = "mg.website",
                ClientSecret = "1e230c33-6de2-4755-bcbf-333f1afe1ff2"
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

            this.token = tokenResult.AccessToken;
            appCache.SetString(tokenKey, tokenResult.AccessToken,
                TimeSpan.FromSeconds(tokenResult.ExpiresIn - 10));

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Token is retrieved from ID-Server");
            Console.ResetColor();
        }

        public async Task<IEnumerable<ProductCategoryDto>> GetProductCategories(int page = 1)
        {
            await InitializeToken();
            client.SetBearerToken(this.token);
            return await proxy.GetCategoriesAsync(page);
        }

        public async Task<MetaDataDto> GetProductCategoriesMetaData()
        {
            var cached = appCache.GetString(metaDataKey);
            if (!string.IsNullOrWhiteSpace(cached))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Metadata is found in cache");
                Console.ResetColor();

                return JsonSerializer.Deserialize<MetaDataDto>(cached);
            }

            await InitializeToken();
            client.SetBearerToken(this.token);
                        
            var metadata = await proxy.GetCategoriesMetaDataAsync();

            var cache = JsonSerializer.Serialize(metadata);
            appCache.SetString(metaDataKey, cache, TimeSpan.FromMinutes(90));

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Metadata is taken from db");
            Console.ResetColor();

            return metadata;
        }
    }
}
