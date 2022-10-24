using Microsoft.Extensions.DependencyInjection;

namespace Data.Seeder;

internal static class ServiceCollectionExtension
{
    public static void AddDataSeeder(this IServiceCollection services)
    {
        services.AddTransient<DataSeeder>();
    }
}