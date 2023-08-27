using Core.Auth.Authorizations;
using Data.Caching.Abstractions;
using Data.Caching.InMemory.CacheManagements;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Data.Caching.InMemory;

public static class CachingOptionsExtension
{
    public static void UseInMemory(this CachingOptions cachingOptions)
    {
        cachingOptions.Services.Decorate<IAuthorizeDataProvider, AuthorizeDataProvider>();
        cachingOptions.Services.AddSingleton<ServiceCacheManagement>();
        cachingOptions.Services.AddSingleton<UserRoleCacheManagement>();
        cachingOptions.Services.AddSingleton<SessionCacheManagement>();
        cachingOptions.Services.AddTransient<EventHandlers>();
        cachingOptions.Services.AddDistributedMemoryCache();
    }
}