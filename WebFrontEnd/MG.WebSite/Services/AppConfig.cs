namespace MG.WebSite.Services
{
    public class AppConfig
    {
        public string ConnectionString { get; set; }

        public string CatalogServiceUrl { get; set; }

        public string CatalogServiceScopes{ get; set; }

        public string IdServerUrl{ get; set; }

        public string ClientId { get; set; }

        public string ClientSecret{ get; set; }

        public AppCacheConfig AppCacheConfig { get; set; }
    }
}