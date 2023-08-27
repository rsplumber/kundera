using Core.Auth.Authorizations;
using Data.Caching.Abstractions;
using Data.Caching.Redis.CacheManagements;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Data.Caching.Redis;

public static class CachingOptionsExtension
{
    public static void UseRedis(this CachingOptions cachingOptions, IConfiguration configuration)
    {
        cachingOptions.Services.Decorate<IAuthorizeDataProvider, AuthorizeDataProvider>();
        cachingOptions.Services.AddSingleton<ServiceCacheManagement>();
        cachingOptions.Services.AddSingleton<UserRoleCacheManagement>();
        cachingOptions.Services.AddSingleton<SessionCacheManagement>();
        cachingOptions.Services.AddTransient<EventHandlers>();
        cachingOptions.Services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetSection("Caching:Configs:Connection").Value ??
                                    throw new ArgumentNullException("Caching:Configs:Connection", "Enter Caching:Configs:Connection in appsettings.json");
        });
    }
}