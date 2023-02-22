using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Seeders;

public static class ServiceCollectionExtension
{
    public static void AddDataSeeders(this IServiceCollection services, IConfiguration configuration)
    {
        services.TryAddTransient<DataSeeder>();
        services.AddHostedService<SeedBackgroundWorker>();
    }
}