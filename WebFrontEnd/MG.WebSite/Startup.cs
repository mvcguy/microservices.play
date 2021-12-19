using MG.WebSite.Services;
using MG.WebSite.WebClients;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using Polly.Extensions.Http;
using System;
using System.Net.Http;

namespace MG.WebSite
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = "Cookies";
                    options.DefaultChallengeScheme = "oidc";
                })
                .AddCookie("Cookies")
                .AddOpenIdConnect("oidc", options =>
                {
                    options.Authority = "https://localhost:5010";
                    options.ClientId = "mvc";
                    options.ClientSecret = "secret";
                    options.ResponseType = "code";
                    options.SaveTokens = true;
                    options.Scope.Add("profile");
                    options.GetClaimsFromUserInfoEndpoint = true;
                });

            services.AddHttpContextAccessor();
            services.AddControllersWithViews(options =>
            {
                //
                // make sure that ONLY an authenticated user is allowed.
                //
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                var filter = new AuthorizeFilter(policy);
                options.Filters.Add(filter);
            });

            services.AddHttpClient<ProductCategoriesClient>()
                .AddPolicyHandler(GetRetryPolicy())
                .AddPolicyHandler(GetCircuitBreakerPolicy());

            services.AddHttpClient<IAuthTokenService, AuthTokenService>()
                .AddPolicyHandler(GetRetryPolicy())
                .AddPolicyHandler(GetCircuitBreakerPolicy());

            services.AddSingleton<IAppCache, InMemoryCache>();
            ConfigureAppOptions(services);
        }

        private void ConfigureAppOptions(IServiceCollection services)
        {
            services.Configure<AppConfig>(options =>
            {
                TimeSpan.TryParse(Configuration["AppConfig:AppCacheConfig:DefaultExpiry"], out var defaultCacheExpiry);
                options.AppCacheConfig = new AppCacheConfig
                {
                    CacheServerUrl = Configuration["AppConfig:AppCacheConfig:CacheServerUrl"],
                    DefaultExpiry = defaultCacheExpiry
                };
                options.CatalogServiceScopes = Configuration["AppConfig:CatalogServiceScopes"];
                options.CatalogServiceUrl = Configuration["AppConfig:CatalogServiceUrl"];
                options.ClientId = Configuration["AppConfig:ClientId"];
                options.ClientSecret = Configuration["AppConfig:ClientSecret"];
                options.IdServerUrl = Configuration["AppConfig:IdServerUrl"];
            });
        }

        private IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            var policy = HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(3, TimeSpan.FromSeconds(15));
            return policy;
        }

        private IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            var policy = HttpPolicyExtensions.HandleTransientHttpError()
                .WaitAndRetryAsync(10,
                attemptNbr => TimeSpan.FromMilliseconds(Math.Pow(1.5, attemptNbr)),
                (result, waitingTime) =>
                {
                    var message = result.Exception?.Message;
                    Console.WriteLine($"Retrying due to error: '{message}'");
                });

            return policy;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
