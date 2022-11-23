using Analytics.Builder;
using Auth.Builder;
using Data.Seeder;
using Kite.Hashing.HMAC;
using Kite.Hashing.HMAC.Extensions.Microsoft.DependencyInjection;
using Managements.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Savorboard.CAP.InMemoryMessageQueue;

namespace Builder;

public static class ServiceCollectionExtension
{
    public static void AddKundera(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddLibraries(configuration);
        services.AddModules(configuration);
        services.AddDataSeeder();
    }

    private static void AddLibraries(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHashHmac(HashingType.HMACSHA512);
        services.AddMediator();
        services.AddCap(x =>
        {
            var connectionString = configuration.GetConnectionString("EventSourcing");
            if (connectionString is null)
            {
                throw new ArgumentNullException(nameof(connectionString), "Enter EventSourcing connection string");
            }

            x.UseSqlServer(connectionString);
            x.UseInMemoryMessageQueue();
        });
    }

    private static void AddModules(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuth(configuration);
        services.AddAnalytics(configuration);
        services.AddManagements();
    }
}