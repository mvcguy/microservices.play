using MG.Services.Catalog.Domain;
using MG.Services.Catalog.Domain.InMemoryStores;
using MG.Services.Catalog.Domain.Repositories;
using MG.Services.Catalog.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;

namespace MG.Services.Catalog
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
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = "https://localhost:5010";
                    options.Audience = "mg.services.catalog:API";
                });

            services.AddControllers(options =>
            {
                options.Filters.Add(GetAuthorizeFilter());
                options.Filters.Add(GetExceptionsFilter());
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("catalog.fullaccess",
                    p => p.RequireClaim("scope", "catalog.fullaccess"));
            });

            services.AddSwaggerGen(ConfigureSwagger());

            SetupDomainObjects(services);
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Moniba Garments Catalog API V1");


            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static Action<SwaggerGenOptions> ConfigureSwagger()
        {
            return options =>
            {
                options.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "Moniba Garments Catalog API",
                        Version = "v1",
                    });


                options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        ClientCredentials = new OpenApiOAuthFlow
                        {
                            TokenUrl = new Uri("https://localhost:5010/connect/token"),
                            Scopes = { { "catalog.fullaccess", "gives full access" }, { "catalog.readonlyaccess", "gives only readonly access" } }
                        },

                    },
                    In = ParameterLocation.Header,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,

                });



                options.AddSecurityRequirement(new OpenApiSecurityRequirement{ { new OpenApiSecurityScheme
                                {
                                    Reference = new OpenApiReference
                                    {
                                        Type = ReferenceType.SecurityScheme,
                                        Id = JwtBearerDefaults.AuthenticationScheme
                                    }
                                }, new string[] {} }  });
            };
        }

        private IFilterMetadata GetAuthorizeFilter()
        {
            //
            // make sure that ONLY an authenticated user is allowed.
            //
            var policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
            return new AuthorizeFilter(policy);
        }

        private IFilterMetadata GetExceptionsFilter()
        {
            return new ExceptionsFilter();
        }

        private void SetupDomainObjects(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddSingleton(provider =>
            {
                return new PaginationPolicy { MaxPageSize = 25 };
            });

            services.AddSingleton(provider =>
            {
                var fakeProductCatalog = new ProductCategoriesInMemory();
                fakeProductCatalog.Seed();
                return fakeProductCatalog;
            });

            //services.AddScoped<IProductCategoriesRepository,
            //    ProductCategoryRepositoryInMemory>();

            services.AddScoped<IProductCategoriesRepository,
              ProductCategoriesRepository>();

            services.AddScoped<ProductCategoriesDomain>();
        }
    }
}
