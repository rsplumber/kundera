using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Data.Seeder;

internal static class ServiceCollectionExtension
{
    public static void AddDataSeeder(this IServiceCollection services)
    {
        services.TryAddScoped<DataSeeder>();
    }
}