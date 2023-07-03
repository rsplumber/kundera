using Core.Auth.Authorizations;
using Data.Caching.CacheManagements;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Data.Caching;

public static class ServiceCollectionExtension
{
    public static void AddCaching(this IServiceCollection services, IConfiguration configuration)
    {
        if (configuration.GetSection("Caching:Enabled").Value is null || !bool.Parse(configuration.GetSection("Caching:Enabled").Value!))
        {
            return;
        }

        services.AddScoped<IAuthorizeDataProvider, AuthorizeDataProvider>();
        services.AddSingleton<ServiceCacheManagement>();
        services.AddSingleton<UserRoleCacheManagement>();
        services.AddSingleton<SessionCacheManagement>();
        services.AddTransient<EventHandlers>();
        var cacheType = configuration.GetSection("Caching:Type").Value;
        switch (cacheType)
        {
            case "Redis":
                services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = configuration.GetSection("Caching:Configs:Connection").Value ??
                                            throw new ArgumentNullException("Caching:Configs:Connection", "Enter Caching:Configs:Connection in appsettings.json");
                });
                break;

            case "InMemory":
                services.AddDistributedMemoryCache();
                services.AddSingleton<IDistributedCache, MemoryCacheWrapper>();
                break;
            default:
                services.AddDistributedMemoryCache();
                services.AddSingleton<IDistributedCache, MemoryCacheWrapper>();
                break;
        }
    }
}