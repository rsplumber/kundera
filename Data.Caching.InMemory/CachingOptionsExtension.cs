using Core.Auth.Authorizations;
using Data.Caching.InMemory.CacheManagements;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Data.Caching.InMemory;

public static class CachingOptionsExtension
{
    public static void UseInMemoryCaching(this IServiceCollection services, IConfiguration? configuration = default)
    {
        services.Decorate<IAuthorizeDataProvider, AuthorizeDataProvider>();
        services.AddSingleton<ServiceCacheManagement>();
        services.AddSingleton<UserRoleCacheManagement>();
        services.AddSingleton<SessionCacheManagement>();
        services.AddTransient<EventHandlers>();
        services.AddDistributedMemoryCache();
    }
}