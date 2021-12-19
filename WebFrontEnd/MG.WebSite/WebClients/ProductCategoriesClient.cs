using IdentityModel.Client;
using MG.WebSite.Services;
using Microsoft.Extensions.Options;
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
        private readonly ProductCatalogClient productCatalogClient;
        private readonly HttpClient client;
        private readonly IOptions<AppConfig> appConfig;
        private readonly IAuthTokenService tokenService;
        private readonly IAppCache appCache;

        public ProductCategoriesClient(IOptions<AppConfig> appConfig, HttpClient client, IAuthTokenService tokenService, IAppCache appCache)
        {
            this.appConfig = appConfig;
            this.tokenService = tokenService;
            this.appCache = appCache;
            this.client = client;
            productCatalogClient = new ProductCatalogClient(this.appConfig.Value.CatalogServiceUrl, this.client);
        }
        public async Task<IEnumerable<ProductCategoryDto>> GetProductCategories(int page = 1)
        {
            var token = await this.tokenService.GetTokenAsync();
            client.SetBearerToken(token);
            return await productCatalogClient.GetCategoriesAsync(page);
        }

        public async Task<MetaDataDto> GetProductCategoriesMetaData()
        {
            var cached = appCache.GetString(AppConstants.CategoriesMetaDataKey);
            if (!string.IsNullOrWhiteSpace(cached))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Metadata is found in cache");
                Console.ResetColor();

                return JsonSerializer.Deserialize<MetaDataDto>(cached);
            }

            var token = await tokenService.GetTokenAsync();
            client.SetBearerToken(token);

            var metadata = await productCatalogClient.GetCategoriesMetaDataAsync();

            var cache = JsonSerializer.Serialize(metadata);
            appCache.SetString(AppConstants.CategoriesMetaDataKey, cache, appConfig.Value.AppCacheConfig.DefaultExpiry);

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Metadata is taken from db");
            Console.ResetColor();

            return metadata;
        }

        public async Task<IEnumerable<ProductDto>> GetProductsByCategory(Guid catId, int page = 1)
        {
            var token = await tokenService.GetTokenAsync();
            client.SetBearerToken(token);            
            return await productCatalogClient.GetProductsByCategoryAsync(catId, page);
        }

        public async Task<MetaDataDto> GetProductsMetaData()
        {
            var cached = appCache.GetString(AppConstants.ProductsMetaDataKey);
            if (!string.IsNullOrWhiteSpace(cached))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Metadata is found in cache");
                Console.ResetColor();

                return JsonSerializer.Deserialize<MetaDataDto>(cached);
            }

            var token = await tokenService.GetTokenAsync();
            client.SetBearerToken(token);

            var metadata = await productCatalogClient.GetProductsMetaDataAsync();

            var cache = JsonSerializer.Serialize(metadata);
            appCache.SetString(AppConstants.ProductsMetaDataKey, cache, appConfig.Value.AppCacheConfig.DefaultExpiry);

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Metadata is taken from db");
            Console.ResetColor();

            return metadata;
        }
    }

}
