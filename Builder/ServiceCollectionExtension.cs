using Auth.Builder;
using Data.Seeder;
using Kite.Cache.InMemory;
using Kite.Hashing.HMAC;
using Kite.Serializer.Microsoft;
using Managements.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Builder;

public static class ServiceCollectionExtension
{
    public static void AddKundera(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddLibrariesDependencies();

        services.AddModules(configuration);

        services.AddDataSeeder();
    }

    private static void AddLibrariesDependencies(this IServiceCollection services)
    {
        services.AddMicrosoftSerializer();
        services.AddCacheInMemory();
        services.AddHashHmac();
    }

    private static void AddModules(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuth(configuration);
        services.AddManagements(configuration);
    }
}