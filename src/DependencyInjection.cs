using ConfigurationSubstitution;
using Cronos;
using ApiShortUrl.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using ApiShortUrl.Services.UrlService;
using ApiShortUrl.Models.Settings;
using Microsoft.OpenApi.Models;
using ApiShortUrl.Services.Redis;

namespace ApiShortUrl
{
    public static class DependencyInjection
    {
        public static void Inject(this IServiceCollection services)
        {
            var config = new ConfigurationBuilder()
                 .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                 .AddJsonFile($"appsettings.json", optional: true, reloadOnChange: true)
                 .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true, reloadOnChange: true)
                 .AddEnvironmentVariables()
                 .EnableSubstitutions("%", "%")
                 .Build();

            services.InjectConfigurations(config);
            services.InjectAuthentions();
            services.InjectRedis(config);
            services.InjectServices();
            services.InjectSwagger();
            services.InjectCors();
        }
        private static void InjectServices(this IServiceCollection services)
        {
            services.AddAuthorization();
            services.AddSingleton<IRedisService, RedisService>();
            services.AddSingleton<IUrlService, UrlService>();
        }


        private static void InjectAuthentions(this IServiceCollection services)
        {
            services.AddAuthentication("ApiKey")
                 .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>
                 ("ApiKey", null);

        }

        private static void InjectRedis(this IServiceCollection services, IConfigurationRoot config)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = GetRedisContext();
            });

            string GetRedisContext()
            {
                var redisContextUrl = config.GetConnectionString("Redis");
                Uri redisUrl;
                bool isRedisUrl = Uri.TryCreate(redisContextUrl, UriKind.Absolute, out redisUrl);
                if (isRedisUrl)
                {
                    redisContextUrl = string.Format("{0}:{1},password={2}", redisUrl.Host, redisUrl.Port, redisUrl.UserInfo.Split(':')[1]);
                }
                return redisContextUrl;
            }
        }

        private static void InjectConfigurations(this IServiceCollection services, IConfigurationRoot config)
        {
            services.Configure<ApiConfig>(options => config.GetSection("ApiConfig").Bind(options));
        }

        private static void InjectCors(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var apiConfig = serviceProvider.GetRequiredService<IOptions<ApiConfig>>().Value;
            services.AddCors(p => p.AddPolicy("corsapp",
            builder =>
            {
                builder
                .WithOrigins(apiConfig.CorsOrigin)
                .AllowAnyMethod()
                .AllowAnyHeader();
            }));
        }

        private static void InjectSwagger(this IServiceCollection services)
        {
            var contact = new OpenApiContact()
            {
                Name = "Roberto Paes",
                Email = "contato@robertinho.net",
                Url = new Uri("https://robertocpaes.dev")
            };
            var info = new OpenApiInfo()
            {
                Version = "v1",
                Title = "Api Short Url",
                Description = "Minimal API",
                Contact = contact
            };
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", info);
                c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
                {
                    Description = "Insert the apikey above.",
                    Type = SecuritySchemeType.ApiKey,
                    Name = "ApiKey",
                    In = ParameterLocation.Header,
                    Scheme = "ApiKeyScheme"
                });
                var key = new OpenApiSecurityScheme()
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "ApiKey"
                    },
                    In = ParameterLocation.Header
                };
                var requirement = new OpenApiSecurityRequirement
                    {
                             { key, new List<string>() }
                    };
                c.AddSecurityRequirement(requirement);
            });
        }
    }
}
