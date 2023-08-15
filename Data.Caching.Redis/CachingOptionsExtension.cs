using Core.Auth.Authorizations;
using Data.Caching.Redis.CacheManagements;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Data.Caching.Redis;

public static class CachingOptionsExtension
{
    public static void UseRedisCaching(this IServiceCollection services, IConfiguration configuration)
    {
        services.Decorate<IAuthorizeDataProvider, AuthorizeDataProvider>();
        services.AddSingleton<ServiceCacheManagement>();
        services.AddSingleton<UserRoleCacheManagement>();
        services.AddSingleton<SessionCacheManagement>();
        services.AddTransient<EventHandlers>();
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetSection("Caching:Configs:Connection").Value ??
                                    throw new ArgumentNullException("Caching:Configs:Connection", "Enter Caching:Configs:Connection in appsettings.json");
        });
    }
}