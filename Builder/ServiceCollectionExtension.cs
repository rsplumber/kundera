using Auth.Builder;
using Data.Seeder;
using Kite.Cache.InMemory.Extensions.Microsoft.DependencyInjection;
using Kite.Hashing.HMAC;
using Kite.Hashing.HMAC.Extensions.Microsoft.DependencyInjection;
using Kite.Serializer.Microsoft.Extensions.Microsoft.DependencyInjection;
using Managements.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Builder;

public static class ServiceCollectionExtension
{
    public static void AddKundera(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddLibraries();
        services.AddModules(configuration);
        services.AddDataSeeder();
    }

    private static void AddLibraries(this IServiceCollection services)
    {
        services.AddMicrosoftSerializer();
        services.AddCacheInMemory();
        services.AddHashHmac(HashingType.HMACSHA512);
    }

    private static void AddModules(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuth(configuration);
        services.AddManagements(configuration);
    }
}